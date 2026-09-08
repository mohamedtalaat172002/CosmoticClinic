using AutoMapper;
using Clinic.Application.Common;
using Clinic.Application.Common.ResponseFormat;
using Clinic.Application.Features.RegisterUser.Commands.Models;
using Clinic.Application.Features.RegisterUser.Commands.Dtos;
using Clinic.Application.Interfaces.Services;
using Clinic.Domain.Entities.Identity;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Features.RegisterUser.Commands.Handler
{
    public class DeafRegisterHandler : IRequestHandler<AdminRegisterCommand, Result<RegisterUserDto>>
    {
        private readonly IIdentityService _identityServies;
        private readonly IMapper _mapper;

        public DeafRegisterHandler(
            IMapper mapper,
            IIdentityService identityServies)
        {
            _mapper = mapper;
            _identityServies = identityServies;
        }

        public async Task<Result<RegisterUserDto>> Handle(AdminRegisterCommand request, CancellationToken cancellationToken)
        {

            var emailExists = await _identityServies.IsEmailExist(request.Email, cancellationToken);

            if (emailExists.IsSuccess && emailExists.Value)
            {
                return Result<RegisterUserDto>.Failure("Email already registered.", ErrorType.Conflict);
            }

            var newUser = _mapper.Map<ApplicationUser>(request);
            if (newUser == null)
            {
                return Result<RegisterUserDto>.Failure("User mapping failed.", ErrorType.InternalServerError);
            }

            var userResult = await _identityServies.CreateUserAsync(newUser, request.Password, ApplicationRoles.Admin, cancellationToken);
            if (!userResult.IsSuccess)
                return Result<RegisterUserDto>.Failure(userResult.Message, userResult.ErrorType);

            var token = await _identityServies.CreateJwtToken(newUser, cancellationToken);

            var response = new RegisterUserDto
            {
                UserId = newUser.Id,
                Email = newUser.Email,
                Token = token.Value,
                Role = ApplicationRoles.Admin
            };

            return Result<RegisterUserDto>.Success(response);
        }
    }
}
