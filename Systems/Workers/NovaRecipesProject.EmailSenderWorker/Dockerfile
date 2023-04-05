#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Systems/Workers/NovaRecipesProject.EmailSenderWorker/NovaRecipesProject.EmailSenderWorker.csproj", "Systems/Workers/NovaRecipesProject.EmailSenderWorker/"]
COPY ["Data/NovaRecipesProject.Context/NovaRecipesProject.Context.csproj", "Data/NovaRecipesProject.Context/"]
COPY ["Shared/NovaRecipesProject.Settings/NovaRecipesProject.Settings.csproj", "Shared/NovaRecipesProject.Settings/"]
COPY ["Data/NovaRecipesProject.Context.Entities/NovaRecipesProject.Context.Entities.csproj", "Data/NovaRecipesProject.Context.Entities/"]
COPY ["Services/NovaRecipesProject.Services.Actions/NovaRecipesProject.Services.Actions.csproj", "Services/NovaRecipesProject.Services.Actions/"]
COPY ["Services/NovaRecipesProject.Services.EmailSender/NovaRecipesProject.Services.EmailSender.csproj", "Services/NovaRecipesProject.Services.EmailSender/"]
COPY ["Shared/NovaRecipesProject.Consts/NovaRecipesProject.Consts.csproj", "Shared/NovaRecipesProject.Consts/"]
COPY ["Services/NovaRecipesProject.Services.RabbitMq/NovaRecipesProject.Services.RabbitMq.csproj", "Services/NovaRecipesProject.Services.RabbitMq/"]
COPY ["Shared/NovaRecipesProject.Common/NovaRecipesProject.Common.csproj", "Shared/NovaRecipesProject.Common/"]
RUN dotnet restore "Systems/Workers/NovaRecipesProject.EmailSenderWorker/NovaRecipesProject.EmailSenderWorker.csproj"
COPY . .
WORKDIR "/src/Systems/Workers/NovaRecipesProject.EmailSenderWorker"
RUN dotnet build "NovaRecipesProject.EmailSenderWorker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NovaRecipesProject.EmailSenderWorker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NovaRecipesProject.EmailSenderWorker.dll"]