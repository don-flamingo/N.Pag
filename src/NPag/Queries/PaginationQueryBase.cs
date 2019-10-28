using NPag.Settings;

namespace NPag.Queries
{
    public abstract class PaginationQueryBase : IPaginationQuery
    {
        public string OrderBy { get; }
        public string Where { get; }
        public int Page { get; }
        public int PageSize { get; } = PaginationSettings.DefaultPageSize;
    }
}