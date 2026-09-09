namespace Clinic.Application.Interfaces.Services
{
    public interface ISlugService
    {
        string GenerateSlug(string text, Guid id);

    }
}
