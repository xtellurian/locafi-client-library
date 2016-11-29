using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Crypto;
using Locafi.Client.Contract.Repo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests.Client.Core
{
    [TestClass]
    public class AgentRepoTests
    {
        private IAuthenticationRepo _authRepo;        

        [TestInitialize]
        public void Initialize()
        {
            _authRepo = WebRepoContainer.AuthRepo;            
        }
    }
}
