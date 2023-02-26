namespace NovaRecipesProject.Common.Responses;

#pragma warning disable CS8618

public class ErrorResponse
{
    public int ErrorCode { get; set; }
    public string Message { get; set; }
    public IEnumerable<ErrorResponseFieldInfo> FieldErrors { get; set; }
}

public class ErrorResponseFieldInfo
{
    public string FieldName { get; set; }
    public string Message { get; set; }
}
