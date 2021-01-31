#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["ApplicationInsight.csproj", ""]
RUN dotnet restore "./ApplicationInsight.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "ApplicationInsight.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApplicationInsight.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApplicationInsight.dll"]