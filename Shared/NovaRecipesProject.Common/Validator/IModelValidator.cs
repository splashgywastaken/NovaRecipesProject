namespace NovaRecipesProject.Common.Validator;

/// <summary>
/// Class used for DTO validation with FluentValidation
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IModelValidator<in T> where T : class
{
    /// <summary>
    /// Method used to check model validity
    /// </summary>
    /// <param name="model"></param>
    void Check(T model);
}