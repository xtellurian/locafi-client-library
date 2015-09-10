using System;
using Locafi.Client.Contract.Repo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Locafi.Client.UnitTests.Tests
{
    [TestClass]
    public class OrderRepoProcessTests
    {
        private IPlaceRepo _placeRepo;
        private IPersonRepo _personRepo;
        private IOrderRepo _orderRepo;
        private IUserRepo _userRepo;

        [TestInitialize]
        public void Initialize()
        {
            _placeRepo = WebRepoContainer.PlaceRepo;
            _personRepo = WebRepoContainer.PersonRepo;
            _orderRepo = WebRepoContainer.OrderRepo;
            _userRepo = WebRepoContainer.UserRepo;
        }

        [TestMethod]
        public void OrderProcess_AllocateSuccess()
        {

        }
    }
}
