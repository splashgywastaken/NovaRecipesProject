#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Systems/Identity/NovaRecipesProject.Identity/NovaRecipesProject.Identity.csproj", "Systems/Identity/NovaRecipesProject.Identity/"]
COPY ["Data/NovaRecipesProject.Context.Entities/NovaRecipesProject.Context.Entities.csproj", "Data/NovaRecipesProject.Context.Entities/"]
COPY ["Data/NovaRecipesProject.Context/NovaRecipesProject.Context.csproj", "Data/NovaRecipesProject.Context/"]
COPY ["Shared/NovaRecipesProject.Settings/NovaRecipesProject.Settings.csproj", "Shared/NovaRecipesProject.Settings/"]
COPY ["Shared/NovaRecipesProject.Common/NovaRecipesProject.Common.csproj", "Shared/NovaRecipesProject.Common/"]
RUN dotnet restore "Systems/Identity/NovaRecipesProject.Identity/NovaRecipesProject.Identity.csproj"
COPY . .
WORKDIR "/src/Systems/Identity/NovaRecipesProject.Identity"
RUN dotnet build "NovaRecipesProject.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NovaRecipesProject.Identity.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NovaRecipesProject.Identity.dll"]