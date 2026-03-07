# 1. Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# ფაილების კოპირება (დარწმუნდი რომ გზა სწორია შენს რეპოზიტორიაში)
COPY ["Portfolio.Asp/Portfolio.Asp/Portfolio.Asp.csproj", "Portfolio.Asp/Portfolio.Asp/"]

# გადავდივართ პროექტის საქაღალდეში
WORKDIR "/src/Portfolio.Asp/Portfolio.Asp"

# პაკეტების დამატება კონკრეტული ვერსიებით .NET 8-ისთვის
RUN dotnet add package AWSSDK.S3 --version 3.7.307
RUN dotnet add package AWSSDK.Extensions.NETCore.Setup --version 3.7.300
RUN dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 8.0.0

# პაკეტების აღდგენა
RUN dotnet restore "Portfolio.Asp.csproj"

# უკან დაბრუნება root-ში კოდის დასაკოპირებლად
WORKDIR /src
COPY . .

# ფინალური Build
WORKDIR "/src/Portfolio.Asp/Portfolio.Asp"
RUN dotnet publish "Portfolio.Asp.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 2. Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Railway პორტის კონფიგურაცია
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Portfolio.Asp.dll"]