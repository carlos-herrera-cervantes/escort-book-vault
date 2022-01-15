namespace EscortBookVault.Models

open System
open System.ComponentModel.DataAnnotations
open Microsoft.Azure.Cosmos.Table
open Newtonsoft.Json

[<AllowNullLiteral>]
type Secret () =
    inherit TableEntity()

    [<Required>]
    [<JsonProperty("secretName")>]
    member val SecretName : string = null with get, set

    [<Required>]
    [<JsonProperty("secretValue")>]
    member val SecretValue : string = null with get, set

    [<JsonProperty("createdAt")>]
    member val CreatedAt : DateTime = DateTime.UtcNow with get, set

    [<JsonProperty("updatedAt")>]
    member val UpdatedAt : DateTime = DateTime.UtcNow with get, set

[<AllowNullLiteral>]
type SingleSecretDto () =
    inherit SingleBase()

    [<JsonProperty("secretName")>]
    member val SecretName : string = null with get, set

    [<JsonProperty("secretValue")>]
    member val SecretValue : string = null with get, set

type UpdateSecretDto () =

    [<JsonProperty("secretValue")>]
    member val SecretValue : string = null with get, set

type SuccessSecretResponse () =
    inherit BaseResponse()

    member val Data : SingleSecretDto = null with get, set