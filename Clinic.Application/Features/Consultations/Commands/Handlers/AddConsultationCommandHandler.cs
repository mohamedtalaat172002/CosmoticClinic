using AutoMapper;
using Clinic.Application.Abstractions.Persistence;
using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Consultations.Commands.Models;
using Clinic.Application.Features.Consultations.Queries.Result;
using Clinic.Application.Interfaces.Repositories.IConsultationRepo;
using Clinic.Domain.Entities.consultation;
using MediatR;

namespace Clinic.Application.Features.Consultations.Commands.Handlers
{
    public class AddConsultationCommandHandler : IRequestHandler<AddConsultationCommand, Result<SingleConsultationDto>>
    {
        private readonly IConsultationWriteRepository _consultationWriteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AddConsultationCommandHandler(IConsultationWriteRepository consultationWriteRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _consultationWriteRepository = consultationWriteRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<SingleConsultationDto>> Handle(AddConsultationCommand request, CancellationToken cancellationToken)
        {
            var consultation = _mapper.Map<Consultation>(request);
            consultation.Status = ConsultationStatus.Pending;
            await _consultationWriteRepository.AddAsync(consultation);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var consultationDto = _mapper.Map<SingleConsultationDto>(consultation);

            return Result<SingleConsultationDto>.Success(
                consultationDto,
                "Consultation added successfully.");
        }
    }
}
