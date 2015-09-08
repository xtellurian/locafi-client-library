using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.TagReservation;
using Locafi.Client.Model.RelativeUri;
using Locafi.Client.Model.Responses;

namespace Locafi.Client.Repo
{
    public class TagReservationRepo : WebRepo, ITagReservationRepo
    {
        public TagReservationRepo(IAuthorisedHttpTransferConfigService authorisedUnauthorizedConfigService, ISerialiserService serialiser)
            : base(authorisedUnauthorizedConfigService, serialiser, TagReservationUri.SerivceName)
        {
        }

        public async Task<TagReservationDto> ReserveTagsForSku(Guid skuId, int quantity)
        {
            var path = TagReservationUri.ReserveBySku(skuId, quantity);
            var result = await Get<TagReservationDto>(path);
            return result;
        }

        public async Task<TagReservationDto> ReserveTagsForGtin(string gtin, int quantity)
        {
            var path = TagReservationUri.ReserveByGtin(gtin, quantity);
            var result = await Get<TagReservationDto>(path);
            return result;
        }

        public override Task Handle(IEnumerable<CustomResponseMessage> serverMessages, HttpStatusCode statusCode)
        {
            throw new TagReservationRepoException(serverMessages, statusCode);
        }

        public override Task Handle(HttpResponseMessage response)
        {
            throw new TagReservationRepoException(null,response.StatusCode);
        }
    }
}
