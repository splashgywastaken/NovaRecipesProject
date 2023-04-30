using NovaRecipesProject.Common.Enums;
using NovaRecipesProject.Services.Recipes.Models.RecipeCommentModels;
using NovaRecipesProject.Services.Recipes.Models.RecipeIngredientModels;
using NovaRecipesProject.Services.Recipes.Models.RecipeModels;

namespace NovaRecipesProject.Services.Recipes;

/// <summary>
/// Recipe's service with main methods to implement them further
/// </summary>
public interface IRecipeService
{
    /// <summary>
    /// Method to get basic recipe list
    /// </summary>
    /// <param name="offset">Sets offset for data it got</param>
    /// <param name="limit">Sets limit for number of data to return</param>
    /// <returns>Returns list of RecipeModel</returns>
    Task<IEnumerable<RecipeModel>> GetRecipes(int offset, int limit);
    /// <summary>
    /// Method used to search for recipes using some filters.
    /// Caching is not used for this method, because of data filters changes 
    /// </summary>
    /// <param name="nameSearchString">Search string which used to search through recipes and their names</param>
    /// <param name="searchType">Type of search
    /// (describes should it include search string, be a full search string e.t.c.). By default its partial search</param>
    /// <param name="sortType">Type of sort</param>
    /// <param name="categoriesList">List of categories to search to</param>
    /// <param name="offset">Offset for data</param>
    /// <param name="limit">Limit for data</param>
    /// <returns></returns>
    Task<IEnumerable<RecipeModel>> GetRecipesFiltered(
        string? nameSearchString,
        SearchType searchType,
        SortType? sortType,
        List<string>? categoriesList,
        int offset,
        int limit
        );
    /// <summary>
    /// Method to get basic recipe list.
    /// Caches data using Id of some user
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="offset">Sets offset for data it got</param>
    /// <param name="limit">Sets limit for number of data to return</param>
    /// <returns>Returns list of RecipeModel</returns>
    Task<IEnumerable<RecipeModel>> GetRecipesAndCacheForUser(string userId, int offset, int limit);
    /// <summary>
    /// Method to get list of recipes of exact user
    /// </summary>
    /// <param name="userId">User's id</param>
    /// <param name="offset">Sets offset for data it got</param>
    /// <param name="limit">Sets limit for number of data to return</param>
    /// <returns></returns>
    Task<IEnumerable<RecipeModel>> GetUserRecipes(string userId, int offset, int limit);
    /// <summary>
    /// Method used to get comments of certain recipe
    /// </summary>
    /// <param name="recipeId">recipe's Id</param>
    /// <param name="offset">offset for data</param>
    /// <param name="limit">limit for data</param>
    /// <returns>List of comments for certain recipe ordered by CreatedDateTime value</returns>
    Task<IEnumerable<RecipeCommentLightModel>> GetRecipeComments(int recipeId, int offset, int limit);

    /// <summary>
    /// Method used to get comments of certain recipe and cache data using user's data
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="recipeId">recipe's Id</param>
    /// <param name="offset">offset for data</param>
    /// <param name="limit">limit for data</param>
    /// <returns>List of comments for certain recipe ordered by CreatedDateTime value</returns>
    Task<IEnumerable<RecipeCommentLightModel>> GetRecipeCommentsAndCacheForUser(
        string userId, 
        int recipeId,
        int offset, 
        int limit
        );
    /// <summary>
    /// Method to get list of recipe's ingredients.
    /// </summary>
    /// <param name="recipeId">recipe id to get list of ingredients from</param>
    /// <returns>Returns <c>IEnumerable</c> of <c>RecipeIngredientModel</c> which
    /// describes main information about recipe's ingredients</returns>
    Task<IEnumerable<RecipeIngredientModel>> GetRecipesIngredients(int recipeId);

    /// <summary>
    /// Method to get list of recipe's ingredients. And cache using user's data
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="recipeId">recipe id to get list of ingredients from</param>
    /// <returns>Returns <c>IEnumerable</c> of <c>RecipeIngredientModel</c> which
    /// describes main information about recipe's ingredients</returns>
    Task<IEnumerable<RecipeIngredientModel>> GetRecipesIngredientsAndCacheForUser(string userId, int recipeId);
    /// <summary>
    /// Method to get recipe by some Id
    /// </summary>
    /// <param name="id">Id of a recipe</param>
    /// <returns>Exact recipe with given Id and most of its data, or error related to what occured in proccess</returns>
    Task<WholeRecipeModel> GetRecipeById(int id);
    /// <summary>
    /// Uses argument ot add new recipe to a DB,
    /// also uses data about current logged in user to add new recipe to all of his others
    /// </summary>
    /// <param name="userId">User Id to add recipe to</param>
    /// <param name="model">Basic data about recipe</param>
    /// <returns></returns>
    Task<RecipeModel> AddRecipeWithUser(string userId, AddRecipeModel model);
    /// <summary>
    /// Creates new entry for join table, basically adds ingredient to a recipe
    /// </summary>
    /// <param name="model"></param>
    /// <returns>Model with </returns>
    Task<RecipeIngredientModel> AddIngredientToRecipe(AddRecipeIngredientModel model);
    /// <summary>
    /// Creates new entry for recipeComments table for certain recipe
    /// </summary>
    /// <param name="recipeId">Id of recipe to add comment to</param>
    /// <param name="model">data of comment</param>
    /// <returns></returns>
    Task<RecipeCommentLightModel> AddCommentToRecipe(int recipeId, AddRecipeCommentModel model);
    /// <summary>
    /// Method used to update data in DB
    /// </summary>
    /// <param name="id">Id of exact DB entry to update</param>
    /// <param name="model">Model, which data will be used to update an entry in DB</param>
    /// <returns></returns>
    Task UpdateRecipe(int id, UpdateRecipeModel model);
    /// <summary>
    /// Method used to update entry for RecipeInggredient in DB,
    /// could update any property of RecipeIngredient but RecipeId and IngredientId
    /// </summary>
    /// <param name="recipeId">recipe's id</param>
    /// <param name="ingredientId">ingredient's id</param>
    /// <param name="model">Model which data is used to update entry in DB</param>
    /// <returns></returns>
    Task UpdateRecipeIngredient(int recipeId, int ingredientId, UpdateRecipeIngredientModel model);

    /// <summary>
    /// Method used to update data in comment for certain recipe
    /// </summary>
    /// <param name="commentId"></param>
    /// <param name="recipeId"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    Task UpdateRecipeComment(int commentId, int recipeId, UpdateRecipeCommentModel model);
    /// <summary>
    /// Method used to delete entry from DB
    /// </summary>
    /// <param name="id">Id of an entry to delete</param>
    /// <returns></returns>
    Task DeleteRecipe(int id);
    /// <summary>
    /// Deletes entry in RecipeIngredient table in DB
    /// </summary>
    /// <param name="recipeId">Recipe id to search through recipes with</param>
    /// <param name="ingredientId">Ingredient id to search through recipes with</param>
    /// <returns></returns>
    Task DeleteRecipeIngredient(int recipeId, int ingredientId);

    /// <summary>
    /// Deletes comment for certain recipe
    /// </summary>
    /// <param name="commentId"></param>
    /// <param name="recipeId"></param>
    /// <returns></returns>
    Task DeleteRecipeComment(int commentId, int recipeId);
}