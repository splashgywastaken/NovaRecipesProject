namespace NovaRecipesProject.Common.Exceptions;

/// <summary>
/// Base exception for transferred error in the work process
/// </summary>
public class ProcessException : Exception
{
    /// <summary>
    ///Error code
    /// </summary>
    public string Code { get; } = null!; 

    /// <summary>
    /// Error name
    /// </summary>
    public string Name { get; } = null!;

    #region Constructors

    /// <inheritdoc />
    public ProcessException()
    {
    }

    private ProcessException(string message) : base(message, new Exception(message))
    {
    }

    /// <inheritdoc />
    public ProcessException(Exception inner) : base(inner.Message, inner)
    {
    }

    /// <inheritdoc />
    public ProcessException(string message, Exception inner) : base(message, inner)
    {
    }

    /// <inheritdoc />
    public ProcessException(string code, string message) : base(message)
    {
        Code = code;
    }

    /// <inheritdoc />
    public ProcessException(string code, string message, Exception inner) : base(message, inner)
    {
        Code = code;
    }

    #endregion

    /// <summary>
    /// Used to Throw exception with exact message
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="message"></param>
    /// <exception cref="ProcessException"></exception>
    public static void ThrowIf(Func<bool> predicate, string message)
    {
        if (predicate.Invoke())
            throw new ProcessException(message);
    }
}
