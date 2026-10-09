using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.ClinicSetting.Queries.Models;
using Clinic.Application.Features.ClinicSetting.Queries.Result;
using Clinic.Application.Interfaces.Repositories.IClinicSettingRepo;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Application.Features.ClinicSetting.Queries.Handler
{
    public class GetClinicSettingsQueryHandler
        : IRequestHandler<GetClinicSettingsQueryModel, Result<ClinicSettingDto>>
    {
        private static readonly Guid ClinicSettingsId =
            Guid.Parse("11111111-1111-1111-1111-111111111111");

        private readonly IClinicSettingReadRepository _clinicSettingReadRepository;

        public GetClinicSettingsQueryHandler(IClinicSettingReadRepository clinicSettingReadRepository)
        {
            _clinicSettingReadRepository = clinicSettingReadRepository;
        }

        public async Task<Result<ClinicSettingDto>> Handle(
            GetClinicSettingsQueryModel request,
            CancellationToken cancellationToken)
        {
            // Direct projection: loads the images too (the old GetById + AutoMapper
            // never included them) and fetches only the columns the DTO needs.
            var clinicSetting = await _clinicSettingReadRepository
                .GetAll()
                .Where(c => c.Id == ClinicSettingsId)
                .Select(c => new ClinicSettingDto
                {
                    Id = c.Id,
                    ClinicName = c.ClinicName,
                    DoctorName = c.DoctorName,
                    Specialization = c.Specialization,
                    Description = c.Description,
                    WhatsAppNumber = c.WhatsAppNumber,
                    Email = c.Email,
                    Address = c.Address,
                    FacebookUrl = c.FacebookUrl,
                    InstagramUrl = c.InstagramUrl,
                    HeroImageUrl = c.Images
                        .Where(i => i.IsPrimary)
                        .Select(i => i.Url)
                        .FirstOrDefault(),
                    Images = c.Images
                        .OrderBy(i => i.DisplayOrder)
                        .Select(i => new ClinicSettingImageDto
                        {
                            Id = i.Id,
                            Url = i.Url,
                            IsPrimary = i.IsPrimary,
                            DisplayOrder = i.DisplayOrder
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (clinicSetting is null)
            {
                return Result<ClinicSettingDto>.Failure(
                    "Clinic setting not found",
                    ErrorType.NotFound);
            }

            return Result<ClinicSettingDto>.Success(clinicSetting);
        }
    }
}