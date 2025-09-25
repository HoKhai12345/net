public interface IPagedResult<T>
{
    List<T> Items { get; set; }
    long TotalCount { get; set; }
    int PageSize { get; set; }
    int PageNumber { get; set; }
}