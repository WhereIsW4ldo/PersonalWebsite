﻿FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

COPY *.sln .
COPY ./Backend.Api/Backend.Api.csproj ./Backend.Api/
COPY ./Database/Database.csproj ./Database/
COPY ./Login/Login.csproj ./Login/
RUN dotnet restore

COPY . .
RUN dotnet build "Backend.Api/Backend.Api.csproj" -c Release -o /app/build -p:AssemblyVersion=$Version
RUN dotnet publish "Backend.Api/Backend.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENV DB_PASSWORD=$SQL_PASSWORD
EXPOSE 8080
EXPOSE 8081
ENTRYPOINT ["dotnet", "Backend.Api.dll"]
