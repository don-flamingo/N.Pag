namespace NPag.Queries
{
    public interface IPaginationQuery
    {
        string OrderBy { get; }
        string Where { get; }
        int Page { get; }
        int PageSize { get; }
    }
}