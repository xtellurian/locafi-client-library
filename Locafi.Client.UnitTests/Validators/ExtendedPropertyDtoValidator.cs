using Locafi.Client.Model.Dto;
using Locafi.Client.Model.Dto.ExtendedProperties;
using Locafi.Client.Model.Dto.Items;
using Locafi.Client.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Locafi.Client.UnitTests.Validators
{
    public static class ExtendedPropertyDtoValidator
    {
        public static void ExtendedPropertySummaryCheck(ExtendedPropertySummaryDto dto)
        {
            BaseDtoValidator.DtoBaseCheck(dto);

            Validator.IsInstanceOfType(dto, typeof(ExtendedPropertySummaryDto));

            Validator.IsNotNull(dto.TemplateType, "ExtendedPropertySummaryCheck: TemplateType == null");
            Validator.IsNotNull(dto.DataType, "ExtendedPropertySummaryCheck: DataType == null");

            try
            {
                BaseDtoValidator.CheckString(dto.Name, "ExtendedPropertySummaryCheck: Name == null/Empty");
            }
            catch (Exception e)
            {
                var msg = "Error Message: " + e.Message + "\r\n\r\nStack Trace: " + e.StackTrace;
                MessageBox.Show(msg, "Validation Error");
                throw e;
            }
        }

        public static void ExtendedPropertyDetailCheck(ExtendedPropertyDetailDto dto)
        {
            ExtendedPropertySummaryCheck(dto);

            Validator.IsInstanceOfType(dto, typeof(ExtendedPropertyDetailDto));
        }

        public static bool ParsedValuesAreEqual(ReadEntityExtendedPropertyDto rDto, WriteEntityExtendedPropertyDto wDto)
        {
            if (string.IsNullOrEmpty(rDto.Value) && string.IsNullOrEmpty(wDto.Value))
                return true;

            try
            {
                switch (rDto.ExtendedPropertyDataType)
                {
                    case TemplateDataTypes.Bool:
                        var testBool1 = bool.Parse(rDto.Value);
                        var testBool2 = bool.Parse(wDto.Value);
                        return testBool1 == testBool2;
                        break;
                    case TemplateDataTypes.DateTime:
                        var testDT1 = DateTime.Parse(rDto.Value);
                        var testDT2 = DateTime.Parse(wDto.Value);
                        return testDT1 == testDT2;
                        break;
                    case TemplateDataTypes.Decimal:
                        var testDecimal1 = double.Parse(rDto.Value);
                        var testDecimal2 = double.Parse(wDto.Value);
                        return testDecimal1 == testDecimal2;
                        break;
                    case TemplateDataTypes.Number:
                        var testNumber1 = int.Parse(rDto.Value);
                        var testNumber2 = int.Parse(wDto.Value);
                        return testNumber1 == testNumber2;
                        break;
                    case TemplateDataTypes.String:
                        var testString1 = rDto.Value;
                        var testString2 = rDto.Value;
                        return testString1 == testString2;
                        break;
                }
                return false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public static bool ParsedValuesAreEqual(ReadItemExtendedPropertyDto rDto, WriteItemExtendedPropertyDto wDto)
        {
            if (string.IsNullOrEmpty(rDto.Value) && string.IsNullOrEmpty(wDto.Value))
                    return true;
            try
            {
                switch (rDto.DataType)
                {
                    case TemplateDataTypes.Bool:
                        var testBool1 = bool.Parse(rDto.Value);
                        var testBool2 = bool.Parse(wDto.Value);
                        return testBool1 == testBool2;
                        break;
                    case TemplateDataTypes.DateTime:
                        var testDT1 = DateTime.Parse(rDto.Value);
                        var testDT2 = DateTime.Parse(wDto.Value);
                        return testDT1 == testDT2;
                        break;
                    case TemplateDataTypes.Decimal:
                        var testDecimal1 = double.Parse(rDto.Value);
                        var testDecimal2 = double.Parse(wDto.Value);
                        return testDecimal1 == testDecimal2;
                        break;
                    case TemplateDataTypes.Number:
                        var testNumber1 = int.Parse(rDto.Value);
                        var testNumber2 = int.Parse(wDto.Value);
                        return testNumber1 == testNumber2;
                        break;
                    case TemplateDataTypes.String:
                        var testString1 = rDto.Value;
                        var testString2 = rDto.Value;
                        return testString1 == testString2;
                        break;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool CanParseDtoValue(ReadEntityExtendedPropertyDto dto)
        {
            if (string.IsNullOrEmpty(dto.Value))
                return true;

            try
            {
                switch (dto.ExtendedPropertyDataType)
                {
                    case TemplateDataTypes.Bool:
                        var testBool = bool.Parse(dto.Value);
                        break;
                    case TemplateDataTypes.DateTime:
                        var testDT = DateTime.Parse(dto.Value);
                        break;
                    case TemplateDataTypes.Decimal:
                        var testDecimal = double.Parse(dto.Value);
                        break;
                    case TemplateDataTypes.Number:
                        var testNumber = int.Parse(dto.Value);
                        break;
                    case TemplateDataTypes.String:
                        var testString = dto.Value;
                        break;
                }

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public static bool CanParseDtoValue(ReadItemExtendedPropertyDto dto)
        {
            if (string.IsNullOrEmpty(dto.Value))
                return true;

            try
            {
                switch (dto.DataType)
                {
                    case TemplateDataTypes.Bool:
                        var testBool = bool.Parse(dto.Value);
                        break;
                    case TemplateDataTypes.DateTime:
                        var testDT = DateTime.Parse(dto.Value);
                        break;
                    case TemplateDataTypes.Decimal:
                        var testDecimal = double.Parse(dto.Value);
                        break;
                    case TemplateDataTypes.Number:
                        var testNumber = int.Parse(dto.Value);
                        break;
                    case TemplateDataTypes.String:
                        var testString = dto.Value;
                        break;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
