using NovaRecipesProject.Services.Recipes.Models;

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
    Task<IEnumerable<RecipeModel>> GetRecipes(int offset = 0, int limit = 10);
    /// <summary>
    /// Method to get recipe by some Id
    /// </summary>
    /// <param name="id">Id of a recipe</param>
    /// <returns>Exact recipe with given Id, or error related to what occured in proccess</returns>
    Task<RecipeModel> GetRecipeById(int id);
    /// <summary>
    /// Uses argument to add new recipe to a DB
    /// </summary>
    /// <param name="model">Model which is used to add new data to DB</param>
    /// <returns>Returns added recipe data</returns>
    Task<RecipeModel> AddRecipe(AddRecipeModel model);
    /// <summary>
    /// Method used to update data in DB
    /// </summary>
    /// <param name="id">Id of exact DB entry to update</param>
    /// <param name="model">Model, which data will be used to update an entry in DB</param>
    /// <returns></returns>
    Task UpdateRecipe(int id, UpdateRecipeModel model);
    /// <summary>
    /// Method used to delete entry from DB
    /// </summary>
    /// <param name="id">Id of an entry to delete</param>
    /// <returns></returns>
    Task DeleteRecipe(int id);
}