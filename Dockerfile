FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["GarmentProductionTracking.WebUI/GarmentProductionTracking.WebUI.csproj", "GarmentProductionTracking.WebUI/"]
COPY ["GarmentProductionTracking.Application/GarmentProductionTracking.Application.csproj", "GarmentProductionTracking.Application/"]
COPY ["GarmentProductionTracking.Domain/GarmentProductionTracking.Domain.csproj", "GarmentProductionTracking.Domain/"]
COPY ["GarmentProductionTracking.Infrastructure/GarmentProductionTracking.Infrastructure.csproj", "GarmentProductionTracking.Infrastructure/"]

RUN dotnet restore "GarmentProductionTracking.WebUI/GarmentProductionTracking.WebUI.csproj"

COPY . .

WORKDIR "/src/GarmentProductionTracking.WebUI"

RUN dotnet publish "GarmentProductionTracking.WebUI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "GarmentProductionTracking.WebUI.dll"]