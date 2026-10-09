using Clinic.Application.Features.Articles.Queries.Result;
using Clinic.Application.Features.ClinicSetting.Queries.Result;
using Clinic.Application.Features.Treatments.Queries.Result;
using System.ComponentModel.DataAnnotations;

namespace Clinic.web.Models;

public sealed class HomeViewModel
{
    public ClinicSettingDto? ClinicSettings { get; init; }
    public IReadOnlyList<ClinicSettingImageDto> HeroImages { get; init; } = [];
    public string? HeroImageUrl => HeroImages.FirstOrDefault(x => x.IsPrimary)?.Url
                                ?? HeroImages.FirstOrDefault()?.Url
                                ?? ClinicSettings?.HeroImageUrl;

    // List cards: each item carries only its primary image (AllTreatmentsDto.ImageUrl).
    public IReadOnlyList<AllTreatmentsDto> Treatments { get; init; } = [];

    // List cards: each item carries only its primary image (AllArticlesDto.FeaturedImageUrl).
    public IReadOnlyList<AllArticlesDto> Articles { get; init; } = [];
}

public sealed class ConsultationInputModel
{
    [Required, Display(Name = "Full name")]
    public string FullName { get; set; } = string.Empty;

    [Required, Phone, Display(Name = "Phone number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Message { get; set; }
}

public sealed class LoginInputModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }
}

public sealed class ContactInputModel
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(1000)]
    public string Message { get; set; } = string.Empty;
}

public sealed class TreatmentsIndexViewModel
{
    //public IReadOnlyList<TreatmentSummaryDto> Treatments { get; init; } = [];
    public int PageIndex { get; init; }
    public int PageSize { get; init; }
    public int TotalRows { get; init; }
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalRows / PageSize) : 0;
    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;
}