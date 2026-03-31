FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
RUN apt-get update && apt-get install -y libgssapi-krb5-2 && rm -rf /var/lib/apt/lists/*
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY Company.App.Web/Company.App.Web.csproj ./Company.App.Web/
RUN dotnet restore Company.App.Web/Company.App.Web.csproj
COPY . .
WORKDIR /src/Company.App.Web
RUN dotnet build Company.App.Web.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Company.App.Web.csproj -c Release -o /app

FROM base AS final
WORKDIR	/app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Company.App.Web.dll"]