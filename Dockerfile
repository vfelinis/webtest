#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WebTest/WebTest.csproj", "WebTest/"]
RUN dotnet restore "WebTest/WebTest.csproj"
COPY . .
WORKDIR "/src/WebTest"
RUN dotnet build "WebTest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebTest.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebTest.dll"]