using Clinic.Application.Abstractions.Persistence;
using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Consultations.Commands.Models;
using Clinic.Application.Interfaces.Repositories.IConsultationRepo;
using MediatR;

namespace Clinic.Application.Features.Consultations.Commands.Handlers
{
    public class DeleteConsultationCommandHandler : IRequestHandler<DeleteConsultationCommand, Result<bool>>
    {
        private readonly IConsultationWriteRepository _consultationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConsultationReadRepository _consultationReadRepository;

        public DeleteConsultationCommandHandler(IConsultationWriteRepository consultationRepository, IUnitOfWork unitOfWork, IConsultationReadRepository consultationReadRepository)
        {
            _consultationRepository = consultationRepository;
            _unitOfWork = unitOfWork;
            _consultationReadRepository = consultationReadRepository;
        }

        public async Task<Result<bool>> Handle(DeleteConsultationCommand request, CancellationToken cancellationToken)
        {
            var consultation = await _consultationReadRepository.GetById(request.Id);
            if (consultation == null) { return Result<bool>.Failure("Consultation not found", ErrorType.NotFound); }
            _consultationRepository.Remove(consultation);
            await _unitOfWork.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
    }
}
