namespace EscortBookVault.Extensions

open System
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Options
open Microsoft.Azure.Cosmos.Table
open EscortBookVault.Models
open EscortBookVault.Repositories

module AzureTableExtensions =

    type IServiceCollection with

        member this.AddAzureTableStorage(options: Action<AzureTableClientOptions>) =
            this.Configure<AzureTableClientOptions>(options) |> ignore
            
            this.AddScoped<IAzureTableClient>(Func<_,_>(fun (sp: IServiceProvider) ->
                    let options = sp.GetRequiredService<IOptions<AzureTableClientOptions>>()
                    new AzureTableClient(options.Value) :> IAzureTableClient)) |> ignore

            this

    type CloudTable with

        member this.InsertOrMergeEntityAsync<'a when 'a :> ITableEntity>(entity: 'a) =
            async {
                let operation = TableOperation.InsertOrMerge(entity)
                let! result = this.ExecuteAsync(operation) |> Async.AwaitTask

                return result.Result
            }

        member this.GetEntityUsingPointQueryAsync<'a when 'a :> ITableEntity>(partitionKey: string, rowKey: string) =
            async {
                let operation = TableOperation.Retrieve<'a>(partitionKey, rowKey)
                let! result = this.ExecuteAsync(operation) |> Async.AwaitTask

                return result.Result
            }

        member this.DeleteEntityAsync<'a when 'a :> ITableEntity>(entity: 'a) =
            async {
                let operation = TableOperation.Delete(entity)
                let! result = this.ExecuteAsync(operation) |> Async.AwaitTask

                return result
            }