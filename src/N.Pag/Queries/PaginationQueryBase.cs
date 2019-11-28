using N.Pag.Settings;

namespace N.Pag.Queries
{
    public abstract class PaginationQueryBase : IPaginationQuery
    {
        public string OrderBy { get; }
        public string Where { get; }
        public int Page { get; }
        public int PageSize { get; } = PaginationSettings.DefaultPageSize;
    }
}