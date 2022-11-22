namespace PaymentsTracker.Common.DTOs;

public record ListCriteriaDto<T>(T? Data = default, int PageSize = 10, int PageNumber = 1);