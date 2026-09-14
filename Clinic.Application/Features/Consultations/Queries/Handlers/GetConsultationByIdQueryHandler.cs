using AutoMapper;
using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Consultations.Queries.Models;
using Clinic.Application.Features.Consultations.Queries.Result;
using Clinic.Application.Interfaces.Repositories.IConsultationRepo;
using MediatR;

namespace Clinic.Application.Features.Consultations.Queries.Handlers
{
    public sealed class GetConsultationByIdQueryHandler
     : IRequestHandler<GetConsultationByIdQuery, Result<SingleConsultationDto>>
    {
        private readonly IConsultationReadRepository _consultationReadRepository;
        private readonly IMapper _mapper;

        public GetConsultationByIdQueryHandler(
            IConsultationReadRepository consultationReadRepository,
            IMapper mapper)
        {
            _consultationReadRepository = consultationReadRepository;
            _mapper = mapper;
        }

        public async Task<Result<SingleConsultationDto>> Handle(
            GetConsultationByIdQuery request,
            CancellationToken cancellationToken)
        {
            var consultation = await _consultationReadRepository.GetById(request.id);

            if (consultation is null)
                return Result<SingleConsultationDto>.Failure("Consultation not found", ErrorType.NotFound);

            var dto = _mapper.Map<SingleConsultationDto>(consultation);

            return Result<SingleConsultationDto>.Success(dto);
        }
    }
}
