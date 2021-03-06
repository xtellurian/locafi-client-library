﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Http;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.TagReservation;
using Locafi.Client.Model.RelativeUri;
using Locafi.Client.Model.Responses;

namespace Locafi.Client.Repo
{
    public class TagReservationRepo : WebRepo, ITagReservationRepo
    {
        public TagReservationRepo(IAuthorisedHttpTransferConfigService authorisedConfigService, ISerialiserService serialiser)
            : base(new SimpleHttpTransferer(), authorisedConfigService, serialiser, TagReservationUri.ServiceName)
        {
        }

        public TagReservationRepo(IHttpTransferer transferer, IAuthorisedHttpTransferConfigService authorisedConfigService, ISerialiserService serialiser)
           : base(transferer, authorisedConfigService, serialiser, TagReservationUri.ServiceName)
        {
        }

        public async Task<TagReservationDto> ReserveTagsForSku(Guid skuId, int quantity)
        {
            var path = TagReservationUri.ReserveBySku(skuId, quantity);
            var result = await Get<TagReservationDto>(path);
            return result;
        }

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode, string url, string payload)
        {
            throw new TagReservationRepoException(serverMessages, statusCode, url, payload);
        }

        public override async Task Handle(HttpResponseMessage response, string url, string payload)
        {
            throw new TagReservationRepoException($"{url} -- {payload} -- " + await response.Content.ReadAsStringAsync());
        }
    }
}
