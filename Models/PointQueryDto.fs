namespace EscortBookVault.Models

open System.ComponentModel.DataAnnotations
open Newtonsoft.Json

type PointQueryDto () =
    
    [<Required>]
    [<JsonProperty("partitionKey")>]
    member val PartitionKey : string = null with get, set

    [<Required>]
    [<JsonProperty("rowKey")>]
    member val RowKey : string = null with get, set

type RowKeyDto () =
    
    [<Required>]
    [<JsonProperty("rowKey")>]
    member val RowKey : string = null with get, set