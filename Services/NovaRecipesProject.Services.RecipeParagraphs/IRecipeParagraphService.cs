using NovaRecipesProject.Services.RecipeParagraphs.Models;

namespace NovaRecipesProject.Services.RecipeParagraphs;

/// <summary>
/// Main RecipeParagraph service used to work with DB
/// </summary>
public interface IRecipeParagraphService
{
    /// <summary>
    /// Method to get basic ingredients list
    /// </summary>
    /// <param name="offset">Sets offset for data it got</param>
    /// <param name="limit">Sets limit for number of data to return</param>
    /// <returns>Returns list of RecipeParagraphModel</returns>
    Task<IEnumerable<RecipeParagraphModel>> GetRecipeParagraphs(int offset = 0, int limit = 10);
    /// <summary>
    /// Method to get ingredient by some Id
    /// </summary>
    /// <param name="id">Id of a ingredient</param>
    /// <returns>Exact ingredient with given Id, or error related to what occured in proccess</returns>
    Task<RecipeParagraphModel> GetRecipeParagraphById(int id);
    /// <summary>
    /// Uses argument to add new ingredient to a DB
    /// </summary>
    /// <param name="model">Model which is used to add new data to DB</param>
    /// <returns>Returns added ingredient data</returns>
    Task<RecipeParagraphModel> AddRecipeParagraph(AddRecipeParagraphModel model);
    /// <summary>
    /// Method used to update data in DB
    /// </summary>
    /// <param name="id">Id of exact DB entry to update</param>
    /// <param name="model">Model, which data will be used to update an entry in DB</param>
    /// <returns></returns>
    Task UpdateRecipeParagraph(int id, UpdateRecipeParagraphModel model);
    /// <summary>
    /// Method used to delete entry from DB
    /// </summary>
    /// <param name="id">Id of an entry to delete</param>
    /// <returns></returns>
    Task DeleteRecipeParagraph(int id);
}