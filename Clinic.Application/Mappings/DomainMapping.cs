using AutoMapper;
using BackEnd.Domain.Entities;
using Clinic.Application.Features.Products.Dto;

namespace Clinic.Application.Mappings
{
    public class DomainMapping : Profile
    {
        public DomainMapping()
        {
            CreateMap<Product, ReturnProductDto>();
        }
    }
}
