using Locafi.Client.Model.Dto.Inventory;
using Locafi.Client.Model.Dto.Items;
using Locafi.Client.Model.Dto.Skus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Locafi.Client.UnitTests.Validators
{
    public static class ItemDtoValidator
    {
        public static void ItemSummaryCheck(ItemSummaryDto dto)
        {
            BaseDtoValidator.DtoBaseCheck(dto);

            try {
                BaseDtoValidator.CheckString(dto.Name, "ItemSummaryCheck: Name == null/Empty");

                BaseDtoValidator.CheckGuid(dto.SkuId, "ItemSummaryCheck: SkuId == null/Empty");
                BaseDtoValidator.CheckString(dto.SkuName, "ItemSummaryCheck: SkuName == null/Empty");

                BaseDtoValidator.CheckGuid(dto.PlaceId, "ItemSummaryCheck: PlaceId == null/Empty");
                BaseDtoValidator.CheckString(dto.PlaceName, "ItemSummaryCheck: PlaceName == null/Empty");
            }catch(Exception e)
            {
                var msg = "Error Message: " + e.Message + "\r\n\r\nStack Trace: " + e.StackTrace;
                MessageBox.Show(msg, "Validation Error");
                throw e;
            }
        }

        public static void ItemSummaryReasonCheck(ItemSummaryReasonDto dto, Guid? reasonId = null)
        {
            ItemSummaryCheck(dto);

            try
            {
                BaseDtoValidator.CheckNullableGuid(dto.ReasonId, "ItemSummaryReasonCheck: ReasonId == Empty");

                Validator.AreEqual(reasonId, dto.ReasonId, "ItemSummaryReasonCheck: ReasonId doesn't match");
            }
            catch (Exception e)
            {
                var msg = "Error Message: " + e.Message + "\r\n\r\nStack Trace: " + e.StackTrace;
                MessageBox.Show(msg, "Validation Error");
                throw e;
            }
        }

        public static void ItemDetailCheck(ItemDetailDto dto, SkuDetailDto skuDto = null)
        {
            ItemSummaryCheck(dto);

            try {

                if (skuDto != null)
                {
                    // check for valid extended properties (don't look at sku level ones)
                    foreach (var prop in skuDto.SkuExtendedPropertyList.Where(p => !p.IsSkuLevelProperty && p.ExtendedPropertyIsRequired))
                    {
                        var dtoProp = dto.ItemExtendedPropertyList.FirstOrDefault(p => p.ExtendedPropertyId == prop.ExtendedPropertyId);
                        Assert.IsNotNull(dtoProp, "ItemDetailCheck: " + dtoProp.Name + " == null");
                        if (prop.ExtendedPropertyIsRequired)
                            BaseDtoValidator.CheckString(dtoProp.Value, "ItemDetailCheck: " + dtoProp.Name + " requires a value");
                    }
                }
            }catch(Exception e)
            {
                var msg = "Error Message: " + e.Message + "\r\n\r\nStack Trace: " + e.StackTrace;
                MessageBox.Show(msg, "Validation Error");
                throw e;
            }
        }
    }
}
