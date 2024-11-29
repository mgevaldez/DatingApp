namespace API.Helpers;

public class PaginationParams
{
    private int _defaultPageSize = 10;
    private const int MaxPageSize = 50;
    private const int DefaultPageNumber = 1;

    public int PageNumber { get; set; } = DefaultPageNumber;

    public int PageSize
    {
        get => _defaultPageSize;
        set => _defaultPageSize = value > MaxPageSize ? MaxPageSize : value;
    }
}