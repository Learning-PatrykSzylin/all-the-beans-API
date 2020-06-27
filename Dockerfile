FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV ALL-THE-BEANS-CONNSTRING="Host=db-postgressql-vt-do-user-7319987-0.a.db.ondigitalocean.com;Username=doadmin;Password=twclnuo44sljjkn3;Database=AllTheBeans;Port=25060;SSL Mode=Require;Trust Server Certificate=true"

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY . .
RUN dotnet restore "AllTheBeans.API/AllTheBeans.API.csproj"
WORKDIR "/src/AllTheBeans.API"
RUN dotnet build "AllTheBeans.API.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR "/src/AllTheBeans.API"
RUN dotnet publish "AllTheBeans.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AllTheBeans.API.dll"]