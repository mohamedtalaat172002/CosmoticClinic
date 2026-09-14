using AutoMapper;
using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Treatments.Queries.Models;
using Clinic.Application.Features.Treatments.Queries.Result;
using Clinic.Application.Interfaces.Repositories.ITreatmentRepo;
using MediatR;

namespace Clinic.Application.Features.Treatments.Queries.Handler
{
    internal sealed class GetTreatmentBySlugQueryHandler
     : IRequestHandler<GetTreatmentBySlugQuery, Result<SingleTreatmentDto>>
    {
        private readonly ITreatmentReadRepository _treatmentReadRepository;
        private readonly IMapper _mapper;

        public GetTreatmentBySlugQueryHandler(
            ITreatmentReadRepository treatmentReadRepository,
            IMapper mapper)
        {
            _treatmentReadRepository = treatmentReadRepository;
            _mapper = mapper;
        }

        public async Task<Result<SingleTreatmentDto>> Handle(
            GetTreatmentBySlugQuery request,
            CancellationToken cancellationToken)
        {
            var treatment = await _treatmentReadRepository.GetBySlugAsync(request.Slug);

            if (treatment is null || treatment.IsDeleted)
            {
                return Result<SingleTreatmentDto>.Failure(
                    "Treatment not found.",
                    ErrorType.NotFound);
            }

            var dto = _mapper.Map<SingleTreatmentDto>(treatment);

            return Result<SingleTreatmentDto>.Success(dto);
        }
    }
}
