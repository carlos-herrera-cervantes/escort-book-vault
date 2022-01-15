namespace EscortBookVault.Models

open Microsoft.Azure.Cosmos.Table

[<AllowNullLiteral>]
type AzureTableClientOptions () =
    inherit TableClientConfiguration()

    member val AzureStorageConnectionString : string = null with get, set

    new (azureStorageConnectionString : string) as this = AzureTableClientOptions() then
        this.AzureStorageConnectionString <- azureStorageConnectionString