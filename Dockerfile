# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY Api/*.csproj Api/
COPY Model/*.csproj Model/
RUN dotnet restore Api/Api.csproj
RUN dotnet restore Model/Model.csproj

# copy and build app and libraries
COPY Api/ Api/
COPY Model/ Model/
WORKDIR /source/Api
RUN dotnet build -c Release

FROM build AS publish
RUN dotnet publish -c Release --no-build  -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 as runtime
WORKDIR /app
COPY --from=publish /app .
ENV ASPNETCORE_URLS=http://localhost:5000
EXPOSE 5000
ENTRYPOINT ["dotnet", "Api.dll"]