using System.Collections.Generic;
using System.Security.Claims;

namespace BohFoundation.Domain.Dtos.UserManagement
{
    public class SuccessOrFailureDtoWithClaims : SuccessOrFailureDto
    {
        public List<Claim> Claims { get; set; }
    }
}
