using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Processors;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Exceptions;
using Locafi.Client.Model.Dto.Items;
using Locafi.Client.Model.Dto.Orders;
using Locafi.Client.Model.Dto.Snapshots;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Query.PropertyComparison;
using Locafi.Client.Processors.Orders.Strategies;

namespace Locafi.Client.Processors.Orders
{
    public class OrderProcessor
    {
        public OrderDetailDto OrderDetail { get; }
        public IList<SnapshotTagDto> Tags { get; }
        public IList<ItemSummaryDto> UnknownItems { get; } 

        private readonly IItemRepo _itemRepo;
        private readonly IProcessSnapshotTagOrderStrategy _strategy;
        private StrategyState _state;

        public OrderProcessor(IItemRepo itemRepo, OrderDetailDto orderDetail, IProcessSnapshotTagOrderStrategy strategy)
        {
            OrderDetail = orderDetail;
            UnknownItems = new List<ItemSummaryDto>();
            _itemRepo = itemRepo;
            _strategy = strategy;
            Tags = new List<SnapshotTagDto>();
        }

        public async Task InitialiseState(ISnapshotRepo snapshotRepo)
        {
            var sourceTags = new List<SnapshotTagDto>();
            var destinationTags = new List<SnapshotTagDto>();
            foreach (var snapshot in OrderDetail.SourceSnapshotIds)
            {
                var detail = await snapshotRepo.GetSnapshot(snapshot);
                sourceTags.AddRange(detail.Tags);
            }
            foreach (var snapshot in OrderDetail.DestinationSnapshotIds)
            {
                var detail = await snapshotRepo.GetSnapshot(snapshot);
                destinationTags.AddRange(detail.Tags);
            }
            _state = new InitStrategyState(sourceTags, destinationTags);
        }

        public virtual async Task AddSnapshotTag(SnapshotTagDto snapshotTag)
        {
            if(_state==null) throw new NullReferenceException("State was not initialised");
            var result = _strategy.ProcessTag(snapshotTag, OrderDetail, _state);
            _state = result.State;
            if (result.IsSuccessful)
            {
                Tags.Add(snapshotTag);
            }
            else
            {
                switch (result.ResultCategory)
                {
                    case ProcessSnapshotTagResultCategory.LineOverAllocated:

                        break;
                    case ProcessSnapshotTagResultCategory.UnknownTag:
                        await OnUnknownTag(snapshotTag);
                        break;
                }
            }
        }

        private async Task OnUnknownTag(SnapshotTagDto snapshotTag)
        {
            var query = ItemQuery.NewQuery(i => i.TagNumber, snapshotTag.TagNumber,
                ComparisonOperator.Equals);
            var results = await _itemRepo.QueryItems(query);
            foreach (var item in results)
            {
                UnknownItems.Add(item);
            }
        }
    }
}
