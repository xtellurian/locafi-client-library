using System;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.ErrorLogs;
using Locafi.Client.Model.Enums;

namespace Locafi.Client.Contract.Repo
{
    public interface IErrorRepo
    {
        Task<ErrorLogDetailDto> LogException (Exception ex, ErrorLevel level = ErrorLevel.Minor);
    }
}