﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Locafi.Client.Data;

namespace Locafi.Client.Contract.Services
{
    public interface IUserRepo
    {
        Task<IList<UserDto>> GetAllUsers();
    }
}