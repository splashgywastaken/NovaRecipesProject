using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using FluentValidation;
using NovaRecipesProject.Common.Models.BaseModels;
using NovaRecipesProject.Context.Entities;
using NovaRecipesProject.Services.Categories.Models;

namespace NovaRecipesProject.Services.Recipes.Models.RecipeModels;

/// <summary>
/// DTO for updating data in DB
/// </summary>
public class UpdateRecipeModel : BaseNameDescriptionModel
{
    /// <summary>
    /// Recipe's categories
    /// </summary>
    // ReSharper disable once CollectionNeverUpdated.Global
    public ICollection<UpdateRecipeCategoryModel>? CategoryIds { get; set; }
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
        CreateMap<UpdateRecipeModel, Recipe>()
            .ForMember(dest => dest.Categories,
                opt => opt.MapFrom(p => p.CategoryIds)
            );
    }
}