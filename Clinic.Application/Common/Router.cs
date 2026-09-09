namespace Clinic.Application.Common
{

    public static class Router
    {
        public const string SingleRoute = "/{id:Guid}";

        public const string Root = "Api";
        public const string Version = "V1";
        public const string Rule = Root + "/" + Version + "/";

        public static class ArticleRouting
        {
            public const string Prefix = Rule + "Article";
            public const string List = Prefix + "/List";
            public const string GetByID = Prefix + SingleRoute;   // Api/V1/Article/{id:int}
            public const string GetBySlug = Prefix + "/{slug}";   // Api/V1/Article/{slug}
            public const string Create = Prefix + "/Create";
            public const string Edit = Prefix + SingleRoute;      // Api/V1/Article/{id:int}
            public const string Delete = Prefix + SingleRoute;    // Api/V1/Article/{id:int}
        }
    }
}
