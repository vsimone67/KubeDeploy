FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["SERVICENAME.csproj", ""]
RUN dotnet restore "./SERVICENAME.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "SERVICENAME.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SERVICENAME.csproj" -c Kubernetes -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SERVICENAME.dll"]