using Clinic.Application.Common.Extensions;
using Clinic.Application.Common.SearchCriteria;
using Clinic.Application.Interfaces.Repositories.IConsultationRepo;
using Clinic.Domain.Entities.consultation;
using Clinic.Infrastructure.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Infrastructure.Persistence.Repositories.ConsultationRepo
{
    public class ConsultationReadRepository : ReadRepository<Consultation, ConsultationSearchCriteria, Guid>, IConsultationReadRepository
    {
        public ConsultationReadRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override IQueryable<Consultation> ApplyCustomFilters(IQueryable<Consultation> query, ConsultationSearchCriteria criteria)
        {
            var search = criteria.Search?.Trim();

            return query
                .WhereIf(
                    !string.IsNullOrWhiteSpace(search),
                    a =>
                        EF.Functions.Like(a.FullName, $"%{search}%") ||
                        EF.Functions.Like(a.Message, $"%{search}%"))
                .WhereIf(criteria.Status.HasValue,
                    a => a.Status == criteria.Status.Value); ;

        }

    }
}
