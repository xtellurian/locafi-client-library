using Locafi.Builder.Model;
using Locafi.Builder.Model.Items;
using Locafi.Builder.Model.Persons;
using Locafi.Builder.Model.Places;
using Locafi.Builder.Model.Skus;
using Locafi.Builder.Model.Templates;
using Locafi.Builder.Model.Users;
using Locafi.Client.Model.Dto.ExtendedProperties;
using Locafi.Client.Model.Dto.Places;
using Locafi.Client.Model.Dto.Skus;
using Locafi.Client.Model.Dto.Tags;
using Locafi.Client.Model.Dto.Templates;
using Locafi.Client.Model.Dto.Users;
using Locafi.Client.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Builder
{
    public class DevEnvironment1
    {
        // Register details / 
        public static string OrganisationName => "Locafi_Client_Test_Organisation";
        public static string UserFirstName => "Locafi";
        public static string UserLastName => "Client";
        public static string RegUserEmail => "locaficlient@ramp.comm.au";
        public static string RegUserPassword => "tester123";

        // Extended Properties to add
        public static string ExpiryDateExtPropName = "ExpiryDate";
        public static string ServiceDateExtPropName = "ServiceDate";
        public static List<AddExtendedPropertyDto> ExtendedPropertiesToCreate = new List<AddExtendedPropertyDto>()
        {
            new AddExtendedPropertyDto()
            {
                Name = ExpiryDateExtPropName,
                Description = ExpiryDateExtPropName,
                DataType = TemplateDataTypes.DateTime,
                TemplateType = TemplateFor.Item
            },
            new AddExtendedPropertyDto()
            {
                Name = ServiceDateExtPropName,
                Description = ServiceDateExtPropName,
                DataType = TemplateDataTypes.DateTime,
                TemplateType = TemplateFor.Item
            }
        };

        // Templates to add
        public static string SkuTemplateName = "Sku Item";
        public static string AssetTemplateName = "Asset Item";
        public static string PlaceTemplateName = "Place";
        public static string PersonTemplateName = "Person Template";
        public static List<BuilderAddTemplateDto> TemplatesToCreate = new List<BuilderAddTemplateDto>()
        {
            new BuilderAddTemplateDto()
            {
                Name = SkuTemplateName,
                TemplateType = TemplateFor.Item,
                BuilderTemplateExtendedPropertyList = new List<BuilderAddTemplateExtendedPropertyDto>()
                {
                    new BuilderAddTemplateExtendedPropertyDto()
                    {
                        ExtendedPropertyName = ExpiryDateExtPropName,
                        IsRequired = false
                    }
                }
            },
            new BuilderAddTemplateDto()
            {
                Name = AssetTemplateName,
                TemplateType = TemplateFor.Item,
                BuilderTemplateExtendedPropertyList = new List<BuilderAddTemplateExtendedPropertyDto>()
                {
                    new BuilderAddTemplateExtendedPropertyDto()
                    {
                        ExtendedPropertyName = ServiceDateExtPropName,
                        IsRequired = false
                    }
                }
            },
            new BuilderAddTemplateDto()
            {
                Name = PlaceTemplateName,
                TemplateType = TemplateFor.Place,
                BuilderTemplateExtendedPropertyList = new List<BuilderAddTemplateExtendedPropertyDto>()
                {
                    
                }
            }
        };

        // Categories to Create
        public static string SkuCategory1Name = "Milk";
        public static string SkuCategory2Name = "Bread";
        public static string AssetCategory1Name = "Tools";
        public static string AssetCategory2Name = "IT Assets";
        public static List<BuilderAddSkuDto> SkusToCreate = new List<BuilderAddSkuDto>()
        {
            new BuilderAddSkuDto()
            {
                Name = SkuCategory1Name,
                SkuNumber = "",
                CompanyPrefix = "2000000",
                ItemReference = "000004",
                CustomPrefix = "",
                IsSgtin = true,
                TemplateName = SkuTemplateName,
                BuilderSkuExtendedPropertyList = new List<BuilderWriteSkuExtendedPropertyDto>()
                {
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = ExpiryDateExtPropName,
                        IsSkuLevelProperty = false
                    }
                }
            },
            new BuilderAddSkuDto()
            {
                Name = SkuCategory2Name,
                SkuNumber = "",
                CompanyPrefix = "0000250",
                ItemReference = "880318",
                CustomPrefix = "",
                IsSgtin = true,
                TemplateName = SkuTemplateName,
                BuilderSkuExtendedPropertyList = new List<BuilderWriteSkuExtendedPropertyDto>()
                {
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = ExpiryDateExtPropName,
                        IsSkuLevelProperty = false
                    }
                }
            },
            new BuilderAddSkuDto()
            {
                Name = AssetCategory1Name,
                SkuNumber = "T001",
                CompanyPrefix = "",
                ItemReference = "",
                CustomPrefix = "5860",
                TemplateName = AssetTemplateName,
                BuilderSkuExtendedPropertyList = new List<BuilderWriteSkuExtendedPropertyDto>()
                {
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = ServiceDateExtPropName,
                        IsSkuLevelProperty = false
                    }
                }
            },
            new BuilderAddSkuDto()
            {
                Name = AssetCategory2Name,
                SkuNumber = "IT001",
                CompanyPrefix = "",
                ItemReference = "",
                CustomPrefix = "5960",
                TemplateName = AssetTemplateName,
                BuilderSkuExtendedPropertyList = new List<BuilderWriteSkuExtendedPropertyDto>()
                {
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = ServiceDateExtPropName,
                        IsSkuLevelProperty = false
                    }
                }
            }
        };

        // Roles to create
        public static string AdminRoleName = "Administrator";
        //public static List<BuilderAddRoleDto> RolesToCreate = new List<BuilderAddRoleDto>() { }

        // Users to register
        public static string HHTestUserEmail = "HH@ramp.com.au";
        public static string HHTestUserPassword = "ramp123";
        public static string TestUserEmail = "clienttester@ramp.comm.au";
        public static string TestUserPassword = "tester123";
        public static string DemoUserEmail = "demo@ramp.com.au";
        public static string DemoUserPassword = "demo123";
        public static List<BuilderAddUserDto> UsersToCreate = new List<BuilderAddUserDto>()
        {
            new BuilderAddUserDto()
            {
                GivenName = "HH",
                Surname = "Tester",
                Email = HHTestUserEmail,
                Password = HHTestUserPassword,
                RoleName = AdminRoleName,
                TemplateName = PersonTemplateName,
                PersonTagList = new List<WriteTagDto>()
                {

                },
                BuilderPersonExtendedPropertyList = new List<BuilderWriteEntityExtendedPropertyDto>()
                {

                }
            },
            new BuilderAddUserDto()
            {
                GivenName = "Client",
                Surname = "Tester",
                Email = TestUserEmail,
                Password = TestUserPassword,
                RoleName = AdminRoleName,
                TemplateName = PersonTemplateName,
                PersonTagList = new List<WriteTagDto>()
                {

                },
                BuilderPersonExtendedPropertyList = new List<BuilderWriteEntityExtendedPropertyDto>()
                {

                }
            },
            new BuilderAddUserDto()
            {
                GivenName = "Demo",
                Surname = "User",
                Email = DemoUserEmail,
                Password = DemoUserPassword,
                RoleName = AdminRoleName,
                TemplateName = PersonTemplateName,
                PersonTagList = new List<WriteTagDto>()
                {

                },
                BuilderPersonExtendedPropertyList = new List<BuilderWriteEntityExtendedPropertyDto>()
                {

                }
            }
        };

        // Persons to create
        public static string Person1Email = "person1@ramp.com.au";
        public static string Person2Email = "person2@ramp.com.au";
        public static List<BuilderAddPersonDto> PersonsToCreate = new List<BuilderAddPersonDto>()
        {
            new BuilderAddPersonDto()
            {
                GivenName = "Person 1",
                Surname = "Test",
                Email = Person1Email,
                TemplateName = PersonTemplateName,
                PersonTagList = new List<WriteTagDto>()
                {

                },
                BuilderPersonExtendedPropertyList = new List<BuilderWriteEntityExtendedPropertyDto>()
                {

                }
            },
            new BuilderAddPersonDto()
            {
                GivenName = "Person 2",
                Surname = "Test",
                Email = Person2Email,
                TemplateName = PersonTemplateName,
                PersonTagList = new List<WriteTagDto>()
                {

                },
                BuilderPersonExtendedPropertyList = new List<BuilderWriteEntityExtendedPropertyDto>()
                {

                }
            }
        };

        // Places to create
        public static string Place1Name = "Store 1";
        public static string Place2Name = "Store 2";
        public static List<BuilderAddPlaceDto> PlacesToCreate = new List<BuilderAddPlaceDto>()
        {
            new BuilderAddPlaceDto()
            {
                Name = Place1Name,
                TemplateName = PlaceTemplateName,
                PlaceTagList = new List<WriteTagDto>()
                {

                },
                BuilderPlaceExtendedPropertyList = new List<BuilderWriteEntityExtendedPropertyDto>()
                {
                    
                }
            },
            new BuilderAddPlaceDto()
            {
                Name = Place2Name,
                TemplateName = PlaceTemplateName,
                PlaceTagList = new List<WriteTagDto>()
                {

                },
                BuilderPlaceExtendedPropertyList = new List<BuilderWriteEntityExtendedPropertyDto>()
                {

                }
            }
        };


        // Items to create
        public static string Asset1Name = "Hammer";
        public static string Asset1TagNumber = "586000000000000000000001";
        public static string Asset2Name = "Circular Saw";
        public static string Asset2TagNumber = "586000000000000000000002";
        public static string Asset3Name = "Monitor";
        public static string Asset3TagNumber = "596000000000000000000001";
        public static string Asset4Name = "Printer";
        public static string Asset4TagNumber = "596000000000000000000002";
        public static List<BuilderAddItemDto> ItemsToCreate = new List<BuilderAddItemDto>()
        {
            new BuilderAddItemDto()
            {
                Name = Asset1Name,
                Description = "A hammer",
                SkuName = AssetCategory1Name,
                PlaceName = Place1Name,
                PersonEmail = Person1Email,
                ItemTagList = new List<WriteTagDto>()
                {
                    new WriteTagDto()
                    {
                        TagNumber = Asset1TagNumber
                    }
                },
                BuilderItemExtendedPropertyList = new List<BuilderWriteItemExtendedPropertyDto>()
                {
                    new BuilderWriteItemExtendedPropertyDto()
                    {
                        ExtendedPropertyName = ServiceDateExtPropName,
                        Value = ""
                    }
                }
            },
            new BuilderAddItemDto()
            {
                Name = Asset2Name,
                Description = "2000W circular saw",
                SkuName = AssetCategory1Name,
                PlaceName = Place1Name,
                PersonEmail = Person1Email,
                ItemTagList = new List<WriteTagDto>()
                {
                    new WriteTagDto()
                    {
                        TagNumber = Asset2TagNumber
                    }
                },
                BuilderItemExtendedPropertyList = new List<BuilderWriteItemExtendedPropertyDto>()
                {
                    new BuilderWriteItemExtendedPropertyDto()
                    {
                        ExtendedPropertyName = ServiceDateExtPropName,
                        Value = ""
                    }
                }
            },
            new BuilderAddItemDto()
            {
                Name = Asset3Name,
                Description = "Dell 22\" Monitor",
                SkuName = AssetCategory2Name,
                PlaceName = Place2Name,
                PersonEmail = Person2Email,
                ItemTagList = new List<WriteTagDto>()
                {
                    new WriteTagDto()
                    {
                        TagNumber = Asset3TagNumber
                    }
                },
                BuilderItemExtendedPropertyList = new List<BuilderWriteItemExtendedPropertyDto>()
                {
                    new BuilderWriteItemExtendedPropertyDto()
                    {
                        ExtendedPropertyName = ServiceDateExtPropName,
                        Value = ""
                    }
                }
            },
            new BuilderAddItemDto()
            {
                Name = Asset4Name,
                Description = "HP Printer",
                SkuName = AssetCategory2Name,
                PlaceName = Place2Name,
                PersonEmail = Person2Email,
                ItemTagList = new List<WriteTagDto>()
                {
                    new WriteTagDto()
                    {
                        TagNumber = Asset4TagNumber
                    }
                },
                BuilderItemExtendedPropertyList = new List<BuilderWriteItemExtendedPropertyDto>()
                {
                    new BuilderWriteItemExtendedPropertyDto()
                    {
                        ExtendedPropertyName = ServiceDateExtPropName,
                        Value = ""
                    }
                }
            }
        };
    }
}
