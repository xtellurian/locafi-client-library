using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Authentication
{
    public interface ILoginCredentialsProvider
    {
        string UserName { get; }
        string Password { get; }
    }
}
