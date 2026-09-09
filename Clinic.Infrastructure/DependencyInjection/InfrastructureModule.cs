using Clinic.Application.Abstractions.Persistence;
using Clinic.Application.Interfaces.Repositories.IArticalRepo;
using Clinic.Application.Interfaces.Repositories.ProductRepo;
using Clinic.Application.Interfaces.Services;
using Clinic.Infrastructure.Persistence;
using Clinic.Infrastructure.Persistence.DbContext;
using Clinic.Infrastructure.Persistence.Repositories;
using Clinic.Infrastructure.Persistence.Repositories.ArticleRepo;
using Clinic.Infrastructure.Persistence.Repositories.ProductRepo;
using Clinic.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Clinic.Infrastructure.DependencyInjection
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IIdentityService, IdentityServies>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IArticleReadRepository, ArticleReadRepository>();
            services.AddScoped<IArticleWriteRepository, ArticleWriteRepository>();
            services.AddScoped(
                    typeof(IGenericRepository<,>),
                    typeof(GenericRepository<,>)
                );
            services.AddScoped<ISlugService, SlugService>();
            return services;

        }
    }
}
