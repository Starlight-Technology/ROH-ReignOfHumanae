using ROH.Contracts.WebSocket.Chat;

using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace ROH.Gateway.Realtime;

public static partial class ChatMessageValidator
{
    public static ChatValidationResult Validate(
        ChatChannel channel,
        string? message,
        int maximumLength)
    {
        if (channel != ChatChannel.Global)
            return ChatValidationResult.Failure("UnsupportedChannel", "Somente o canal Global está disponível.");

        string sanitized = Sanitize(message);
        if (string.IsNullOrWhiteSpace(sanitized))
            return ChatValidationResult.Failure("EmptyMessage", "A mensagem não pode estar vazia.");

        int safeMaximumLength = maximumLength > 0 ? maximumLength : 300;
        return sanitized.Length > safeMaximumLength
            ? ChatValidationResult.Failure(
                "MessageTooLong",
                $"A mensagem excede o limite de {safeMaximumLength} caracteres.")
            : ChatValidationResult.Success(sanitized);
    }

    public static string Sanitize(string? message)
    {
        if (string.IsNullOrEmpty(message))
            return string.Empty;

        StringBuilder builder = new(message.Length);
        foreach (char character in message)
        {
            if (character is '\r' or '\n' or '\t')
            {
                builder.Append(' ');
                continue;
            }

            if (!char.IsControl(character)
                && CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.Format)
            {
                builder.Append(character);
            }
        }

        return WhitespaceRegex().Replace(builder.ToString(), " ").Trim();
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex WhitespaceRegex();
}
