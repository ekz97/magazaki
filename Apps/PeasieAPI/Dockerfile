FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PeasieAPI/PeasieAPI.csproj", "PeasieAPI/"]
RUN dotnet restore "PeasieAPI/PeasieAPI.csproj"
COPY . .
WORKDIR "/src/PeasieAPI"
RUN dotnet build "PeasieAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PeasieAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PeasieAPI.dll"]