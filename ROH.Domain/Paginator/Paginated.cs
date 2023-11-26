// Ignore Spelling: Paginator

namespace ROH.Domain.Paginator
{
    public record Paginated(int Total, ICollection<dynamic> ObjectResponse);
}