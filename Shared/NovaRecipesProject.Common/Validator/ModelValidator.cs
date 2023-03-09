namespace NovaRecipesProject.Common.Validator;

using FluentValidation;

/// <inheritdoc />
public class ModelValidator<T> : IModelValidator<T> where T : class
{
    private readonly IValidator<T> _validator;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="validator"></param>
    public ModelValidator(IValidator<T> validator)
    {
        _validator = validator;
    }

    /// <inheritdoc />
    public void Check(T model)
    {
        var result = _validator.Validate(model);
        if (!result.IsValid)
            throw new ValidationException(result.Errors);
    }
}