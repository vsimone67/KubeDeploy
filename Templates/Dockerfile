FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["SERVICENAME.csproj", ""]
RUN dotnet restore "./SERVICENAME.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "SERVICENAME.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SERVICENAME.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SERVICENAME.dll"]