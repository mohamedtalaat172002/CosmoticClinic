namespace Clinic.Application.Common.SearchCriteria
{
    public abstract class BaseSearchCriteria
    {
        private const int DefaultPageSize = 10;
        private const int MaxPageSize = 50;

        public string? Search { get; set; }
        public bool? IsDeleted { get; set; } = false;

        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }

        public int NormalizedPageIndex => Math.Max(PageIndex ?? 1, 1);
        public int NormalizedPageSize => Math.Clamp(PageSize ?? DefaultPageSize, 1, MaxPageSize);
    }
}