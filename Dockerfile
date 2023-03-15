FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Een.Api/Een.Api.csproj", "Een.Api/"]
COPY ["Een.Logic/Een.Logic.csproj", "Een.Logic/"]
COPY ["Een.Model/Een.Model.csproj", "Een.Model/"]
COPY ["Een.Socket/Een.Socket.csproj", "Een.Socket/"]
RUN dotnet restore "Een.Api/Een.Api.csproj"
COPY . .
WORKDIR "/src/Een.Api"
RUN dotnet build "Een.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Een.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Een.Api.dll"]
