//using System.Collections;

//namespace Clinic.web.Models;

///// <summary>Maps treatment DTOs into MVC view models without coupling the UI to Application's internal DTOs.</summary>
//public static class TreatmentViewMapper
//{
//    public static IReadOnlyList<TreatmentSummary> ToSummaries(object? items)
//        => items is IEnumerable enumerable
//            ? enumerable.Cast<object>().Select(ToSummary).ToList()
//            : [];

//    public static TreatmentSummary ToSummary(object source) => new()
//    {
//        Id = Get<Guid>(source, "Id"),
//        Name = Get<string>(source, "Name") ?? string.Empty,
//        Description = Get<string>(source, "Description") ?? string.Empty,
//        ImageUrl = Get<string>(source, "ImageUrl")
//    };

//    public static TreatmentDetails ToDetails(object source) => new()
//    {
//        Id = Get<Guid>(source, "Id"),
//        Name = Get<string>(source, "Name") ?? string.Empty,
//        Description = Get<string>(source, "Description") ?? string.Empty,
//        ImageUrl = Get<string>(source, "ImageUrl"),
//        Articles = ToArticles(Get<object>(source, "Articles")?.GetType().GetProperty("Items")?.GetValue(Get<object>(source, "Articles")))
//    };

//    private static IReadOnlyList<ArticleSummary> ToArticles(object? items)
//        => items is IEnumerable enumerable
//            ? enumerable.Cast<object>().Select(item => new ArticleSummary
//            {
//                Id = Get<Guid>(item, "Id"),
//                Title = Get<string>(item, "Title") ?? string.Empty,
//                Summary = Get<string>(item, "Summary") ?? string.Empty
//            }).ToList()
//            : [];

//    private static T? Get<T>(object source, string name)
//        => source.GetType().GetProperty(name)?.GetValue(source) is T value ? value : default;
//}
