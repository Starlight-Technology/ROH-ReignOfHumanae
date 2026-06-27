using ROH.Contracts.WebSocket.Chat;
using ROH.Gateway.Realtime;

namespace ROH.Test.Realtime;

public class ChatMessageValidatorTests
{
    [Fact]
    public void Validate_ShouldNormalizeWhitespaceAndControlCharacters()
    {
        ChatValidationResult result = ChatMessageValidator.Validate(
            ChatChannel.Global,
            "  hello\r\n\tworld\u0001\u202e  ",
            300);

        Assert.True(result.IsValid);
        Assert.Equal("hello world", result.SanitizedMessage);
    }

    [Fact]
    public void Validate_ShouldRejectEmptyMessage()
    {
        ChatValidationResult result = ChatMessageValidator.Validate(ChatChannel.Global, " \r\n\t ", 300);

        Assert.False(result.IsValid);
        Assert.Equal("EmptyMessage", result.Code);
    }

    [Fact]
    public void Validate_ShouldRejectMessageOverConfiguredLimit()
    {
        ChatValidationResult result = ChatMessageValidator.Validate(ChatChannel.Global, "123456", 5);

        Assert.False(result.IsValid);
        Assert.Equal("MessageTooLong", result.Code);
    }

    [Fact]
    public void Validate_ShouldRejectUnsupportedChannel()
    {
        ChatValidationResult result = ChatMessageValidator.Validate(ChatChannel.Private, "hello", 300);

        Assert.False(result.IsValid);
        Assert.Equal("UnsupportedChannel", result.Code);
    }
}
