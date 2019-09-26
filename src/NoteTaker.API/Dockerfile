FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/NoteTaker.API/NoteTaker.API.csproj", "src/NoteTaker.API/"]
COPY ["src/NoteTaker.Core/NoteTaker.Core.csproj", "src/NoteTaker.Core/"]
COPY ["src/NoteTaker.DAL/NoteTaker.DAL.csproj", "src/NoteTaker.DAL/"]
RUN dotnet restore "src/NoteTaker.API/NoteTaker.API.csproj"
COPY . .
WORKDIR "/src/src/NoteTaker.API"
RUN dotnet build "NoteTaker.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NoteTaker.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NoteTaker.API.dll"]