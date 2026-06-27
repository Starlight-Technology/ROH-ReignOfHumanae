namespace ROH.Gateway.Realtime;

public sealed record ChatValidationResult(bool IsValid, string Code, string Message, string SanitizedMessage)
{
    public static ChatValidationResult Success(string sanitizedMessage) =>
        new(true, string.Empty, string.Empty, sanitizedMessage);

    public static ChatValidationResult Failure(string code, string message) =>
        new(false, code, message, string.Empty);
}
