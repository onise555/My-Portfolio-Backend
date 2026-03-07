# 1. Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# ფაილების კოპირება რეპოზიტორიის ძირიდან (Root)
COPY ["Portfolio.Asp/Portfolio.Asp/Portfolio.Asp.csproj", "Portfolio.Asp/Portfolio.Asp/"]
RUN dotnet restore "Portfolio.Asp/Portfolio.Asp/Portfolio.Asp.csproj"

# მთლიანი კოდის კოპირება
COPY . .

# სამუშაო საქაღალდის შეცვლა უშუალოდ პროექტის ფაილთან
WORKDIR "/src/Portfolio.Asp/Portfolio.Asp"
RUN dotnet publish "Portfolio.Asp.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 2. Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Railway-სთვის პორტის დინამიური მითითება
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Portfolio.Asp.dll"]