FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ShortLink.WebAPI/ShortLink.WebAPI.csproj", "src/ShortLink.WebAPI/"]
COPY ["src/ShortLink.Core/ShortLink.Core.csproj", "src/ShortLink.Core/"]
COPY ["src/ShortLink.Infrastructure/ShortLink.Infrastructure.csproj", "src/ShortLink.Infrastructure/"]
RUN dotnet restore "src/ShortLink.WebAPI/ShortLink.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/ShortLink.WebAPI"
RUN dotnet build "ShortLink.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ShortLink.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShortLink.WebAPI.dll"]
