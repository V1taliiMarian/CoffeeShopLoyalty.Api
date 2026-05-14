FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Копіюємо файл проекту та відновлюємо залежності
COPY ["CoffeeShopLoyalty.Api.csproj", "./"]
RUN dotnet restore "CoffeeShopLoyalty.Api.csproj"

# Копіюємо весь інший код
COPY . .
WORKDIR "/src/"

# Публікуємо реліз
RUN dotnet publish "CoffeeShopLoyalty.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Render автоматично задає змінну середовища PORT, тому дозволимо ASP.NET слухати на цьому порту
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "CoffeeShopLoyalty.Api.dll"]