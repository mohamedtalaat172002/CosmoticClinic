namespace Clinic.web.Models;

// Kept for any future HTTP client / API contract use.
// The MVC views use Clinic.Application.Abstractions.Queries.PagedResult<T> directly.
public sealed class ApiResult<T>
{
    public bool IsSuccess { get; init; }
    public T? Value { get; init; }
    public string? Message { get; init; }
}
