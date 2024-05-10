#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Knights.Challenge.Api/Knights.Challenge.Api.csproj", "Knights.Challenge.Api/"]
RUN dotnet restore "Knights.Challenge.Api/Knights.Challenge.Api.csproj"
COPY . .
WORKDIR "/src/Knights.Challenge.Api"
RUN dotnet build "Knights.Challenge.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Knights.Challenge.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Knights.Challenge.Api.dll"]
