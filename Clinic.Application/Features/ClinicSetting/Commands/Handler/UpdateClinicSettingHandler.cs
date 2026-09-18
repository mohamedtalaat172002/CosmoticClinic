using AutoMapper;
using Clinic.Application.Abstractions.Persistence;
using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.ClinicSetting.Commands.Models;
using Clinic.Application.Features.ClinicSetting.Queries.Result;
using Clinic.Application.Interfaces.Repositories.IClinicSettingRepo;
using MediatR;

namespace Clinic.Application.Features.ClinicSetting.Commands.Handler
{
    public class UpdateClinicSettingHandler : IRequestHandler<UpdateClinicSettingModel, Result<ClinicSettingDto>>
    {
        private readonly IMapper _mapper;
        private readonly IClinicSettingReadRepository _clinicSettingReadRepository;
        private readonly IClinicSettingWriteRepository _clinicSettingWriteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateClinicSettingHandler(IMapper mapper, IClinicSettingReadRepository clinicSettingReadRepository, IClinicSettingWriteRepository clinicSettingWriteRepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _clinicSettingReadRepository = clinicSettingReadRepository;
            _clinicSettingWriteRepository = clinicSettingWriteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ClinicSettingDto>> Handle(
            UpdateClinicSettingModel request,
            CancellationToken cancellationToken)
        {
            var clinicSetting = await _clinicSettingReadRepository.GetById(
                Guid.Parse("11111111-1111-1111-1111-111111111111"));

            if (clinicSetting is null)
            {
                return Result<ClinicSettingDto>.Failure(
                    "Clinic settings not found.",
                    ErrorType.NotFound);
            }

            _mapper.Map(request, clinicSetting);

            _clinicSettingWriteRepository.Update(clinicSetting);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var clinicSettingDto = _mapper.Map<ClinicSettingDto>(clinicSetting);

            return Result<ClinicSettingDto>.Success(
                clinicSettingDto,
                "Clinic settings updated successfully.");
        }
    }
}
