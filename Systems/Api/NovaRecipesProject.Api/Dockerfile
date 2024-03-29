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
COPY ["Services/NovaRecipesProject.Services.Actions/NovaRecipesProject.Services.Actions.csproj", "Services/NovaRecipesProject.Services.Actions/"]
COPY ["Services/NovaRecipesProject.Services.EmailSender/NovaRecipesProject.Services.EmailSender.csproj", "Services/NovaRecipesProject.Services.EmailSender/"]
COPY ["Shared/NovaRecipesProject.Consts/NovaRecipesProject.Consts.csproj", "Shared/NovaRecipesProject.Consts/"]
COPY ["Services/NovaRecipesProject.Services.Settings/NovaRecipesProject.Services.Settings.csproj", "Services/NovaRecipesProject.Services.Settings/"]
COPY ["Services/NovaRecipesProject.Services.RabbitMq/NovaRecipesProject.Services.RabbitMq.csproj", "Services/NovaRecipesProject.Services.RabbitMq/"]
COPY ["Services/NovaRecipesProject.Services.Cache/NovaRecipesProject.Services.Cache.csproj", "Services/NovaRecipesProject.Services.Cache/"]
COPY ["Shared/NovaRecipesProject.Common/NovaRecipesProject.Common.csproj", "Shared/NovaRecipesProject.Common/"]
COPY ["Services/NovaRecipesProject.Services.Categories/NovaRecipesProject.Services.Categories.csproj", "Services/NovaRecipesProject.Services.Categories/"]
COPY ["Services/NovaRecipesProject.Services.Ingredients/NovaRecipesProject.Services.Ingredients.csproj", "Services/NovaRecipesProject.Services.Ingredients/"]
COPY ["Services/NovaRecipesProject.Services.RecipeCommentsSubscriptions/NovaRecipesProject.Services.RecipeCommentsSubscriptions.csproj", "Services/NovaRecipesProject.Services.RecipeCommentsSubscriptions/"]
COPY ["Services/NovaRecipesProject.Services.RecipeParagraphs/NovaRecipesProject.Services.RecipeParagraphs.csproj", "Services/NovaRecipesProject.Services.RecipeParagraphs/"]
COPY ["Services/NovaRecipesProject.Services.RecipesSubscriptions/NovaRecipesProject.Services.RecipesSubscriptions.csproj", "Services/NovaRecipesProject.Services.RecipesSubscriptions/"]
COPY ["Services/NovaRecipesProject.Services.Recipes/NovaRecipesProject.Services.Recipes.csproj", "Services/NovaRecipesProject.Services.Recipes/"]
COPY ["Services/NovaRecipesProject.Services.UserAccount/NovaRecipesProject.Services.UserAccount.csproj", "Services/NovaRecipesProject.Services.UserAccount/"]
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