using AutoMapper;
using FluentValidation;
using NovaRecipesProject.Common.Models.BaseModels;
using NovaRecipesProject.Context.Entities;

namespace NovaRecipesProject.Services.Recipes.Models;

/// <summary>
/// DTO for updating data in DB
/// </summary>
public class UpdateRecipeModel : BaseNameDescriptionModel
{
}

/// <inheritdoc />
public class UpdateRecipeModelValidator : BaseNameDescriptionModelValidator<UpdateRecipeModel>
{
    /// <summary>
    /// Constructor 
    /// </summary>
    public UpdateRecipeModelValidator()
    {
    }
}

/// <inheritdoc />
public class UpdateRecipeProfile : Profile
{
    /// <summary>
    /// Constructor which describes mapping for this DTO
    /// </summary>
    public UpdateRecipeProfile()
    {
        CreateMap<UpdateRecipeModel, Recipe>();
    }
}