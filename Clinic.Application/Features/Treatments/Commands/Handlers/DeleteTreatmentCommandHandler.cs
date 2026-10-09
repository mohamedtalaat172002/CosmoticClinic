using Clinic.Application.Abstractions.Persistence;
using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Treatments.Commands.Models;
using Clinic.Application.Interfaces.Repositories.ITreatmentRepo;
using Clinic.Application.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Clinic.Application.Features.Treatments.Commands.Handlers
{
    internal sealed class DeleteTreatmentCommandHandler
        : IRequestHandler<DeleteTreatmentCommand, Result<bool>>
    {
        private readonly ITreatmentWriteRepository _treatmentWriteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageStorage _imageStorage;
        private readonly ILogger<DeleteTreatmentCommandHandler> _logger;

        public DeleteTreatmentCommandHandler(
            ITreatmentWriteRepository treatmentWriteRepository,
            IUnitOfWork unitOfWork,
            IImageStorage imageStorage,
            ILogger<DeleteTreatmentCommandHandler> logger)
        {
            _treatmentWriteRepository = treatmentWriteRepository;
            _unitOfWork = unitOfWork;
            _imageStorage = imageStorage;
            _logger = logger;
        }

        public async Task<Result<bool>> Handle(
            DeleteTreatmentCommand request,
            CancellationToken cancellationToken)
        {
            var treatment =
                await _treatmentWriteRepository.GetWithImagesForUpdateAsync(
                    request.Id,
                    cancellationToken);

            if (treatment is null)
            {
                return Result<bool>.Failure(
                    "Treatment not found.",
                    ErrorType.NotFound);
            }

            var imagePublicIds = treatment.Images
                .Select(x => x.PublicId)
                .ToList();

            _treatmentWriteRepository.Remove(treatment);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _imageStorage.DeleteAssetsQuietlyAsync(
                imagePublicIds);

            return Result<bool>.Success(
                true,
                "Treatment deleted successfully.");
        }
    }
}
