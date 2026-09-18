using AutoMapper;
using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.ClinicSetting.Queries.Models;
using Clinic.Application.Features.ClinicSetting.Queries.Result;
using Clinic.Application.Interfaces.Repositories.IClinicSettingRepo;
using MediatR;

namespace Clinic.Application.Features.ClinicSetting.Queries.Handler
{
    public class GetClinicSettingsQueryHandler : IRequestHandler<GetClinicSettingsQueryModel, Result<ClinicSettingDto>>
    {
        private readonly IClinicSettingReadRepository _clinicSettingReadRepository;
        private readonly IMapper _mapper;
        public GetClinicSettingsQueryHandler(IClinicSettingReadRepository clinicSettingReadRepository, IMapper mapper)
        {
            _clinicSettingReadRepository = clinicSettingReadRepository;
            _mapper = mapper;
        }

        public async Task<Result<ClinicSettingDto>> Handle(GetClinicSettingsQueryModel request, CancellationToken cancellationToken)
        {
            var clinicSetting = await _clinicSettingReadRepository.GetById(Guid.Parse("11111111-1111-1111-1111-111111111111"));
            if (clinicSetting is null)
            {
                return Result<ClinicSettingDto>.Failure("Clinic setting not found", ErrorType.NotFound);
            }
            var clinicSettingDto = _mapper.Map<ClinicSettingDto>(clinicSetting);
            return Result<ClinicSettingDto>.Success(clinicSettingDto);

        }
    }
}
