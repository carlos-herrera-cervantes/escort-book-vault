namespace EscortBookVault.Models

open Newtonsoft.Json

type BaseResponse () =

    [<JsonProperty("status")>]
    member val Status: bool = true with get, set