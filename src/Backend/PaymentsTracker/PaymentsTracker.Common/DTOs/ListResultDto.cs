namespace PaymentsTracker.Common.DTOs;

public record ListResultDto<T>(IReadOnlyList<T> Data, int PageSize, int PageNumber, int TotalItems)
{
    public int TotalPages
    {
        get
        {
            if (TotalItems <= PageSize)
                return 1;

            return (int)Math.Ceiling((double)TotalItems / PageSize);
        }
    }
}