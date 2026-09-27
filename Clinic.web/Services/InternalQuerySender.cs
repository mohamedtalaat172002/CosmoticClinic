//using MediatR;

//namespace Clinic.web.Services;

///// <summary>
///// Sends Application requests whose response DTO is internal, then exposes only the result envelope needed by MVC.
///// The web project keeps its own display models instead of referencing internal Application DTOs.
///// </summary>
//public sealed class InternalQuerySender(ISender sender)
//{
//    public async Task<InternalQueryResult> SendAsync(object request, CancellationToken cancellationToken)
//    {
//        var responseType = request.GetType().GetInterfaces()
//            .Single(contract => contract.IsGenericType && contract.GetGenericTypeDefinition() == typeof(IRequest<>))
//            .GetGenericArguments()[0];

//        var sendMethod = typeof(ISender).GetMethods()
//            .Single(method => method.Name == nameof(ISender.Send)
//                && method.IsGenericMethodDefinition
//                && method.GetParameters().Length == 2
//                && method.GetParameters()[0].ParameterType.IsGenericType
//                && method.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(IRequest<>))
//            .MakeGenericMethod(responseType);

//        var task = (Task)sendMethod.Invoke(sender, [request, cancellationToken])!;
//        await task;
//        var result = task.GetType().GetProperty("Result")!.GetValue(task)!;
//        var type = result.GetType();

//        return new InternalQueryResult(
//            (bool)(type.GetProperty("IsSuccess")!.GetValue(result) ?? false),
//            type.GetProperty("Value")?.GetValue(result),
//            type.GetProperty("Message")?.GetValue(result) as string);
//    }
//}

//public sealed record InternalQueryResult(bool IsSuccess, object? Value, string? Message);
