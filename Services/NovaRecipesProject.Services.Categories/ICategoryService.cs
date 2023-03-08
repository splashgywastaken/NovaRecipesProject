using NovaRecipesProject.Services.Categories.Models;

namespace NovaRecipesProject.Services.Categories;

/// <summary>
/// Categories's service with main methods to use with DB
/// </summary>
public interface ICategoryService
{
    /// <summary>
    /// Method to get basic categories list
    /// </summary>
    /// <param name="offset">Sets offset for data it got</param>
    /// <param name="limit">Sets limit for number of data to return</param>
    /// <returns>Returns list of CategoryModel</returns>
    Task<IEnumerable<CategoryModel>> GetCategories(int offset = 0, int limit = 10);
    /// <summary>
    /// Method to get category by some Id
    /// </summary>
    /// <param name="id">Id of a category</param>
    /// <returns>Exact category with given Id, or error related to what occured in proccess</returns>
    Task<CategoryModel> GetCategoryById(int id);
    /// <summary>
    /// Uses argument to add new category to a DB
    /// </summary>
    /// <param name="model">Model which is used to add new data to DB</param>
    /// <returns>Returns added category data</returns>
    Task<CategoryModel> AddCategory(AddCategoryModel model);
    /// <summary>
    /// Method used to update data in DB
    /// </summary>
    /// <param name="id">Id of exact DB entry to update</param>
    /// <param name="model">Model, which data will be used to update an entry in DB</param>
    /// <returns></returns>
    Task UpdateCategory(int id, UpdateCategoryModel model);
    /// <summary>
    /// Method used to delete entry from DB
    /// </summary>
    /// <param name="id">Id of an entry to delete</param>
    /// <returns></returns>
    Task DeleteCategory(int id);
}