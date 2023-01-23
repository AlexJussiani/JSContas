#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["./Services/jsContas/JS.Contas.API/JS.Contas.API.csproj", "Services/jsContas/JS.Contas.API/"]
COPY ["./Services/jsContas/building blocks/JS.EventSourcing/JS.EventSourcing.csproj", "Services/jsContas/building blocks/JS.EventSourcing/"]
COPY ["./Services/jsContas/building blocks/JS.Core/JS.Core.csproj", "Services/jsContas/building blocks/JS.Core/"]
COPY ["./Services/jsContas/building blocks/JS.MessageBus/JS.MessageBus.csproj", "Services/jsContas/building blocks/JS.MessageBus/"]
COPY ["./Services/jsContas/building blocks/JS.WebAPI.Core/JS.WebAPI.Core.csproj", "Services/jsContas/building blocks/JS.WebAPI.Core/"]
COPY ["./Services/jsContas/JS.Contas.Infra/JS.Contas.Infra.csproj", "Services/jsContas/JS.Contas.Infra/"]
COPY ["./Services/jsContas/JS.Contas.Domain/JS.Contas.Domain.csproj", "Services/jsContas/JS.Contas.Domain/"]
RUN dotnet restore "Services/jsContas/JS.Contas.API/JS.Contas.API.csproj"
COPY . .
WORKDIR "./Services/jsContas/JS.Contas.API"
RUN dotnet build "JS.Contas.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JS.Contas.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JS.Contas.API.dll"]