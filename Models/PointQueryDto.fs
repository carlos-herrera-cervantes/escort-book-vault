namespace EscortBookVault.Models

open System.ComponentModel.DataAnnotations

type PointQueryDto () =
    
    [<Required>]
    member val PartitionKey : string = null with get, set

    [<Required>]
    member val RowKey : string = null with get, set

type RowKeyDto () =
    
    [<Required>]
    member val RowKey : string = null with get, set