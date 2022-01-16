namespace EscortBookVault.Controllers

open System
open Microsoft.AspNetCore.Mvc
open AutoMapper
open System.Linq
open Microsoft.Azure.Cosmos.Table
open EscortBookVault.Repositories
open EscortBookVault.Models
open EscortBookVault.Extensions.AzureTableExtensions

[<Route("api/v1/secrets")>]
[<Produces("application/json")>]
[<ApiController>]
type SecretController private () =
    inherit ControllerBase()

    member val _azureTableClient: IAzureTableClient = null with get, set

    member val _mapper: IMapper = null with get, set

    new (azureTableClient: IAzureTableClient, mapper: IMapper) as this = SecretController() then
        this._azureTableClient <- azureTableClient
        this._mapper <- mapper

    [<HttpGet("one")>]
    member this.GetOneAsync ([<FromQuery>] query: PointQueryDto) =
        async {
            let! cloudTable = this._azureTableClient.CreateIfNotExists(sprintf "%ss" (typeof<Secret>.Name.ToLowerInvariant()))
            let! finded = cloudTable.GetEntityUsingPointQueryAsync<Secret>(query.PartitionKey, query.RowKey)
            let response = SuccessSecretResponse()
            
            response.Data <- this._mapper.Map<SingleSecretDto>(finded)

            return response |> this.Ok :> IActionResult
        }

    [<HttpGet("row-key")>]
    member this.GetByRowKeyAsync ([<FromQuery>] query: RowKeyDto) =
        async {
            let! cloudTable = this._azureTableClient.CreateIfNotExists(sprintf "%ss" (typeof<Secret>.Name.ToLowerInvariant()))
            let filter = TableQuery<Secret>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, query.RowKey))
            let! finded = cloudTable.ExecuteQuerySegmentedAsync(filter, null) |> Async.AwaitTask
            let response = SuccessSecretResponse()
            
            response.Data <- this._mapper.Map<SingleSecretDto>(finded.Results.First())

            return response |> this.Ok :> IActionResult
        }

    [<HttpPost>]
    member this.CreateAsync ([<FromBody>] secret: Secret) =
        async {
            let! cloudTable = this._azureTableClient.CreateIfNotExists(sprintf "%ss" (typeof<Secret>.Name.ToLowerInvariant()))

            secret.PartitionKey <- Guid.NewGuid().ToString()

            let! created = cloudTable.InsertOrMergeEntityAsync<Secret>(secret)
            let response = SuccessSecretResponse()

            response.Data <- this._mapper.Map<SingleSecretDto>(created)

            return this.Created("", response) :> IActionResult
        }

    [<HttpPatch("one")>]
    member this.UpdateOneAsync ([<FromQuery>] query: PointQueryDto, [<FromBody>] body: UpdateSecretDto) =
        async {
            let! cloudTable = this._azureTableClient.CreateIfNotExists(sprintf "%ss" (typeof<Secret>.Name.ToLowerInvariant()))
            let! finded = cloudTable.GetEntityUsingPointQueryAsync<Secret>(query.PartitionKey, query.RowKey)

            let secret = this._mapper.Map<Secret>(finded)
            let! _ = cloudTable.DeleteEntityAsync<Secret>(secret)

            if body.SecretValue <> null then
                secret.SecretValue <- body.SecretValue

            secret.UpdatedAt <- DateTime.UtcNow

            let! created = cloudTable.InsertOrMergeEntityAsync<Secret>(secret)
            let response = SuccessSecretResponse()
            let dto = this._mapper.Map<SingleSecretDto>(created)

            response.Data <- dto

            return response |> this.Ok :> IActionResult
        }

    [<HttpDelete("one")>]
    member this.DeleteOneAsync ([<FromQuery>] query: PointQueryDto) =
        async {
            let! cloudTable = this._azureTableClient.CreateIfNotExists(sprintf "%ss" (typeof<Secret>.Name.ToLowerInvariant()))
            let! finded = cloudTable.GetEntityUsingPointQueryAsync<Secret>(query.PartitionKey, query.RowKey)
            let secret = this._mapper.Map<Secret>(finded)
            let! _ = cloudTable.DeleteEntityAsync<Secret>(secret)

            return this.NoContent() :> IActionResult
        }