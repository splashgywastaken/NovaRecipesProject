using NovaRecipesProject.Services.Ingredients.Models;

namespace NovaRecipesProject.Services.Ingredients;

/// <summary>
/// Main Ingredient service used to work with DB
/// </summary>
public interface IIngredientService
{
    /// <summary>
    /// Method to get basic ingredients list
    /// </summary>
    /// <param name="offset">Sets offset for data it got</param>
    /// <param name="limit">Sets limit for number of data to return</param>
    /// <returns>Returns list of IngredientModel</returns>
    Task<IEnumerable<IngredientModel>> GetIngredients(int offset, int limit);
    /// <summary>
    /// Method to get basic ingredients list.
    /// Caches everything using data of some user
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="offset">Sets offset for data it got</param>
    /// <param name="limit">Sets limit for number of data to return</param>
    /// <returns>Returns list of IngredientModel</returns>
    Task<IEnumerable<IngredientModel>> GetIngredientsAndCacheForUser(int userId, int offset, int limit);
    /// <summary>
    /// Method to get ingredient by some Id
    /// </summary>
    /// <param name="id">Id of a ingredient</param>
    /// <returns>Exact ingredient with given Id, or error related to what occured in proccess</returns>
    Task<IngredientModel> GetIngredientById(int id);
    /// <summary>
    /// Uses argument to add new ingredient to a DB
    /// </summary>
    /// <param name="model">Model which is used to add new data to DB</param>
    /// <returns>Returns added ingredient data</returns>
    Task<IngredientModel> AddIngredient(AddIngredientModel model);
    /// <summary>
    /// Method used to update data in DB
    /// </summary>
    /// <param name="id">Id of exact DB entry to update</param>
    /// <param name="model">Model, which data will be used to update an entry in DB</param>
    /// <returns></returns>
    Task UpdateIngredient(int id, UpdateIngredientModel model);
    /// <summary>
    /// Method used to delete entry from DB
    /// </summary>
    /// <param name="id">Id of an entry to delete</param>
    /// <returns></returns>
    Task DeleteIngredient(int id);
}