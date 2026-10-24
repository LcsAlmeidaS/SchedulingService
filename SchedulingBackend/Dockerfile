FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Scheduling.slnx ./
COPY Scheduling.API/Scheduling.API.csproj Scheduling.API/
COPY Scheduling.Application/Scheduling.Application.csproj Scheduling.Application/
COPY Scheduling.Domain/Scheduling.Domain.csproj Scheduling.Domain/
COPY Scheduling.Infrastructure/Scheduling.Infrastructure.csproj Scheduling.Infrastructure/
RUN dotnet restore Scheduling.API/Scheduling.API.csproj

COPY . .
RUN dotnet publish Scheduling.API/Scheduling.API.csproj \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

COPY --from=build /app/publish .

USER $APP_UID

ENTRYPOINT ["dotnet", "Scheduling.API.dll"]
