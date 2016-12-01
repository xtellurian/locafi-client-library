using System;

namespace Locafi.Client.Model.Dto.OutboundIntegrations
{
    public class ItemMovementDto
    {
        public ItemIntegrationDto Item { get; set; }

        public PlaceIntegrationDto OldPlace { get; set; }

        public PlaceIntegrationDto NewPlace { get; set; }

        public PersonIntegrationDto MovedBy { get; set; }

        public string ReasonId { get; set; }

        public DateTime MovedOn { get; set; }

    }
}
