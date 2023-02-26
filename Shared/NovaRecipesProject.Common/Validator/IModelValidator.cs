﻿namespace NovaRecipesProject.Common.Validator;

public interface IModelValidator<T> where T : class
{
    void Check(T model);
}