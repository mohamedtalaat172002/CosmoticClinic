using Clinic.Application.Features.Articles.Queries.Result;
using Clinic.Application.Features.ClinicSetting.Queries.Result;
using Clinic.Application.Features.Treatments.Queries.Result;
using Clinic.Domain.Entities.artical;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Clinic.web.Models;

public sealed class TreatmentUpsertInputModel
{
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;
    [MaxLength(3000)]
    public string Description { get; set; } = string.Empty;

    // New uploads. When provided on edit, they replace ALL existing images.
    public List<IFormFile> Images { get; set; } = [];

    // Display only (filled by the controller). Never bound from the request.
    [BindNever]
    public List<TreatmentImageDto> ExistingImages { get; set; } = [];
}

public sealed class ArticleUpsertInputModel
{
    public Guid Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    public string Summary { get; set; } = string.Empty;

    [Required]
    [MaxLength(3000)]
    public string Content { get; set; } = string.Empty;

    public ArticleStatus Status { get; set; } = ArticleStatus.Draft;
    public Guid? TreatmentId { get; set; }

    // New uploads. When provided on edit, they replace ALL existing images.
    public List<IFormFile> Images { get; set; } = [];

    // Display only (filled by the controller). Never bound from the request.
    [BindNever]
    public List<ArticleImageDto> ExistingImages { get; set; } = [];
}

public sealed class ClinicSettingsInputModel
{

    [StringLength(150)]
    public string? ClinicName { get; set; }

    [Required, StringLength(150)]
    public string DoctorName { get; set; } = string.Empty;

    [StringLength(150)]
    public string? Specialization { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }

    [StringLength(30)]
    public string? WhatsAppNumber { get; set; }

    [EmailAddress, StringLength(255)]
    public string? Email { get; set; }

    [Required, StringLength(600)]
    public string Address { get; set; } = string.Empty;

    [Url, StringLength(500)]
    public string? FacebookUrl { get; set; }

    [Url, StringLength(500)]
    public string? InstagramUrl { get; set; }

    public List<IFormFile> Images { get; set; } = [];

    [BindNever]
    public List<ClinicSettingImageDto> ExistingImages { get; set; } = [];
}