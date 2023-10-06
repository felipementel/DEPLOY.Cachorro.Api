# DEPLOY.Cachorro.Api

![Imagem projeto api de cachorro](./docs/imgreadme1.png)

[![Quality gate](https://sonarcloud.io/api/project_badges/quality_gate?project=felipementel_DEPLOY.Cachorro.Api)](https://sonarcloud.io/summary/new_code?id=felipementel_DEPLOY.Cachorro.Api)

Projeto educacional, criado e mantido através do canal DEPLOY no YouTube.

> Para criar a imagem, a partir do diretório root da aplicação (pasta que contem o arquivo sln)

```
docker build -f ./DEPLOY.Cachorro.Api/Dockerfile  -t crcanaldeploydev.azurecr.io/cachorro.api:0.4 .
```

> Para executar o projeto local, utilizando docker

```
docker container run --rm -p 8088:80 -e ConnectionsString__ApplicationInsights="xxxx" -e ApplicationInsights__ApiKey="yyy" felipementel/cachorro-api:0.3
```

# Testes de unidade

Tecnologia: XUnit

0. Pre requisito
   Será necessário instalar os dois pacotes abaixo para ter sucesso ao executar os comandos descritos nesse arquivo.

```
dotnet tool install --global dotnet-reportgenerator-globaltool
```

```
dotnet tool install --global dotnet-reportgenerator-globaltool
```

1. Como Executar:
   1.1 A partir da pasta src execute o comando:

```
dotnet test
```

2. Geração de relatório de testes
   1.1 A partir da pasta src execute o comando:

```
dotnet test --collect:"XPlat Code Coverage" --logger "console;verbosity=detailed"
```

1.2 A partir da pasta src execute o comando:

```
reportgenerator -reports:C:/Proj/DEPLOY/DEPLOY.Cachorro/src/DEPLOY.Cachorro.Api.Tests/TestResults/**/*cobertura.xml -targetdir:C:/Proj/DEPLOY/DEPLOY.Cachorro/src/DEPLOY.Cachorro.Api.Tests/coveragereport -reporttypes:"Html;SonarQube;JsonSummary;Badges" -verbosity:Verbose -title:Cachorro.API -tag:canal-deploy
```

---

<br/>
<br/>
<br/>
<br/>
<br/>

# Link de documentações citadas durante a criaçao do projeto

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
