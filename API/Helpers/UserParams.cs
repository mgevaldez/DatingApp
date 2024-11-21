namespace API.Helpers;

public class UserParams
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
    
    public string? Gender { get; set; }
    public string? CurrentUsername { get; set; }
    public int MinAge { get; set; } = 18;
    public int MaxAge { get; set; } = 60;
    public string? OrderBy { get; set; } = "lastActive";
}