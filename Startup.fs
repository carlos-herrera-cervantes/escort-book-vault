namespace EscortBookVault

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Configuration
open EscortBookVault.Extensions.AzureTableExtensions
open EscortBookVault.Extensions.AutoMapperExtensions

type Startup private () =

    member val Configuration : IConfiguration = null with get, set

    new (configuration: IConfiguration) as this = Startup() then
        this.Configuration <- configuration

    member this.ConfigureServices(services: IServiceCollection) =
        services.AddAutoMapperConfiguration(this.Configuration) |> ignore
        services.AddControllers() |> ignore
        services.AddAzureTableStorage(fun options ->
            options.AzureStorageConnectionString <- this.Configuration.GetSection("ConnectionStrings").GetSection("TableStorge").Value
        ) |> ignore

    member this.Configure(app: IApplicationBuilder, env: IWebHostEnvironment) =
        if env.IsDevelopment() then
            app.UseDeveloperExceptionPage() |> ignore

        app.UseRouting() |> ignore
        app.UseEndpoints(fun endpoints -> endpoints.MapControllers() |> ignore) |> ignore
