using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model
{
    /// <summary>
    /// Represents a feed of entities that includes additional information that OData formats support.
    /// </summary>
    [DataContract]
    public class PageResult
    {
        private long? _count;

        /// <summary>
        /// Gets the link for the next page of items in the feed.
        /// </summary>
        [DataMember]
        public System.Uri NextPageLink { get; private set; }

        /// <summary>
        /// Gets the total count of items in the feed.
        /// </summary>
        [DataMember]
        public long? Count
        {
            get
            {
                return this._count;
            }
            private set
            {
                if (value.HasValue && value.Value < 0L)
                    throw new ArgumentOutOfRangeException("value", (object)value.Value, "ArgumentMustBeGreaterThanOrEqualTo: 0");
                this._count = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Web.OData.PageResult"/> class.
        /// </summary>
        /// <param name="nextPageLink">The link for the next page of items in the feed.</param><param name="count">The total count of items in the feed.</param>
        protected PageResult(System.Uri nextPageLink, long? count)
        {
            this.NextPageLink = nextPageLink;
            this.Count = count;
        }

        public PageResult() { }
    }

    /// <summary>
    /// Represents a feed of entities that includes additional information that OData formats support.
    /// </summary>
    /// <typeparam name="T"/>
    [JsonObject]
    [DataContract]
    public class PageResult<T> : PageResult, IEnumerable<T>, IEnumerable
    {
        /// <summary>
        /// Gets the collection of entities for this feed.
        /// </summary>
        [DataMember]
        public IEnumerable<T> Items { get; private set; }

        public PageResult() { }

        /// <summary>
        /// Creates a partial set of results - used when server driven paging is enabled.
        /// </summary>
        /// <param name="items">The subset of matching results that should be serialized in this page.</param><param name="nextPageLink">A link to the next page of matching results (if more exists).</param><param name="count">A total count of matching results so clients can know the number of matches on the server.</param>
        public PageResult(IEnumerable<T> items, System.Uri nextPageLink, long? count)
          : base(nextPageLink, count)
        {
            if (items == null)
                throw new ArgumentNullException("data");
            this.Items = items;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// 
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this.Items.GetEnumerator();
        }
    }
}
