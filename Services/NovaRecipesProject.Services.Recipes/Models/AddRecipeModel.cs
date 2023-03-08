﻿using AutoMapper;
using FluentValidation;
using NovaRecipesProject.Common.Models;
using NovaRecipesProject.Context.Entities;

namespace NovaRecipesProject.Services.Recipes.Models;

/// <summary>
/// Model for adding new recipe 
/// </summary>
public class AddRecipeModel : BaseNameDescriptionModel
{

}

/// <inheritdoc />
public class AddRecipeModelProfile : Profile
{
    /// <inheritdoc />
    public AddRecipeModelProfile()
    {
        CreateMap<AddRecipeModel, Recipe>();
    }
}

/// <inheritdoc />
public class AddRecipeModelValidator : AbstractValidator<AddRecipeModel>
{
    /// <inheritdoc />
    public AddRecipeModelValidator()
    {
    }
}