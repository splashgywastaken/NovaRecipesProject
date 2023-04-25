using NovaRecipesProject.Services.RecipeParagraphs.Models;

namespace NovaRecipesProject.Services.RecipeParagraphs;

/// <summary>
/// Main RecipeParagraph service used to work with DB
/// </summary>
public interface IRecipeParagraphService
{
    /// <summary>
    /// Method to get paragraphs of a certain recipe
    /// </summary>
    /// <param name="recipeId">Recipe's id</param>
    /// <param name="offset">Sets offset for data it got</param>
    /// <param name="limit">Sets limit for number of data to return</param>
    /// <returns>Returns list of RecipeParagraphModel</returns>
    Task<IEnumerable<RecipeParagraphModel>> GetRecipeParagraphsByRecipesId(int recipeId, int offset, int limit);

    /// <summary>
    /// Method to get paragraphs of a certain recipe and cache them using data about some user
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="recipeId">Recipe's id</param>
    /// <param name="offset">Sets offset for data it got</param>
    /// <param name="limit">Sets limit for number of data to return</param>
    /// <returns>Returns list of RecipeParagraphModel</returns>
    Task<IEnumerable<RecipeParagraphModel>> GetRecipeParagraphsByRecipesIdAndCacheForUser(
        string userId,
        int recipeId, 
        int offset = 0,
        int limit = 10
        );
    /// <summary>
    /// Method to get recipe paragraph by some Id
    /// </summary>
    /// <param name="id">Id of a recipe paragraph</param>
    /// <returns>Exact recipe paragraph with given Id, or error related to what occured in proccess</returns>
    Task<RecipeParagraphModel> GetRecipeParagraphById(int id);
    /// <summary>
    /// Uses argument to add new recipe paragraph to a DB
    /// </summary>
    /// <param name="recipeId">Id of recipe</param>
    /// <param name="model">Model which is used to add new data to DB</param>
    /// <returns>Returns added recipe paragraph data</returns>
    Task<RecipeParagraphModel> AddRecipeParagraph(int recipeId, AddRecipeParagraphModel model);
    /// <summary>
    /// Method used to update data in DB
    /// </summary>
    /// <param name="id">Id of exact DB entry to update</param>
    /// <param name="model">Model, which data will be used to update an entry in DB</param>
    /// <returns></returns>
    Task UpdateRecipeParagraph(int id, UpdateRecipeParagraphModel model);
    /// <summary>
    /// Method used to update data of a recipe in DB, where it updates only orderNumber value
    /// </summary>
    /// <param name="orderNumber">Order number to update to</param>
    /// <param name="id">Id of a recipe paragraph to update</param>
    /// <returns></returns>
    Task ChangeRecipeParagraphOrderNumber(int orderNumber, int id);
    /// <summary>
    /// Method used to delete entry from DB
    /// </summary>
    /// <param name="id">Id of an entry to delete</param>
    /// <returns></returns>
    Task DeleteRecipeParagraph(int id);
}