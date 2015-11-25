using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Query.Builder
{
    public class UriQuery<T> : IRestQuery<T>
    {
        private readonly string _queryExpression;

        public UriQuery(string queryExpression, int take, int skip)
        {
            Take = take;
            Skip = skip;
            _queryExpression = queryExpression;
        }

        public string AsRestQuery()
        {
            return _queryExpression;
        }

        public int Take { get; set; }
        public int Skip { get; set; }

        public static IRestQuery<T> NoFilter(int skip, int take)
        {
            return new UriQuery<T>("",take,skip );
        } 
    }
}
