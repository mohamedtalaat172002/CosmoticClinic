using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.ClinicSetting.Queries.Result;
using MediatR;

namespace Clinic.Application.Features.ClinicSetting.Commands.Models
{
    public sealed record UpdateClinicSettingModel(
        string? ClinicName,
        string DoctorName,
        string? Specialization,
        string? Description,
        string? WhatsAppNumber,
        string? Email,
        string Address,
        string? FacebookUrl,
        string? InstagramUrl,
        IReadOnlyCollection<ClinicSettingImageUpload>? Images
    ) : IRequest<Result<UpdateCreateClinicSettingDto>>;

    public sealed record ClinicSettingImageUpload(
        Stream FileStream,
        string FileName
    );
}
