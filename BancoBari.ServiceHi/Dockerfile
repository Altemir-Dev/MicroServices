#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["BancoBari.ServiceHi/BancoBari.ServiceHi.csproj", "BancoBari.ServiceHi/"]
COPY ["BancoBari.Contracts/BancoBari.Contracts.csproj", "BancoBari.Contracts/"]
COPY ["BancoBari.Infra/BancoBari.Infra.csproj", "BancoBari.Infra/"]
RUN dotnet restore "BancoBari.ServiceHi/BancoBari.ServiceHi.csproj"
COPY . .
WORKDIR "/src/BancoBari.ServiceHi"
RUN dotnet build "BancoBari.ServiceHi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BancoBari.ServiceHi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BancoBari.ServiceHi.dll"]