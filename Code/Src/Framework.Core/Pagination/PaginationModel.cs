namespace Framework.Core.Pagination;

public abstract class PaginationModel
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPageCount { get; set; }
}