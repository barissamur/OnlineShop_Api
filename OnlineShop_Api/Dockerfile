# SDK Image kullanarak build ortamýný oluþtur
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app
EXPOSE 2020
EXPOSE 2021

# csproj dosyasýný ve restore iþlemini çalýþtýr
COPY *.csproj ./
RUN dotnet restore

# Uygulama dosyalarýný kopyala
COPY . ./

# Uygulamayý build ve publish et
RUN dotnet publish -c Release -o out

# Runtime Image kullanarak final ortamýný oluþtur
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Uygulamayý çalýþtýr
# Uygulamanýzýn adý TestApi.dll ise bu kýsmý deðiþtirmeyin, eðer farklýysa doðru adý girin.
ENTRYPOINT ["dotnet", "TestApi.dll"]
