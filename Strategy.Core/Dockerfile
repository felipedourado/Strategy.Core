FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Strategy.Core/Strategy.Core.csproj", "Strategy.Core/"]
RUN dotnet restore "Strategy.Core/Strategy.Core.csproj"
COPY . .
WORKDIR "/src/Strategy.Core"
RUN dotnet build "Strategy.Core.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Strategy.Core.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Strategy.Core.dll"]
