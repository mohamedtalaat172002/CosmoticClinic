/*
 1- add the new treatment to the changeTracker [DB]
 2- upload the new images to cloudinary
 3- add the new images to treatment.Images [Change tracker] [DB]
 4-  save changes to DB
if any exception occurs during the process of saving or uploading,
delete the newly uploaded images from cloudinary using their publicIds

 */
using Clinic.Application.Abstractions.Persistence;
using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Treatments.Commands.Models;
using Clinic.Application.Features.Treatments.Queries.Result;
using Clinic.Application.Interfaces.Repositories.ITreatmentRepo;
using Clinic.Application.Interfaces.Services;
using Clinic.Domain.Entities;
using Clinic.Domain.Entities.treatment;
using MediatR;

namespace Clinic.Application.Features.Treatments.Commands.Handlers
{
    internal sealed class AddTreatmentCommandHandler
        : IRequestHandler<
            AddTreatmentCommand,
            Result<UpdateCreateSingleTreatmentDto>>
    {
        private readonly ITreatmentWriteRepository _treatmentWriteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISlugService _slugService;
        private readonly IImageStorage _imageStorage;
        public AddTreatmentCommandHandler(
            ITreatmentWriteRepository treatmentWriteRepository,
            IUnitOfWork unitOfWork,
            ISlugService slugService,
            IImageStorage imageStorage)
        {
            _treatmentWriteRepository = treatmentWriteRepository;
            _unitOfWork = unitOfWork;
            _slugService = slugService;
            _imageStorage = imageStorage;
        }

        public async Task<Result<UpdateCreateSingleTreatmentDto>> Handle(
            AddTreatmentCommand request,
            CancellationToken cancellationToken)
        {
            var treatment = new Treatment
            {
                Name = request.Name,
                Description = request.Description
            };

            treatment.Slug = _slugService.GenerateSlug(
                request.Name,
                treatment.Id);

            var uploadedPublicIds = new List<string>();
            try
            {
                await _treatmentWriteRepository.AddAsync(treatment);
                await uploadImages(request, treatment, uploadedPublicIds, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch
            {

                await _imageStorage.DeleteAssetsQuietlyAsync(uploadedPublicIds);
                throw;
            }





            var dto = new UpdateCreateSingleTreatmentDto
            {
                Id = treatment.Id,
                Slug = treatment.Slug,
                Name = treatment.Name,
                Description = treatment.Description,
                Images = treatment.Images
                    .OrderBy(x => x.DisplayOrder)
                    .Select(x => new TreatmentImageDto
                    {
                        Id = x.Id,
                        Url = x.Url,
                        IsPrimary = x.IsPrimary,
                        DisplayOrder = x.DisplayOrder
                    })
                    .ToList()
            };

            return Result<UpdateCreateSingleTreatmentDto>.Success(
                dto,
                "Treatment added successfully.");

            async Task<List<String>> uploadImages(AddTreatmentCommand request, Treatment treatment, List<string> uploadedPublicIds, CancellationToken cancellationToken)
            {
                var displayOrder = 0;

                foreach (var imageRequest in request.Images)
                {
                    var image = await _imageStorage.UploadAsync(
                        imageRequest.FileStream,
                        imageRequest.FileName,
                        "clinic/treatments",
                        cancellationToken);

                    uploadedPublicIds.Add(image.PublicId);
                    var treatmentImage = new TreatmentImage(
                        treatment.Id,
                        image.Url,
                        image.PublicId,
                        displayOrder,
                        displayOrder == 0);

                    treatment.Images.Add(treatmentImage);

                    displayOrder++;

                }
                return uploadedPublicIds;
            }


        }
    }
}
