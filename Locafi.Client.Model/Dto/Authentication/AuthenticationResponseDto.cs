using System.Collections.Generic;

namespace Locafi.Client.Model.Dto.Authentication
{
    public class AuthenticationResponseDto
    {

        public bool Success { get; set; }
        public TokenGroup TokenGroup { get; set; }
        public IList<string> Messages { get; set; }

    }

    public class TokenGroup
    {
        public string Token { get; set; }
        public string Refresh { get; set; }
    }
}
