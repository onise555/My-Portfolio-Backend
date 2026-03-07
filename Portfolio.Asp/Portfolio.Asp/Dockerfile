# 1. Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# ჯერ ვაკოპირებთ მთლიან რეპოზიტორიას
COPY . .

# გადავდივართ პროექტის საქაღალდეში
WORKDIR "/src/Portfolio.Asp/Portfolio.Asp"

# პაკეტების იძულებითი დამატება კონკრეტული ვერსიებით .NET 8-ისთვის
RUN dotnet add package AWSSDK.S3 --version 3.7.307
RUN dotnet add package AWSSDK.Extensions.NETCore.Setup --version 3.7.300
RUN dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 8.0.0

# აღდგენა და გამოქვეყნება
RUN dotnet publish "Portfolio.Asp.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 2. Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Portfolio.Asp.dll"]