namespace Clinic.Application.Common
{

    public static class Router
    {
        public const string Root = "Api";
        public const string Version = "V1";
        public const string Rule = Root + "/" + Version + "/";

        public static class ArticleRouting
        {
            private const string SingleRoute = "/{id:guid}";
            public const string Prefix = Rule + "Article";
            public const string List = Prefix + "/List";
            public const string GetByID = Prefix + SingleRoute;
            public const string GetBySlug = Prefix + "/{slug}";
            public const string Create = Prefix + "/Create";
            public const string Edit = Prefix + SingleRoute;
            public const string Delete = Prefix + SingleRoute;
        }

        public static class TreatmentRouting
        {
            private const string SingleRoute = "/{id:guid}";
            public const string Prefix = Rule + "Treatment";
            public const string List = Prefix + "/List";
            public const string GetByID = Prefix + SingleRoute;
            public const string GetBySlug = Prefix + "/{slug}";
            public const string Create = Prefix + "/Create";
            public const string Edit = Prefix + SingleRoute;
            public const string Delete = Prefix + SingleRoute;
        }
        public static class ConsultationRouting
        {
            private const string SingleRoute = "/{id:guid}";
            public const string Prefix = Rule + "Consultation";
            public const string List = Prefix + "/List";
            public const string GetByID = Prefix + SingleRoute;
            public const string Create = Prefix + "/Create";
            public const string Edit = Prefix + SingleRoute;
            public const string Delete = Prefix + SingleRoute;
        }
    }
}
