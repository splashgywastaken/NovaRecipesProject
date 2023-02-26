#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Systems/Api/NovaRecipesProject.Api/NovaRecipesProject.Api.csproj", "Systems/Api/NovaRecipesProject.Api/"]
COPY ["Data/Migrations/NovaRecipesProject.Context.Migrations.MSSQL/NovaRecipesProject.Context.Migrations.MSSQL.csproj", "Data/Migrations/NovaRecipesProject.Context.Migrations.MSSQL/"]
COPY ["Data/NovaRecipesProject.Context.Entities/NovaRecipesProject.Context.Entities.csproj", "Data/NovaRecipesProject.Context.Entities/"]
COPY ["Data/NovaRecipesProject.Context/NovaRecipesProject.Context.csproj", "Data/NovaRecipesProject.Context/"]
COPY ["Shared/NovaRecipesProject.Settings/NovaRecipesProject.Settings.csproj", "Shared/NovaRecipesProject.Settings/"]
COPY ["Data/Migrations/NovaRecipesProject.Context.Migrations.PostgreSQL/NovaRecipesProject.Context.Migrations.PostgreSQL.csproj", "Data/Migrations/NovaRecipesProject.Context.Migrations.PostgreSQL/"]
COPY ["Services/NovaRecipesProject.Services.Settings/NovaRecipesProject.Services.Settings.csproj", "Services/NovaRecipesProject.Services.Settings/"]
COPY ["Shared/NovaRecipesProject.Common/NovaRecipesProject.Common.csproj", "Shared/NovaRecipesProject.Common/"]
COPY ["Systems/Api/NovaRecipesProject.Api.Settings/NovaRecipesProject.Api.Settings.csproj", "Systems/Api/NovaRecipesProject.Api.Settings/"]
RUN dotnet restore "Systems/Api/NovaRecipesProject.Api/NovaRecipesProject.Api.csproj"
COPY . .
WORKDIR "/src/Systems/Api/NovaRecipesProject.Api"
RUN dotnet build "NovaRecipesProject.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NovaRecipesProject.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NovaRecipesProject.Api.dll"]