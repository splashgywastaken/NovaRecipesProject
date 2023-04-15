#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Systems/Workers/NovaRecipesProject.RecipeInfoSenderWorker/NovaRecipesProject.RecipeInfoSenderWorker.csproj", "Systems/Workers/NovaRecipesProject.RecipeInfoSenderWorker/"]
COPY ["Data/NovaRecipesProject.Context/NovaRecipesProject.Context.csproj", "Data/NovaRecipesProject.Context/"]
COPY ["Shared/NovaRecipesProject.Settings/NovaRecipesProject.Settings.csproj", "Shared/NovaRecipesProject.Settings/"]
COPY ["Data/NovaRecipesProject.Context.Entities/NovaRecipesProject.Context.Entities.csproj", "Data/NovaRecipesProject.Context.Entities/"]
COPY ["Services/NovaRecipesProject.Services.Actions/NovaRecipesProject.Services.Actions.csproj", "Services/NovaRecipesProject.Services.Actions/"]
COPY ["Services/NovaRecipesProject.Services.EmailSender/NovaRecipesProject.Services.EmailSender.csproj", "Services/NovaRecipesProject.Services.EmailSender/"]
COPY ["Shared/NovaRecipesProject.Consts/NovaRecipesProject.Consts.csproj", "Shared/NovaRecipesProject.Consts/"]
COPY ["Services/NovaRecipesProject.Services.Settings/NovaRecipesProject.Services.Settings.csproj", "Services/NovaRecipesProject.Services.Settings/"]
COPY ["Services/NovaRecipesProject.Services.RabbitMq/NovaRecipesProject.Services.RabbitMq.csproj", "Services/NovaRecipesProject.Services.RabbitMq/"]
COPY ["Shared/NovaRecipesProject.Common/NovaRecipesProject.Common.csproj", "Shared/NovaRecipesProject.Common/"]
RUN dotnet restore "Systems/Workers/NovaRecipesProject.RecipeInfoSenderWorker/NovaRecipesProject.RecipeInfoSenderWorker.csproj"
COPY . .
WORKDIR "/src/Systems/Workers/NovaRecipesProject.RecipeInfoSenderWorker"
RUN dotnet build "NovaRecipesProject.RecipeInfoSenderWorker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NovaRecipesProject.RecipeInfoSenderWorker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NovaRecipesProject.RecipeInfoSenderWorker.dll"]