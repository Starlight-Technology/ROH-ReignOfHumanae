// Ignore Spelling: Paginator

using System.Collections.Generic;

namespace ROH.StandardModels.Paginator
{
    public class PaginatedModel
    {
        public int? TotalPages { get; set; } = 0;
        public ICollection<dynamic>? ObjectResponse { get; set; }
    }
}