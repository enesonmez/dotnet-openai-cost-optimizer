FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["DotNetOpenAICostOptimizer.Api/DotNetOpenAICostOptimizer.Api.csproj", "DotNetOpenAICostOptimizer.Api/"]
RUN dotnet restore "DotNetOpenAICostOptimizer.Api/DotNetOpenAICostOptimizer.Api.csproj"
COPY . .
WORKDIR "/src/DotNetOpenAICostOptimizer.Api"
RUN dotnet build "./DotNetOpenAICostOptimizer.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DotNetOpenAICostOptimizer.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DotNetOpenAICostOptimizer.Api.dll"]
