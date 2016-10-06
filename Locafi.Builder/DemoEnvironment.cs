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
    public static class DevEnvironment1
    {
        // Register details / 
        public static string OrganisationName => "Locafi_Demo_Organisation";
        public static string UserFirstName => "Locafi";
        public static string UserLastName => "Client";
        public static string RegUserEmail => "locaficlient@demo.com.au";
        public static string RegUserPassword => "tester123";

        // Extended Properties to add
        public static string ExpiryDateExtPropName = "Expiry Date";
        public static string SerialNoExtPropName = "Serial No";
        public static string LotNoExtPropName = "Lot No";
        public static string BatchNoExtPropName = "Batch No";
        public static string ManufacturerExtPropName = "Manufacturer";
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
                Name = SerialNoExtPropName,
                Description = SerialNoExtPropName,
                DataType = TemplateDataTypes.String,
                TemplateType = TemplateFor.Item
            },
            new AddExtendedPropertyDto()
            {
                Name = LotNoExtPropName,
                Description = LotNoExtPropName,
                DataType = TemplateDataTypes.String,
                TemplateType = TemplateFor.Item
            },
            new AddExtendedPropertyDto()
            {
                Name = BatchNoExtPropName,
                Description = BatchNoExtPropName,
                DataType = TemplateDataTypes.String,
                TemplateType = TemplateFor.Item
            },
            new AddExtendedPropertyDto()
            {
                Name = ManufacturerExtPropName,
                Description = ManufacturerExtPropName,
                DataType = TemplateDataTypes.String,
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
                    },
                    new BuilderAddTemplateExtendedPropertyDto()
                    {
                        ExtendedPropertyName = SerialNoExtPropName,
                        IsRequired = false
                    },
                    new BuilderAddTemplateExtendedPropertyDto()
                    {
                        ExtendedPropertyName = LotNoExtPropName,
                        IsRequired = false
                    },
                    new BuilderAddTemplateExtendedPropertyDto()
                    {
                        ExtendedPropertyName = BatchNoExtPropName,
                        IsRequired = false
                    },
                    new BuilderAddTemplateExtendedPropertyDto()
                    {
                        ExtendedPropertyName = ManufacturerExtPropName,
                        IsRequired = true
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
                        ExtendedPropertyName = SerialNoExtPropName,
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
        public static string SkuCategory1Name = "Prosthetic Arm";
        public static string SkuCategory2Name = "Prosthetic Leg";
        public static string SkuCategory3Name = "Prosthetic Knee";
        public static string SkuCategory4Name = "Prosthetic Foot";
        public static string SkuCategory5Name = "Silicone Arm Cover";
        public static string SkuCategory6Name = "Dynamic Arm";
        public static string SkuCategory7Name = "Silicone Partial Finger";
        public static string SkuCategory8Name = "residual Limb Socket";
        public static string AssetCategory1Name = "Tools";
        public static string AssetCategory2Name = "IT Assets";
        public static List<BuilderAddSkuDto> SkusToCreate = new List<BuilderAddSkuDto>()
        {
            new BuilderAddSkuDto()
            {
                Name = SkuCategory1Name,
                SkuNumber = "END001",
                CompanyPrefix = "3000000",
                ItemReference = "000001",
                CustomPrefix = "",
                IsSgtin = true,
                TemplateName = SkuTemplateName,
                BuilderSkuExtendedPropertyList = new List<BuilderWriteSkuExtendedPropertyDto>()
                {
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = ExpiryDateExtPropName,
                        IsSkuLevelProperty = false
                    },
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = SerialNoExtPropName,
                        IsSkuLevelProperty = false
                    },
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = LotNoExtPropName,
                        IsSkuLevelProperty = false
                    },
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = BatchNoExtPropName,
                        IsSkuLevelProperty = false
                    },
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = ManufacturerExtPropName,
                        IsSkuLevelProperty = true,
                        Value = "Endolite"
                    }
                }
            },
            new BuilderAddSkuDto()
            {
                Name = SkuCategory2Name,
                SkuNumber = "END002",
                CompanyPrefix = "3000000",
                ItemReference = "000002",
                CustomPrefix = "",
                IsSgtin = true,
                TemplateName = SkuTemplateName,
                BuilderSkuExtendedPropertyList = new List<BuilderWriteSkuExtendedPropertyDto>()
                {
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = ExpiryDateExtPropName,
                        IsSkuLevelProperty = false
                    },
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = SerialNoExtPropName,
                        IsSkuLevelProperty = false
                    },
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = LotNoExtPropName,
                        IsSkuLevelProperty = false
                    },
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = BatchNoExtPropName,
                        IsSkuLevelProperty = false
                    },
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = ManufacturerExtPropName,
                        IsSkuLevelProperty = true,
                        Value = "Endolite"
                    }
                }
            },
            new BuilderAddSkuDto()
            {
                Name = SkuCategory3Name,
                SkuNumber = "END003",
                CompanyPrefix = "3000000",
                ItemReference = "000003",
                CustomPrefix = "",
                IsSgtin = true,
                TemplateName = SkuTemplateName,
                BuilderSkuExtendedPropertyList = new List<BuilderWriteSkuExtendedPropertyDto>()
                {
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = ExpiryDateExtPropName,
                        IsSkuLevelProperty = false
                    },
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = SerialNoExtPropName,
                        IsSkuLevelProperty = false
                    },
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = LotNoExtPropName,
                        IsSkuLevelProperty = false
                    },
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = BatchNoExtPropName,
                        IsSkuLevelProperty = false
                    },
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = ManufacturerExtPropName,
                        IsSkuLevelProperty = true,
                        Value = "Endolite"
                    }
                }
            },
            new BuilderAddSkuDto()
            {
                Name = SkuCategory4Name,
                SkuNumber = "END004",
                CompanyPrefix = "3000000",
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
                    },
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = SerialNoExtPropName,
                        IsSkuLevelProperty = false
                    },
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = LotNoExtPropName,
                        IsSkuLevelProperty = false
                    },
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = BatchNoExtPropName,
                        IsSkuLevelProperty = false
                    },
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = ManufacturerExtPropName,
                        IsSkuLevelProperty = true,
                        Value = "Endolite"
                    }
                }
            },
            new BuilderAddSkuDto()
            {
                Name = SkuCategory5Name,
                SkuNumber = "OTT001",
                CompanyPrefix = "4000000",
                ItemReference = "000001",
                CustomPrefix = "",
                IsSgtin = true,
                TemplateName = SkuTemplateName,
                BuilderSkuExtendedPropertyList = new List<BuilderWriteSkuExtendedPropertyDto>()
                {
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = ExpiryDateExtPropName,
                        IsSkuLevelProperty = false
                    },
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = SerialNoExtPropName,
                        IsSkuLevelProperty = false
                    },
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = LotNoExtPropName,
                        IsSkuLevelProperty = false
                    },
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = BatchNoExtPropName,
                        IsSkuLevelProperty = false
                    },
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = ManufacturerExtPropName,
                        IsSkuLevelProperty = true,
                        Value = "Ottobock"
                    }
                }
            },
            new BuilderAddSkuDto()
            {
                Name = SkuCategory6Name,
                SkuNumber = "OTT002",
                CompanyPrefix = "4000000",
                ItemReference = "000002",
                CustomPrefix = "",
                IsSgtin = true,
                TemplateName = SkuTemplateName,
                BuilderSkuExtendedPropertyList = new List<BuilderWriteSkuExtendedPropertyDto>()
                {
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = ExpiryDateExtPropName,
                        IsSkuLevelProperty = false
                    },
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = SerialNoExtPropName,
                        IsSkuLevelProperty = false
                    },
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = LotNoExtPropName,
                        IsSkuLevelProperty = false
                    },
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = BatchNoExtPropName,
                        IsSkuLevelProperty = false
                    },
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = ManufacturerExtPropName,
                        IsSkuLevelProperty = true,
                        Value = "Ottobock"
                    }
                }
            },
            new BuilderAddSkuDto()
            {
                Name = SkuCategory7Name,
                SkuNumber = "OTT003",
                CompanyPrefix = "4000000",
                ItemReference = "000003",
                CustomPrefix = "",
                IsSgtin = true,
                TemplateName = SkuTemplateName,
                BuilderSkuExtendedPropertyList = new List<BuilderWriteSkuExtendedPropertyDto>()
                {
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = ExpiryDateExtPropName,
                        IsSkuLevelProperty = false
                    },
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = SerialNoExtPropName,
                        IsSkuLevelProperty = false
                    },
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = LotNoExtPropName,
                        IsSkuLevelProperty = false
                    },
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = BatchNoExtPropName,
                        IsSkuLevelProperty = false
                    },
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = ManufacturerExtPropName,
                        IsSkuLevelProperty = true,
                        Value = "Ottobock"
                    }
                }
            },
            new BuilderAddSkuDto()
            {
                Name = SkuCategory8Name,
                SkuNumber = "OTT004",
                CompanyPrefix = "4000000",
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
                    },
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = SerialNoExtPropName,
                        IsSkuLevelProperty = false
                    },
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = LotNoExtPropName,
                        IsSkuLevelProperty = false
                    },
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = BatchNoExtPropName,
                        IsSkuLevelProperty = false
                    },
                    new BuilderWriteSkuExtendedPropertyDto()
                    {
                        ExtendedPropertyName = ManufacturerExtPropName,
                        IsSkuLevelProperty = true,
                        Value = "Ottobock"
                    }
                }
            }
            //new BuilderAddSkuDto()
            //{
            //    Name = AssetCategory1Name,
            //    SkuNumber = "T001",
            //    CompanyPrefix = "",
            //    ItemReference = "",
            //    CustomPrefix = "5860",
            //    TemplateName = AssetTemplateName,
            //    BuilderSkuExtendedPropertyList = new List<BuilderWriteSkuExtendedPropertyDto>()
            //    {
            //        new BuilderWriteSkuExtendedPropertyDto()
            //        {
            //            ExtendedPropertyName = SerialNoExtPropName,
            //            IsSkuLevelProperty = false
            //        }
            //    }
            //},
            //new BuilderAddSkuDto()
            //{
            //    Name = AssetCategory2Name,
            //    SkuNumber = "IT001",
            //    CompanyPrefix = "",
            //    ItemReference = "",
            //    CustomPrefix = "5960",
            //    TemplateName = AssetTemplateName,
            //    BuilderSkuExtendedPropertyList = new List<BuilderWriteSkuExtendedPropertyDto>()
            //    {
            //        new BuilderWriteSkuExtendedPropertyDto()
            //        {
            //            ExtendedPropertyName = SerialNoExtPropName,
            //            IsSkuLevelProperty = false
            //        }
            //    }
            //}
        };

        // Roles to create
        public static string AdminRoleName = "Administrator";
        //public static List<BuilderAddRoleDto> RolesToCreate = new List<BuilderAddRoleDto>() { }

        // Users to register
        public static string HHTestUserEmail = "HH@demo.com.au";
        public static string HHTestUserPassword = "demo123";
        public static string TestUserEmail = "clienttester@demo.comm.au";
        public static string TestUserPassword = "tester123";
        public static string DemoUserEmail = "demo@demo.com.au";
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
        public static string Person1Email = "person1@demo.com.au";
        public static string Person2Email = "person2@demo.com.au";
        public static string Person3Email = "person3@demo.com.au";
        public static string Person4Email = "person4@demo.com.au";
        public static string Person5Email = "person5@demo.com.au";
        public static string Person6Email = "person6@demo.com.au";
        public static string Person7Email = "person7@demo.com.au";
        public static string Person8Email = "person8@demo.com.au";
        public static string Person9Email = "person9@demo.com.au";
        public static string Person10Email = "person10@demo.com.au";
        public static List<BuilderAddPersonDto> PersonsToCreate = new List<BuilderAddPersonDto>()
        {
            new BuilderAddPersonDto()
            {
                GivenName = "Person 1",
                Surname = "Demo",
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
                Surname = "Demo",
                Email = Person2Email,
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
                GivenName = "Person 3",
                Surname = "Demo",
                Email = Person3Email,
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
                GivenName = "Person 4",
                Surname = "Demo",
                Email = Person4Email,
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
                GivenName = "Person 5",
                Surname = "Demo",
                Email = Person5Email,
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
                GivenName = "Person 6",
                Surname = "Demo",
                Email = Person6Email,
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
                GivenName = "Person 7",
                Surname = "Demo",
                Email = Person7Email,
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
                GivenName = "Person 8",
                Surname = "Demo",
                Email = Person8Email,
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
                GivenName = "Person 9",
                Surname = "Demo",
                Email = Person9Email,
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
                GivenName = "Person 10",
                Surname = "Demo",
                Email = Person10Email,
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
        public static string Place1Name = "Stock Room 1";
        public static string Place2Name = "Stock Room 2";
        public static string Place3Name = "Stock Room 3";
        public static string Place4Name = "Stock Room 4";
        public static string Place5Name = "Stock Room 5";
        public static string Place6Name = "Stock Room 6";
        public static string Place7Name = "Stock Room 7";
        public static string Place8Name = "Stock Room 8";
        public static string Place9Name = "Stock Room 9";
        public static string Place10Name = "Stock Room 10";
        public static string Place11Name = "Stock Room 11";
        public static string Place12Name = "Stock Room 12";
        public static string Place13Name = "Stock Room 13";
        public static string Place14Name = "Stock Room 14";
        public static string Place15Name = "Stock Room 15";
        public static string Place16Name = "Stock Room 16";
        public static string Place17Name = "Stock Room 17";
        public static string Place18Name = "Stock Room 18";
        public static string Place19Name = "Stock Room 19";
        public static string Place20Name = "Stock Room 20";
        public static string Place21Name = "Stock Room 21";
        public static string Place22Name = "Stock Room 22";
        public static string Place23Name = "Stock Room 23";
        public static string Place24Name = "Stock Room 24";
        public static string Place25Name = "Stock Room 25";
        public static string Place26Name = "Stock Room 26";
        public static string Place27Name = "Stock Room 27";
        public static string Place28Name = "Stock Room 28";
        public static string Place29Name = "Stock Room 29";
        public static string Place30Name = "Stock Room 30";
        public static string Place31Name = "Stock Room 31";
        public static string Place32Name = "Stock Room 32";
        public static string Place33Name = "Stock Room 33";
        public static string Place34Name = "Stock Room 34";
        public static string Place35Name = "Stock Room 35";
        public static string Place36Name = "Stock Room 36";
        public static string Place37Name = "Stock Room 37";
        public static string Place38Name = "Stock Room 38";
        public static string Place39Name = "Stock Room 39";
        public static string Place40Name = "Stock Room 40";
        public static string Place41Name = "Stock Room 41";
        public static string Place42Name = "Stock Room 42";
        public static string Place43Name = "Stock Room 43";
        public static string Place44Name = "Stock Room 44";
        public static string Place45Name = "DHL Warehouse";
        public static string Place46Name = "Hospital 1";
        public static string Place47Name = "Hospital 2";
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
            },
            new BuilderAddPlaceDto()
            {
                Name = Place3Name,
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
                Name = Place4Name,
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
                Name = Place5Name,
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
                Name = Place6Name,
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
                Name = Place7Name,
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
                Name = Place8Name,
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
                Name = Place9Name,
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
                Name = Place10Name,
                TemplateName = PlaceTemplateName,
                PlaceTagList = new List<WriteTagDto>()
                {

                },
                BuilderPlaceExtendedPropertyList = new List<BuilderWriteEntityExtendedPropertyDto>()
                {

                }
            },new BuilderAddPlaceDto()
            {
                Name = Place11Name,
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
                Name = Place12Name,
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
                Name = Place13Name,
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
                Name = Place14Name,
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
                Name = Place15Name,
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
                Name = Place16Name,
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
                Name = Place17Name,
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
                Name = Place18Name,
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
                Name = Place19Name,
                TemplateName = PlaceTemplateName,
                PlaceTagList = new List<WriteTagDto>()
                {

                },
                BuilderPlaceExtendedPropertyList = new List<BuilderWriteEntityExtendedPropertyDto>()
                {

                }
            },new BuilderAddPlaceDto()
            {
                Name = Place20Name,
                TemplateName = PlaceTemplateName,
                PlaceTagList = new List<WriteTagDto>()
                {

                },
                BuilderPlaceExtendedPropertyList = new List<BuilderWriteEntityExtendedPropertyDto>()
                {

                }
            },new BuilderAddPlaceDto()
            {
                Name = Place21Name,
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
                Name = Place22Name,
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
                Name = Place23Name,
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
                Name = Place24Name,
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
                Name = Place25Name,
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
                Name = Place26Name,
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
                Name = Place27Name,
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
                Name = Place28Name,
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
                Name = Place29Name,
                TemplateName = PlaceTemplateName,
                PlaceTagList = new List<WriteTagDto>()
                {

                },
                BuilderPlaceExtendedPropertyList = new List<BuilderWriteEntityExtendedPropertyDto>()
                {

                }
            },new BuilderAddPlaceDto()
            {
                Name = Place30Name,
                TemplateName = PlaceTemplateName,
                PlaceTagList = new List<WriteTagDto>()
                {

                },
                BuilderPlaceExtendedPropertyList = new List<BuilderWriteEntityExtendedPropertyDto>()
                {

                }
            },new BuilderAddPlaceDto()
            {
                Name = Place31Name,
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
                Name = Place32Name,
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
                Name = Place33Name,
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
                Name = Place34Name,
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
                Name = Place35Name,
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
                Name = Place36Name,
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
                Name = Place37Name,
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
                Name = Place38Name,
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
                Name = Place39Name,
                TemplateName = PlaceTemplateName,
                PlaceTagList = new List<WriteTagDto>()
                {

                },
                BuilderPlaceExtendedPropertyList = new List<BuilderWriteEntityExtendedPropertyDto>()
                {

                }
            },new BuilderAddPlaceDto()
            {
                Name = Place40Name,
                TemplateName = PlaceTemplateName,
                PlaceTagList = new List<WriteTagDto>()
                {

                },
                BuilderPlaceExtendedPropertyList = new List<BuilderWriteEntityExtendedPropertyDto>()
                {

                }
            },new BuilderAddPlaceDto()
            {
                Name = Place41Name,
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
                Name = Place42Name,
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
                Name = Place43Name,
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
                Name = Place44Name,
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
                Name = Place45Name,
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
                Name = Place46Name,
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
                Name = Place47Name,
                TemplateName = PlaceTemplateName,
                PlaceTagList = new List<WriteTagDto>()
                {

                },
                BuilderPlaceExtendedPropertyList = new List<BuilderWriteEntityExtendedPropertyDto>()
                {

                }
            }
            //,new BuilderAddPlaceDto()
            //{
            //    Name = Place0Name,
            //    TemplateName = PlaceTemplateName,
            //    PlaceTagList = new List<WriteTagDto>()
            //    {

            //    },
            //    BuilderPlaceExtendedPropertyList = new List<BuilderWriteEntityExtendedPropertyDto>()
            //    {

            //    }
            //},new BuilderAddPlaceDto()
            //{
            //    Name = Place1Name,
            //    TemplateName = PlaceTemplateName,
            //    PlaceTagList = new List<WriteTagDto>()
            //    {

            //    },
            //    BuilderPlaceExtendedPropertyList = new List<BuilderWriteEntityExtendedPropertyDto>()
            //    {

            //    }
            //},
            //new BuilderAddPlaceDto()
            //{
            //    Name = Place2Name,
            //    TemplateName = PlaceTemplateName,
            //    PlaceTagList = new List<WriteTagDto>()
            //    {

            //    },
            //    BuilderPlaceExtendedPropertyList = new List<BuilderWriteEntityExtendedPropertyDto>()
            //    {

            //    }
            //},
            //new BuilderAddPlaceDto()
            //{
            //    Name = Place3Name,
            //    TemplateName = PlaceTemplateName,
            //    PlaceTagList = new List<WriteTagDto>()
            //    {

            //    },
            //    BuilderPlaceExtendedPropertyList = new List<BuilderWriteEntityExtendedPropertyDto>()
            //    {

            //    }
            //},
            //new BuilderAddPlaceDto()
            //{
            //    Name = Place4Name,
            //    TemplateName = PlaceTemplateName,
            //    PlaceTagList = new List<WriteTagDto>()
            //    {

            //    },
            //    BuilderPlaceExtendedPropertyList = new List<BuilderWriteEntityExtendedPropertyDto>()
            //    {

            //    }
            //},
            //new BuilderAddPlaceDto()
            //{
            //    Name = Place5Name,
            //    TemplateName = PlaceTemplateName,
            //    PlaceTagList = new List<WriteTagDto>()
            //    {

            //    },
            //    BuilderPlaceExtendedPropertyList = new List<BuilderWriteEntityExtendedPropertyDto>()
            //    {

            //    }
            //},
            //new BuilderAddPlaceDto()
            //{
            //    Name = Place6Name,
            //    TemplateName = PlaceTemplateName,
            //    PlaceTagList = new List<WriteTagDto>()
            //    {

            //    },
            //    BuilderPlaceExtendedPropertyList = new List<BuilderWriteEntityExtendedPropertyDto>()
            //    {

            //    }
            //},
            //new BuilderAddPlaceDto()
            //{
            //    Name = Place7Name,
            //    TemplateName = PlaceTemplateName,
            //    PlaceTagList = new List<WriteTagDto>()
            //    {

            //    },
            //    BuilderPlaceExtendedPropertyList = new List<BuilderWriteEntityExtendedPropertyDto>()
            //    {

            //    }
            //},
            //new BuilderAddPlaceDto()
            //{
            //    Name = Place8Name,
            //    TemplateName = PlaceTemplateName,
            //    PlaceTagList = new List<WriteTagDto>()
            //    {

            //    },
            //    BuilderPlaceExtendedPropertyList = new List<BuilderWriteEntityExtendedPropertyDto>()
            //    {

            //    }
            //},
            //new BuilderAddPlaceDto()
            //{
            //    Name = Place9Name,
            //    TemplateName = PlaceTemplateName,
            //    PlaceTagList = new List<WriteTagDto>()
            //    {

            //    },
            //    BuilderPlaceExtendedPropertyList = new List<BuilderWriteEntityExtendedPropertyDto>()
            //    {

            //    }
            //}
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
            //new BuilderAddItemDto()
            //{
            //    Name = Asset1Name,
            //    Description = "A hammer",
            //    SkuName = AssetCategory1Name,
            //    PlaceName = Place1Name,
            //    PersonEmail = Person1Email,
            //    ItemTagList = new List<WriteTagDto>()
            //    {
            //        new WriteTagDto()
            //        {
            //            TagNumber = Asset1TagNumber
            //        }
            //    },
            //    BuilderItemExtendedPropertyList = new List<BuilderWriteItemExtendedPropertyDto>()
            //    {
            //        new BuilderWriteItemExtendedPropertyDto()
            //        {
            //            ExtendedPropertyName = SerialNoExtPropName,
            //            Value = ""
            //        }
            //    }
            //},
            //new BuilderAddItemDto()
            //{
            //    Name = Asset2Name,
            //    Description = "2000W circular saw",
            //    SkuName = AssetCategory1Name,
            //    PlaceName = Place1Name,
            //    PersonEmail = Person1Email,
            //    ItemTagList = new List<WriteTagDto>()
            //    {
            //        new WriteTagDto()
            //        {
            //            TagNumber = Asset2TagNumber
            //        }
            //    },
            //    BuilderItemExtendedPropertyList = new List<BuilderWriteItemExtendedPropertyDto>()
            //    {
            //        new BuilderWriteItemExtendedPropertyDto()
            //        {
            //            ExtendedPropertyName = SerialNoExtPropName,
            //            Value = ""
            //        }
            //    }
            //},
            //new BuilderAddItemDto()
            //{
            //    Name = Asset3Name,
            //    Description = "Dell 22\" Monitor",
            //    SkuName = AssetCategory2Name,
            //    PlaceName = Place2Name,
            //    PersonEmail = Person2Email,
            //    ItemTagList = new List<WriteTagDto>()
            //    {
            //        new WriteTagDto()
            //        {
            //            TagNumber = Asset3TagNumber
            //        }
            //    },
            //    BuilderItemExtendedPropertyList = new List<BuilderWriteItemExtendedPropertyDto>()
            //    {
            //        new BuilderWriteItemExtendedPropertyDto()
            //        {
            //            ExtendedPropertyName = SerialNoExtPropName,
            //            Value = ""
            //        }
            //    }
            //},
            //new BuilderAddItemDto()
            //{
            //    Name = Asset4Name,
            //    Description = "HP Printer",
            //    SkuName = AssetCategory2Name,
            //    PlaceName = Place2Name,
            //    PersonEmail = Person2Email,
            //    ItemTagList = new List<WriteTagDto>()
            //    {
            //        new WriteTagDto()
            //        {
            //            TagNumber = Asset4TagNumber
            //        }
            //    },
            //    BuilderItemExtendedPropertyList = new List<BuilderWriteItemExtendedPropertyDto>()
            //    {
            //        new BuilderWriteItemExtendedPropertyDto()
            //        {
            //            ExtendedPropertyName = SerialNoExtPropName,
            //            Value = ""
            //        }
            //    }
            //}
        };
    }
}
