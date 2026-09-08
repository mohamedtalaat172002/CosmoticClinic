using Clinic.Application.Common.ResponseFormat;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Features.AuthenticationFeatures.LoginAndTokens.Logout.Command
{
    public class UserLogoutCommand : IRequest<Result<bool>>
    {

    }
}
