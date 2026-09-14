using Clinic.Application.Common.Extensions;
using Clinic.Application.Common.SearchCriteria;
using Clinic.Application.Interfaces.Repositories.ITreatmentRepo;
using Clinic.Domain.Entities.treatment;
using Clinic.Infrastructure.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Infrastructure.Persistence.Repositories.TreatmentRepo
{
    public class TreatmentReadRepository : SlugReadRepository<Treatment, TreatmentSearchCriteria, Guid>,
          ITreatmentReadRepository
    {
        public TreatmentReadRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override IQueryable<Treatment> ApplyCustomFilters(IQueryable<Treatment> query, TreatmentSearchCriteria criteria)
        {
            var search = criteria.Search?.Trim();

            return query
                .WhereIf(
                    !string.IsNullOrWhiteSpace(search),
                    a =>
                        EF.Functions.Like(a.Name, $"%{search}%") ||
                        EF.Functions.Like(a.Description, $"%{search}%"));


        }

        public async Task<Treatment?> GetByIdWithArticlesAsync(Guid id)
        {
            return await _context.Set<Treatment>()
                .AsNoTracking()
                .Include(t => t.Articles)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
