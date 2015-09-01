using System.Collections.Generic;
using Newtonsoft.Json;

namespace Locafi.Client.Odata
{
    public class ODataCollection<TEntity>
    {
        [JsonProperty(PropertyName = "@odata.context")]
        public string Context { get; set; }

        [JsonProperty(PropertyName = "value")]
        public List<TEntity> Value { get; set; }
    }
}
