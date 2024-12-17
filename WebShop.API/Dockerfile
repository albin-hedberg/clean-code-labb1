FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["WebShop.API/WebShop.API.csproj", "WebShop.API/"]
COPY ["WebShop.DataAccess/WebShop.DataAccess.csproj", "WebShop.DataAccess/"]
COPY ["WebShop.Tests/WebShop.Tests.csproj", "WebShop.Tests/"]

RUN dotnet restore "WebShop.API/WebShop.API.csproj"

COPY . .

RUN dotnet build "WebShop.API/WebShop.API.csproj" -c Release
RUN dotnet test "WebShop.Tests/WebShop.Tests.csproj" --no-build --logger:trx --results-directory /src/tests
RUN dotnet publish "WebShop.API/WebShop.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "WebShop.API.dll"]
