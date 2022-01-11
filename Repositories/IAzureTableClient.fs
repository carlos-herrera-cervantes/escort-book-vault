namespace EscortBookVault.Repositories

open System.Threading.Tasks
open Microsoft.Azure.Cosmos.Table

[<AllowNullLiteral>]
type IAzureTableClient =
    abstract member CreateIfNotExists : string -> Async<CloudTable>