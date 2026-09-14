using Clinic.Application.Abstractions.Persistence;
using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Treatments.Commands.Models;
using Clinic.Application.Interfaces.Repositories.ITreatmentRepo;
using MediatR;

namespace Clinic.Application.Features.Treatments.Commands.Handlers
{
    internal sealed class DeleteTreatmentCommandHandler
        : IRequestHandler<DeleteTreatmentCommand, Result<bool>>
    {
        private readonly ITreatmentReadRepository _treatmentReadRepository;
        private readonly ITreatmentWriteRepository _treatmentWriteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTreatmentCommandHandler(
            ITreatmentReadRepository treatmentReadRepository,
            ITreatmentWriteRepository treatmentWriteRepository,
            IUnitOfWork unitOfWork)
        {
            _treatmentReadRepository = treatmentReadRepository;
            _treatmentWriteRepository = treatmentWriteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(
            DeleteTreatmentCommand request,
            CancellationToken cancellationToken)
        {
            var treatment = await _treatmentReadRepository.GetById(request.Id);

            if (treatment is null)
            {
                return Result<bool>.Failure(
                    "Treatment not found.",
                    ErrorType.NotFound);
            }

            _treatmentWriteRepository.Remove(treatment);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(
                true,
                "Treatment deleted successfully.");


        }
    }
}