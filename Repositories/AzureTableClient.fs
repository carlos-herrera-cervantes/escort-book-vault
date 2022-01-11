namespace EscortBookVault.Repositories

open System
open Microsoft.Azure.Cosmos.Table
open EscortBookVault.Models

type AzureTableClient private () =

    member val _cloudStorageAccount : CloudStorageAccount = null with get, set

    member val _cloudTableClient : CloudTableClient = null with get, set

    new (azureTableClientOptions : AzureTableClientOptions) as this = new AzureTableClient() then
        this._cloudStorageAccount <- CloudStorageAccount.Parse(azureTableClientOptions.AzureStorageConnectionString)
        this._cloudTableClient <- this._cloudStorageAccount.CreateCloudTableClient(azureTableClientOptions)

    interface IAzureTableClient with

        member this.CreateIfNotExists (table: string) =
            async {
                let cloudTable = this._cloudTableClient.GetTableReference(table)
                let! _ = cloudTable.CreateIfNotExistsAsync() |> Async.AwaitTask
                
                return cloudTable
            }

    interface IDisposable with

        member this.Dispose () =
            try
                this._cloudStorageAccount <- null
                this._cloudTableClient <- null
            with
                | _ -> printf "We got an exception"