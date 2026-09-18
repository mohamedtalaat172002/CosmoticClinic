using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.ClinicSetting.Queries.Result;
using MediatR;

namespace Clinic.Application.Features.ClinicSetting.Queries.Models
{
    public class GetClinicSettingsQueryModel : IRequest<Result<ClinicSettingDto>>
    {
    }
}
