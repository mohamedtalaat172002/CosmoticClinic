using AutoMapper;
using Clinic.Application.Abstractions.Persistence;
using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.Treatments.Commands.Models;
using Clinic.Application.Features.Treatments.Queries.Result;
using Clinic.Application.Interfaces.Repositories.ITreatmentRepo;
using Clinic.Application.Interfaces.Services;
using Clinic.Domain.Entities.treatment;
using MediatR;

namespace Clinic.Application.Features.Treatments.Commands.Handlers
{
    internal sealed class AddTreatmentCommandHandler : IRequestHandler<AddTreatmentCommand, Result<UpdateCreateSingleTreatmentDto>>
    {
        private readonly ITreatmentWriteRepository _treatmentWriteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISlugService _slugService;
        public AddTreatmentCommandHandler(ITreatmentWriteRepository treatmentWriteRepository, IUnitOfWork unitOfWork, IMapper mapper, ISlugService slugService)
        {
            _treatmentWriteRepository = treatmentWriteRepository;
            _unitOfWork = unitOfWork; _mapper = mapper;
            _slugService = slugService;
        }
        public async Task<Result<UpdateCreateSingleTreatmentDto>> Handle(AddTreatmentCommand request, CancellationToken cancellationToken)
        {
            var treatment = _mapper.Map<Treatment>(request);
            treatment.Slug = _slugService.GenerateSlug(request.Name, treatment.Id);
            _treatmentWriteRepository.AddAsync(treatment);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            var treatmentDto = new UpdateCreateSingleTreatmentDto
            {
                Id = treatment.Id,
                Slug = treatment.Slug,
                Name = treatment.Name,
                Description = treatment.Description,
                ImageUrl = treatment.ImageUrl
            };
            return Result<UpdateCreateSingleTreatmentDto>.Success(treatmentDto, "Added Successfully");
        }
    }
}


