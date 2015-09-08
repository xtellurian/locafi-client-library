using System;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.TagReservation;

namespace Locafi.Client.Contract.Repo
{
    public interface ITagReservationRepo
    {
        Task<TagReservationDto> ReserveTagsForSku(Guid skuId, int quantity);
    }
}