using Microsoft.AspNetCore.Mvc;
using PaymentsTracker.Common.DTOs;

namespace PaymentsTracker.API.Extensions;

public static class OperationResultEx
{
    public static IActionResult ToActionResult<T>(this OperationResult<T> result)
    {
        if (result.IsFailed)
            return new BadRequestObjectResult(result.Error);

        return new OkObjectResult(result.Data);
    }

    public static async Task<IActionResult> ToActionResult<T>(this Task<OperationResult<T>> result)
    {
        var taskResult = await result;
        return ToActionResult(taskResult);
    }
}