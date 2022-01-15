namespace EscortBookVault.Models

open AutoMapper
open Microsoft.Azure.Cosmos.Table

type AutoMapping () as this =
    inherit Profile()

    do
        this.CreateMap<Secret, SingleSecretDto>() |> ignore
        this.CreateMap<TableResult, SingleSecretDto>() |> ignore
        this.CreateMap<TableResult, Secret>() |> ignore