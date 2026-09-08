using Clinic.Application.Common.ResponseFormat;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Features.AuthenticationFeatures.Password.RestPassword.Command
{
    public class SendTokenToRestPasswordCommand : IRequest<Result<ResetPasswordTokenResponse>>
    {
        public string Email { get; set; }
    }
    public class ResetPasswordTokenResponse
    {
        public string Token { get; set; }
        public string UserId { get; set; }
    }

}
