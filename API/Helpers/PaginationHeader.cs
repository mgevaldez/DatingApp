namespace API.Helpers;

public class PaginationHeader(int currentPage, int pageSize, int totalCount, int totalPages)
{
    public int CurrentPage { get; set; } = currentPage;
    public int PageSize { get; set; } = pageSize;
    public int TotalCount { get; set; } = totalCount;
    public int TotalPages { get; set; } = totalPages;
}