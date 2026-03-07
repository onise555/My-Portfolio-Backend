# 1. Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# ფაილების კოპირება
COPY ["Portfolio.Asp/Portfolio.Asp/Portfolio.Asp.csproj", "Portfolio.Asp/Portfolio.Asp/"]

# გადავდივართ პროექტის საქაღალდეში პაკეტების დასამატებლად
WORKDIR "/src/Portfolio.Asp/Portfolio.Asp"

# პაკეტების იძულებითი დამატება (თუ .csproj-ში არ ჩანს)
RUN dotnet add package AWSSDK.S3
RUN dotnet add package AWSSDK.Extensions.NETCore.Setup
RUN dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL

# პაკეტების აღდგენა
RUN dotnet restore

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

# Railway-სთვის პორტის მართვა
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Portfolio.Asp.dll"]