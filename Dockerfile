# Build aşaması
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Tüm kaynak kodları kopyala
COPY . .

# Restore işlemi
RUN dotnet restore "src/EnterpriseAPI.Api/EnterpriseAPI.Api.csproj"

# Release modunda derleme ve yayınlama
WORKDIR "/app/src/EnterpriseAPI.Api"

RUN dotnet publish "EnterpriseAPI.Api.csproj" -c Release -o /app/publish

# Runtime aşaması
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Sertifika için (HTTPS isteniyorsa)
ENV ASPNETCORE_URLS=http://+:80

# Build aşamasından yayınlanan dosyaları kopyala
COPY --from=build /app/publish .

# Uygulamayı başlat
ENTRYPOINT ["dotnet", "EnterpriseAPI.Api.dll"]