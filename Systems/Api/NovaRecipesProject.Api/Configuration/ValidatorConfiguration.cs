using NovaRecipesProject.Common.Extensions;

namespace NovaRecipesProject.Api.Configuration;

using Common.Helpers;
using Common.Responses;
using Common.Validator;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

/// <summary>
/// Configuration class for validation
/// </summary>
public static class ValidatorConfiguration
{
    /// <summary>
    /// Method for DI
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IMvcBuilder AddAppValidator(this IMvcBuilder builder)
    {
        builder.ConfigureApiBehaviorOptions(options =>
         {
             options.InvalidModelStateResponseFactory = context =>
             {
                 var fieldErrors = new List<ErrorResponseFieldInfo>();
                 foreach (var (field, state) in context.ModelState)
                 {
                     if (state.ValidationState == ModelValidationState.Invalid)
                         fieldErrors.Add(new ErrorResponseFieldInfo()
                         {
                             FieldName = field.ToCamelCase(),
                             Message = string.Join(", ", state.Errors.Select(x => x.ErrorMessage))
                         });
                 }

                 var result = new BadRequestObjectResult(new ErrorResponse()
                 {
                     ErrorCode = 100,
                     Message = "One or more validation errors occurred.",
                     FieldErrors = fieldErrors
                 });

                 return result;
             };
         });
        
        builder.AddFluentValidation(fv =>
        {
            fv.DisableDataAnnotationsValidation = true;
            fv.ImplicitlyValidateChildProperties = true;
            fv.AutomaticValidationEnabled = true;
        });

        ValidatorsRegisterHelper.Register(builder.Services);

        builder.Services.AddSingleton(typeof(IModelValidator<>), typeof(ModelValidator<>));

        return builder;
    }
}