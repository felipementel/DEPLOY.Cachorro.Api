# DEPLOY.Cachorro.Api

> Para criar nossa imagem, a partir do diretório root da aplicação (pasta que contem o arquivo sln)

```
docker build -f ./DEPLOY.Cachorro.Api/Dockerfile  -t crcanaldeploydev.azurecr.io/cachorro-api:0.4 .
```

> Para executar o projeto local, utilizando docker

```
docker container run --rm -p 8088:80 -e ConnectionsString__ApplicationInsights="xxxx" -e ApplicationInsights__ApiKey="yyy" felipementel/cachorro-api:0.3
```

![Imagem projeto api de cachorro](./docs/imgreadme1.png)

```
https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-7.0
```

```
https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-7.0&tabs=windows
```

ILogger

```
https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-7.0
```

```
https://learn.microsoft.com/en-us/azure/azure-monitor/app/ilogger?tabs=dotnet6
```

Application Insights

```
https://learn.microsoft.com/pt-br/azure/azure-monitor/app/asp-net-core?tabs=netcorenew%2Cnetcore6
```

Live Stream / Live Metrics

```
https://learn.microsoft.com/en-us/azure/azure-monitor/app/live-stream?tabs=dotnet6
```

Configurations

```
https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-7.0
```
