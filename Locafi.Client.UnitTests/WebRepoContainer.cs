﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Authentication;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Http;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Repo;
using Locafi.Client.UnitTests.Factory;
using Locafi.Client.UnitTests.Implementations;
using Locafi.Client.Model.Query.Builder;
using Locafi.Client.Model.Dto.Places;
using Locafi.Builder;
using Locafi.Client.Model.Query;
using Locafi.Client.Model.Dto;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Locafi.Client.Model.Dto.Skus;
using Locafi.Client.Model.Dto.Persons;
using Locafi.Client.Model.Dto.Items;

namespace Locafi.Client.UnitTests
{
    public static class WebRepoContainer
    {
        private static readonly ISerialiserService Serialiser;
        //private static IAuthorisedHttpTransferConfigService AuthorisedHttpTransferConfigService 
        //    => HttpConfigFactory.Generate(StringConstants.BaseUrl, StringConstants.TestingEmailAddress, StringConstants.Password).Result;
        public static IAuthorisedHttpTransferConfigService AuthorisedHttpTransferConfigService
            = HttpConfigFactory.Generate(StringConstants.BaseUrl, DevEnvironment.TestUserEmail, DevEnvironment.TestUserPassword).Result;
        private static readonly IHttpTransferConfigService HttpConfigService;


        public static IItemRepo ItemRepo => new ItemRepo(AuthorisedHttpTransferConfigService, Serialiser);
        
        public static IInventoryRepo InventoryRepo => new InventoryRepo(AuthorisedHttpTransferConfigService, Serialiser);
        public static IOrderRepo OrderRepo => new OrderRepo(AuthorisedHttpTransferConfigService,Serialiser);
        public static IPlaceRepo PlaceRepo => new PlaceRepo(AuthorisedHttpTransferConfigService, Serialiser);
        public static IPersonRepo PersonRepo => new PersonRepo(AuthorisedHttpTransferConfigService, Serialiser);
        public static IReasonRepo ReasonRepo => new ReasonRepo(AuthorisedHttpTransferConfigService,Serialiser);
        public static ISnapshotRepo SnapshotRepo => new SnapshotRepo(AuthorisedHttpTransferConfigService, Serialiser);
        public static ISkuRepo SkuRepo => new SkuRepo(AuthorisedHttpTransferConfigService, Serialiser);
        public static ISkuGroupRepo SkuGroupRepo => new SkuGroupRepo(AuthorisedHttpTransferConfigService, Serialiser );
        public static ITagReservationRepo TagReservationRepo => new TagReservationRepo(AuthorisedHttpTransferConfigService, Serialiser);
        public static ITemplateRepo TemplateRepo => new TemplateRepo(AuthorisedHttpTransferConfigService,Serialiser);
        public static IUserRepo UserRepo => new UserRepo(AuthorisedHttpTransferConfigService, Serialiser);
        public static IAuthenticationRepo AuthRepo => new AuthenticationRepo(HttpConfigService, Serialiser);
        public static IPortalRepo PortalRepo => new PortalRepo(AuthorisedHttpTransferConfigService, Serialiser);
        public static IErrorRepo ErrorRepo => new ErrorRepo(new SimpleHttpTransferer(), AuthorisedHttpTransferConfigService, new Serialiser());
        public static ICycleCountRepo CycleCountRepo => new CycleCountRepo(AuthorisedHttpTransferConfigService, Serialiser);
        public static IExtendedPropertyRepo ExtendedPropertyRepo => new ExtendedPropertyRepo(AuthorisedHttpTransferConfigService, Serialiser);
        public static IRoleRepo RoleRepo => new RoleRepo(AuthorisedHttpTransferConfigService, Serialiser);

        static WebRepoContainer()
        {
            Serialiser = new Serialiser();
            //AuthorisedHttpTransferConfigService = HttpConfigFactory.Generate(StringConstants.BaseUrl, UserName, Password).Result;
            HttpConfigService = new UnauthorisedHttpTransferConfigService();
        }

        private static Guid _place1Id;
        public static Guid Place1Id
        {
            get {
                if(_place1Id == null || _place1Id == Guid.Empty)
                {
                    _place1Id = PlaceRepo.QueryPlaces(QueryBuilder<PlaceSummaryDto>.NewQuery(p => p.Name, DevEnvironment.Place1Name, ComparisonOperator.Equals).Build()).Result.First().Id;
                }

                return _place1Id;
            }
        }

        private static Guid _place2Id;
        public static Guid Place2Id
        {
            get
            {
                if (_place2Id == null || _place2Id == Guid.Empty)
                {
                    _place2Id = PlaceRepo.QueryPlaces(QueryBuilder<PlaceSummaryDto>.NewQuery(p => p.Name, DevEnvironment.Place2Name, ComparisonOperator.Equals).Build()).Result.First().Id;
                }

                return _place2Id;
            }
        }

        private static Guid _sku1Id;
        public static Guid Sku1Id
        {
            get
            {
                if (_sku1Id == null || _sku1Id == Guid.Empty)
                {
                    _sku1Id = SkuRepo.QuerySkus(QueryBuilder<SkuSummaryDto>.NewQuery(e => e.Name, DevEnvironment.SkuCategory1Name, ComparisonOperator.Equals).Build()).Result.First().Id;
                }

                return _sku1Id;
            }
        }

        private static Guid _sku2Id;
        public static Guid Sku2Id
        {
            get
            {
                if (_sku2Id == null || _sku2Id == Guid.Empty)
                {
                    _sku2Id = SkuRepo.QuerySkus(QueryBuilder<SkuSummaryDto>.NewQuery(e => e.Name, DevEnvironment.SkuCategory2Name, ComparisonOperator.Equals).Build()).Result.First().Id;
                }

                return _sku2Id;
            }
        }

        private static Guid _assetCategory1Id;
        public static Guid AssetCategory1Id
        {
            get
            {
                if (_assetCategory1Id == null || _assetCategory1Id == Guid.Empty)
                {
                    _assetCategory1Id = SkuRepo.QuerySkus(QueryBuilder<SkuSummaryDto>.NewQuery(e => e.Name, DevEnvironment.AssetCategory1Name, ComparisonOperator.Equals).Build()).Result.First().Id;
                }

                return _assetCategory1Id;
            }
        }

        private static Guid _assetCategory2Id;
        public static Guid AssetCategory2Id
        {
            get
            {
                if (_assetCategory2Id == null || _assetCategory2Id == Guid.Empty)
                {
                    _assetCategory2Id = SkuRepo.QuerySkus(QueryBuilder<SkuSummaryDto>.NewQuery(e => e.Name, DevEnvironment.AssetCategory2Name, ComparisonOperator.Equals).Build()).Result.First().Id;
                }

                return _assetCategory2Id;
            }
        }

        private static Guid _person1Id;
        public static Guid Person1Id
        {
            get
            {
                if (_person1Id == null || _person1Id == Guid.Empty)
                {
                    _person1Id = PersonRepo.QueryPersons(QueryBuilder<PersonSummaryDto>.NewQuery(e => e.Email, DevEnvironment.Person1Email, ComparisonOperator.Equals).Build()).Result.First().Id;
                }

                return _person1Id;
            }
        }

        private static Guid _person2Id;
        public static Guid Person2Id
        {
            get
            {
                if (_person2Id == null || _person2Id == Guid.Empty)
                {
                    _person2Id = PersonRepo.QueryPersons(QueryBuilder<PersonSummaryDto>.NewQuery(e => e.Email, DevEnvironment.Person2Email, ComparisonOperator.Equals).Build()).Result.First().Id;
                }

                return _person2Id;
            }
        }

        private static Guid _asset1Id;
        public static Guid Asset1Id
        {
            get
            {
                if (_asset1Id == null || _asset1Id == Guid.Empty)
                {
                    _asset1Id = ItemRepo.QueryItems(QueryBuilder<ItemSummaryDto>.NewQuery(e => e.TagNumber, DevEnvironment.Asset1TagNumber, ComparisonOperator.Equals).Build()).Result.First().Id;
                }

                return _asset1Id;
            }
        }

        private static Guid _asset2Id;
        public static Guid Asset2Id
        {
            get
            {
                if (_asset2Id == null || _asset2Id == Guid.Empty)
                {
                    _asset2Id = ItemRepo.QueryItems(QueryBuilder<ItemSummaryDto>.NewQuery(e => e.TagNumber, DevEnvironment.Asset2TagNumber, ComparisonOperator.Equals).Build()).Result.First().Id;
                }

                return _asset2Id;
            }
        }

        private static Guid _asset3Id;
        public static Guid Asset3Id
        {
            get
            {
                if (_asset3Id == null || _asset3Id == Guid.Empty)
                {
                    _asset3Id = ItemRepo.QueryItems(QueryBuilder<ItemSummaryDto>.NewQuery(e => e.TagNumber, DevEnvironment.Asset3TagNumber, ComparisonOperator.Equals).Build()).Result.First().Id;
                }

                return _asset3Id;
            }
        }

        private static Guid _asset4Id;
        public static Guid Asset4Id
        {
            get
            {
                if (_asset4Id == null || _asset4Id == Guid.Empty)
                {
                    _asset4Id = ItemRepo.QueryItems(QueryBuilder<ItemSummaryDto>.NewQuery(e => e.TagNumber, DevEnvironment.Asset4TagNumber, ComparisonOperator.Equals).Build()).Result.First().Id;
                }

                return _asset4Id;
            }
        }
    }
}
