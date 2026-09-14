using AutoMapper;
using Clinic.Application.Abstractions.Persistence;
using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Consultations.Commands.Models;
using Clinic.Application.Features.Consultations.Queries.Result;
using Clinic.Application.Interfaces.Repositories.IConsultationRepo;
using MediatR;

namespace Clinic.Application.Features.Consultations.Commands.Handlers
{
    public class UpdateConsultationCommandHandler : IRequestHandler<UpdateConsultationCommand, Result<SingleConsultationDto>>
    {
        private readonly IMapper _mapper;
        private readonly IConsultationReadRepository _consultationReadRepository;
        private readonly IConsultationWriteRepository _consultationWriteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateConsultationCommandHandler(IMapper mapper, IConsultationReadRepository consultationReadRepository, IConsultationWriteRepository consultationWriteRepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _consultationReadRepository = consultationReadRepository;
            _consultationWriteRepository = consultationWriteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<SingleConsultationDto>> Handle(UpdateConsultationCommand request, CancellationToken cancellationToken)
        {
            var consultation = await _consultationReadRepository.GetById(request.Id);
            if (consultation is null)
            {
                return Result<SingleConsultationDto>.Failure(
                    "Consultation not found.",
                    ErrorType.NotFound);
            }


            _mapper.Map(request, consultation);
            _consultationWriteRepository.Update(consultation);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var consultationDto = _mapper.Map<SingleConsultationDto>(consultation);
            return Result<SingleConsultationDto>.Success(
                consultationDto,
                "Consultation updated successfully.");
        }
    }
}
