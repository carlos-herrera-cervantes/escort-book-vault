namespace EscortBookVault.Extensions

open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open AutoMapper
open EscortBookVault.Models

module AutoMapperExtensions =

    type IServiceCollection with

        member this.AddAutoMapperConfiguration (configuration: IConfiguration) =
            let mapperConfig = MapperConfiguration(fun mc -> mc.AddProfile(AutoMapping()))
            let mapper = mapperConfig.CreateMapper()

            this.AddSingleton(mapper) |> ignore
            this