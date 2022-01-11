namespace EscortBookVault.Models

open System
open Newtonsoft.Json

[<AllowNullLiteral>]
type SingleBase () =

    [<JsonProperty("partitionKey")>]
    member val PartitionKey : string = null with get, set

    [<JsonProperty("rowKey")>]
    member val RowKey : string = null with get, set

    [<JsonProperty("createdAt")>]
    member val CreatedAt : DateTime = DateTime.UtcNow with get, set

    [<JsonProperty("updatedAt")>]
    member val UpdatedAt : DateTime = DateTime.UtcNow with get, set