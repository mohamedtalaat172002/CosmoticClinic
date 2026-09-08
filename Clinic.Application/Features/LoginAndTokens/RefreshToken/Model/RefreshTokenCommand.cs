using Clinic.Application.Common.ResponseFormat;
using Clinic.Domain.Entities.Identity.AuthenticationHepler;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Features.AuthenticationFeatures.LoginAndTokens.RefreshToken.Model
{
    public class RefreshTokenCommand : IRequest<Result<ResponseAuthModel>>
    {

    }
}
