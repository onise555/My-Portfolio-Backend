# 1. Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# ფაილების კოპირება ხდება Root-იდან. 
# სურათის მიხედვით გზა არის: Portfolio.Asp (მშობელი) -> Portfolio.Asp (შვილი)
COPY ["Portfolio.Asp/Portfolio.Asp/Portfolio.Asp.csproj", "Portfolio.Asp/Portfolio.Asp/"]
RUN dotnet restore "Portfolio.Asp/Portfolio.Asp/Portfolio.Asp.csproj"

# მთლიანი კოდის კოპირება
COPY . .

# გადავდივართ უშუალოდ პროექტის საქაღალდეში build-ისთვის
WORKDIR "/src/Portfolio.Asp/Portfolio.Asp"
RUN dotnet publish "Portfolio.Asp.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 2. Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Portfolio.Asp.dll"]