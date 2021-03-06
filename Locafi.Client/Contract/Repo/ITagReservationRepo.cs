﻿// This is a contract file
// Breaking changes to this contract should be accompanied by a version change in the third version number
// ie ... 1.2.42.11 -> 1.2.43.0

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