using AutoMapper;
using Clinic.Application.Abstractions.Persistence;
using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Treatments.Commands.Models;
using Clinic.Application.Features.Treatments.Queries.Result;
using Clinic.Application.Interfaces.Repositories.ITreatmentRepo;
using Clinic.Application.Interfaces.Services;
using MediatR;

namespace Clinic.Application.Features.Treatments.Commands.Handlers
{
    internal sealed class UpdateTreatmentCommandHandler
        : IRequestHandler<UpdateTreatmentCommand, Result<UpdateSingleTreatmentDto>>
    {
        private readonly ITreatmentReadRepository _treatmentReadRepository;
        private readonly ITreatmentWriteRepository _treatmentWriteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISlugService _slugService;

        public UpdateTreatmentCommandHandler(
            ITreatmentReadRepository treatmentReadRepository,
            ITreatmentWriteRepository treatmentWriteRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ISlugService slugService)
        {
            _treatmentReadRepository = treatmentReadRepository;
            _treatmentWriteRepository = treatmentWriteRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _slugService = slugService;
        }

        public async Task<Result<UpdateSingleTreatmentDto>> Handle(
            UpdateTreatmentCommand request,
            CancellationToken cancellationToken)
        {
            var treatment = await _treatmentReadRepository.GetById(request.Id);

            if (treatment is null)
            {
                return Result<UpdateSingleTreatmentDto>.Failure(
                    "Treatment not found.",
                    ErrorType.NotFound);
            }

            _mapper.Map(request, treatment);

            treatment.Slug = _slugService.GenerateSlug(
                request.Name,
                treatment.Id);

            _treatmentWriteRepository.Update(treatment);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var treatmentDto = _mapper.Map<UpdateSingleTreatmentDto>(treatment);

            return Result<UpdateSingleTreatmentDto>.Success(
                treatmentDto,
                "Treatment updated successfully.");
        }
    }
}