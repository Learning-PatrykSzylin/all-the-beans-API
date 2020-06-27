FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["AllTheBeans.API/AllTheBeans.API.csproj", "AllTheBeans.API/"]
RUN dotnet restore "AllTheBeans.API/AllTheBeans.API.csproj"
COPY . .
WORKDIR "/src/AllTheBeans.API"
RUN dotnet build "AllTheBeans.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AllTheBeans.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AllTheBeans.API.dll"]