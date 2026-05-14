# Використовуємо образ .NET 8 SDK для збірки (або 7, якщо використовуєте його)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копіюємо файл проекту та відновлюємо залежності
COPY ["CoffeeShopLoyalty.Api.csproj", "./"]
RUN dotnet restore "CoffeeShopLoyalty.Api.csproj"

# Копіюємо весь інший код
COPY . .
WORKDIR "/src/"

# Публікуємо реліз
RUN dotnet publish "CoffeeShopLoyalty.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Створюємо фінальний образ на основі легкого ASP.NET runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Render автоматично задає змінну середовища PORT, тому дозволимо ASP.NET слухати на цьому порту
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "CoffeeShopLoyalty.Api.dll"]