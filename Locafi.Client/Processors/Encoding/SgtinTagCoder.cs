/*

DO NOT MODIFY THIS CLASS

This is a class shared between server and client. It should not be modified on the client without comparible modifications on the server.
You can modify Sgtin.cs instead.

*/


using System;
using System.Collections.Generic;


namespace Locafi.Client.Processors.Encoding
{
    internal class SgtinTagCoder
    {
        /* EAN-13 / GTIN-13
         * - 13 characters
         * 1-12 are company prefix + item reference
         * 13 is check digit
         * Pad to 14 characters with leading zeros to convert to GTIN-14
        */

        /* EAN-12 / UPC-A / GTIN-12
         * - 12 characters
         * 1-11 are company prefix + item reference
         * 12 is check digit
         * Pad to 14 characters with leading zeros to convert to GTIN-14
        */

        #region Constants

        // EPC header values
        public const string SGTIN96Header = "00110000";
        public const string SGTIN64Header = "10";
        public const string SSCC96Header = "00110001";
        public const string SSCC64Header = "00001000";
        public const string SGTIN198Header = "00110110";
        public const byte SGTIN96HeaderVal = 48;

        // URI headers
        public const string SGTIN96Uri = "urn:epc:tag:sgtin-96:";
        public const string SSCC96Uri = "urn:epc:tag:sscc-96:";
        public const string SGTIN64Uri = "urn:epc:tag:sgtin-64:";
        public const string SSCC64Uri = "urn:epc:tag:sscc-64:";


        // ErrorMessages
        public const string INV_HEX_DIGIT = "This is not a valid Hexadecimal number - Unexpected digit [{0}] found";
        public const string INV_EPC_PARTITION = "Invalid partition value [{0}] found in EPC. Partition value has to be a number between 0 adn 6";
        public const string INV_EPC_LENGTHMODE = "Invalid partition length mode - supported values are Bits and Digits";
        public const string DB_NO_CONNECTION = "DB Error: Unable to connect to database";
        public const string DB_PRODUCT_NOT_FOUND = "{0} [{1}] does not exist in database";
        public const string INV_EPC_FORMAT = "This is not a supported EPC format";

        #endregion

        public string EPC { get; private set; }
        public SgtinInfo TagInfo { get; private set; }

        // creates from an existing hexadecimal EPC number
        public SgtinTagCoder(string epc)
        {
            EPC = epc;

            TagInfo = SgtinTagCoder.GetSgtinInfo(EPC);
        }

        // creates a new SGTIN EPC given inputs
        public SgtinTagCoder(EPCEncoding encoding, MerchandiseType type, string companyPrefix, string itemReference, long serialNumber)
        {
            if (TagInfo == null)
                TagInfo = new SgtinInfo();

            switch (encoding)
            {
                case EPCEncoding.SGTIN96:
                    TagInfo.Header = SGTIN96HeaderVal;
                    TagInfo.Filter = (byte)type;
                    TagInfo.Type = type;
                    break;
                default:
                    throw new Exception(INV_EPC_FORMAT);
                    break;
            }

            // set company prefix
            TagInfo.CompanyPrefix = companyPrefix;
            // set item reference
            TagInfo.ItemReference = itemReference;
            // set serial number
            TagInfo.SerialNumber = serialNumber;
            // set our version of the sku
            TagInfo.Gtin = TagInfo.CompanyPrefix + TagInfo.ItemReference;

            // get partition value based on company prefix decimal number size (number of digits)
            TagInfo.Partition = (byte)GetPartition(companyPrefix.Length);
            // get prefix length in bits
            int prefixLengthBits = GetCompanyPrefixLength(TagInfo.Partition, CompanyPrefixLengthMode.Bits);
            // get item ref length in bits
            int itemRefLengthBits = GetItemReferenceLength(TagInfo.Partition, CompanyPrefixLengthMode.Bits);

            // now create binary EPC string
            var binEpc = DecimalToBinary(TagInfo.Header, 8) + DecimalToBinary(TagInfo.Filter, 3) + DecimalToBinary(TagInfo.Partition, 3) + DecimalToBinary(Convert.ToInt64(TagInfo.CompanyPrefix), prefixLengthBits) + DecimalToBinary(Convert.ToInt64(TagInfo.ItemReference), itemRefLengthBits) + DecimalToBinary(TagInfo.SerialNumber, 38);

            // convert to Hexadecimal EPC string
            EPC = BinToHex(binEpc);
        }

        #region Public Methods

        public static string GetSgtinPrefix(EPCEncoding encoding, MerchandiseType type, string companyPrefix, string itemReference)
        {
            SgtinInfo TagInfo = new SgtinInfo();

            switch (encoding)
            {
                case EPCEncoding.SGTIN96:
                    TagInfo.Header = SGTIN96HeaderVal;
                    TagInfo.Filter = (byte)type;
                    TagInfo.Type = type;
                    break;
                default:
                    throw new Exception(INV_EPC_FORMAT);
                    break;
            }

            // set company prefix
            TagInfo.CompanyPrefix = companyPrefix;
            // set item reference
            TagInfo.ItemReference = itemReference;

            // get partition value based on company prefix decimal number size (number of digits)
            TagInfo.Partition = (byte)GetPartition(companyPrefix.Length);
            // get prefix length in bits
            int prefixLengthBits = GetCompanyPrefixLength(TagInfo.Partition, CompanyPrefixLengthMode.Bits);
            // get item ref length in bits
            int itemRefLengthBits = GetItemReferenceLength(TagInfo.Partition, CompanyPrefixLengthMode.Bits);

            // now create binary EPC string
            var sgtinPrefix = DecimalToBinary(TagInfo.Header, 8) + DecimalToBinary(TagInfo.Filter, 3) + DecimalToBinary(TagInfo.Partition, 3) + DecimalToBinary(Convert.ToInt64(TagInfo.CompanyPrefix), prefixLengthBits) + DecimalToBinary(Convert.ToInt64(TagInfo.ItemReference), itemRefLengthBits);

            // append "00" to round up to 8 bytes long (60bits)
            sgtinPrefix += "00";
            return BinToHex(sgtinPrefix);
        }

        public static bool IsSgtinFormat(string epc)
        {
            if (IsHex(epc.ToCharArray()))
            {
                var binEpc = HexToBin(epc);

                var header = (byte)BinaryToDecimal(binEpc.Substring(0, 8));

                try
                {
                    EPCEncoding encoding = (EPCEncoding)header;
                    if (encoding != EPCEncoding.Invalid)
                        return true;
                    else
                        return false;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        public static SgtinInfo GetSgtinInfo(string epc)
        {
            var info = new SgtinInfo();

            // check for valid hex number
            if (IsHex(epc.ToCharArray()))
            {
                var binEpc = HexToBin(epc);

                info.Header = (byte)BinaryToDecimal(binEpc.Substring(0, 8));

                if (Enum.IsDefined(typeof(EPCEncoding), (EPCEncoding)info.Header) && (EPCEncoding)info.Header != EPCEncoding.Invalid)
                {
                    info.Encoding = (EPCEncoding)info.Header;

                    info.Filter = (byte)BinaryToDecimal(binEpc.Substring(8, 3));
                    info.Type = (MerchandiseType)info.Filter;

                    info.Partition = (byte)BinaryToDecimal(binEpc.Substring(11, 3));

                    int prefixBitLength = GetCompanyPrefixLength(info.Partition, CompanyPrefixLengthMode.Bits);
                    int prefixDigitLength = GetCompanyPrefixLength(info.Partition, CompanyPrefixLengthMode.Digits);
                    info.CompanyPrefix = BinaryToDecimalString(binEpc.Substring(14, prefixBitLength), prefixDigitLength);

                    int itemRefBitLength = GetItemReferenceLength(info.Partition, CompanyPrefixLengthMode.Bits);
                    int itemRefDigitLength = GetItemReferenceLength(info.Partition, CompanyPrefixLengthMode.Digits);
                    info.ItemReference = BinaryToDecimalString(binEpc.Substring(14 + prefixBitLength, itemRefBitLength), itemRefDigitLength);

                    info.SerialNumber = BinaryToDecimal(binEpc.Substring(58, 38));

                    info.Gtin = ToGtin(info.CompanyPrefix, info.ItemReference);

                    return info;
                }
            }

            // if an unsupported EPC number is provided we will stop decoding and come in here to flag as invalid
            info.Encoding = EPCEncoding.Invalid;

            return info;
        }

        public static string ToGtin(string companyPrefix, string itemReference)
        {
            return companyPrefix + itemReference;
        }

        public static string GetGtin(string epc)
        {
            // check for a valid hex tag number
            if (IsHex(epc.ToCharArray()))
            {
                var binEpc = HexToBin(epc);

                var Header = (byte)BinaryToDecimal(binEpc.Substring(0, 8));

                if (Enum.IsDefined(typeof(EPCEncoding), (EPCEncoding)Header) && (EPCEncoding)Header != EPCEncoding.Invalid)
                {
                    var Filter = (byte)BinaryToDecimal(binEpc.Substring(8, 3));

                    var Partition = (byte)BinaryToDecimal(binEpc.Substring(11, 3));

                    int prefixLength = GetCompanyPrefixLength(Partition, CompanyPrefixLengthMode.Bits);
                    int prefixDigitLength = GetCompanyPrefixLength(Partition, CompanyPrefixLengthMode.Digits);
                    var CompanyPrefix = BinaryToDecimalString(binEpc.Substring(14, prefixLength), prefixDigitLength);

                    int itemRefLength = GetItemReferenceLength(Partition, CompanyPrefixLengthMode.Bits);
                    int itemRefDigitLength = GetItemReferenceLength(Partition, CompanyPrefixLengthMode.Digits);
                    var ItemReference = BinaryToDecimalString(binEpc.Substring(14 + prefixLength, itemRefLength), itemRefDigitLength);

                    return ToGtin(CompanyPrefix, ItemReference);
                }
            }

            return null;
        }

        public string EpcToEan13(string epc)
        {
            epc = epc.Replace("0x", "");
            string sgtin = "";
            string ean13 = "";
            Int64 serialNumber;

            sgtin = this.GetDecimal(epc);

            //check we have a gtin
            if (sgtin != "")
            {
                ean13 = sgtin;
                serialNumber = BinaryToDecimal(HexToBin(epc.Substring(epc.Length - 8)));
                sgtin = "urn:epc:id:sgtin:7701144." + sgtin.Substring(0, 1) + sgtin.Substring(8, 5) + "." + serialNumber;
                return ean13.Substring(0, 12);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Get EAN13 from sgtin
        /// </summary>
        /// <param name="epc"></param>
        /// <returns></returns>
        public string GetEAN13(string epc)
        {
            epc = epc.Replace("0x", "");
            string men;
            string ean = this.Decode(epc, BarcodeType.GTIN, out men);

            //epc = epc.Replace("0x", "");
            //string ean13 = "";
            //ean13 = this.GetDecimal(epc);
            ean = ean.Substring(1, 12);
            return ean + this.CalculateCheckDigit("EAN13", ean);

            //if (ean.StartsWith("0"))
            //{
            //}                
        }

        public string GetEAN14(string ean13)
        {
            string checkDigit = "";
            if (ean13 != "")
            {
                checkDigit = CalculateCheckDigit(ean13).ToString();
                return "0" + ean13 + checkDigit;
            }
            else
                return "";
        }

        /// <summary>
        /// Returns the EPC URI code (urn:epc:id:sgtin...)
        /// </summary>
        /// <param name="epc"></param>
        /// <param name="ean13"></param>
        /// <returns></returns>
        public string GetEPC(string epc, string ean13)
        {
            epc = epc.Replace("0x", "");
            epc = epc.Substring(0, 24);
            Int64 serialNumber;

            // make sure they gave us a barcode number
            if (ean13 != "")
            {
                // Add a zero to ean13 , so that it is in ean14
                if (ean13.Length == 13)
                    ean13 = "0" + ean13;

                serialNumber = BinaryToDecimal(HexToBin(epc.Substring(epc.Length - 8)));
                if (ean13.Length == 14)
                    return "urn:epc:id:sgtin:" + ean13.Substring(1, 7) + "." + ean13.Substring(0, 1) + ean13.Substring(8, 5) + "." + serialNumber;
                else
                    return "urn:epc:id:sgtin:" + ean13.Substring(1, 7) + "." + ean13.Substring(0, 1) + ean13.Substring(8, 5) + "." + serialNumber;
            }
            else
            {
                return "";
            }
        }
        #endregion


        #region Private methods

        private static bool IsHex(IEnumerable<char> chars)
        {
            bool isHex;
            foreach (var c in chars)
            {
                isHex = ((c >= '0' && c <= '9') ||
                         (c >= 'a' && c <= 'f') ||
                         (c >= 'A' && c <= 'F'));

                if (!isHex)
                    return false;
            }
            return true;
        }

        private string GetDecimal(string codigo)
        {
            if (codigo.Length >= 0)
            {
                string binaryEpc = HexToBin(codigo);
                string header = "";
                string itemIdentifier = "";

                int headerLength = 0;
                if (binaryEpc.Substring(0, 2).Equals("00"))
                {
                    headerLength = 8;
                }
                else
                {
                    headerLength = 2;
                }
                header = binaryEpc.Substring(0, headerLength);

                if (header == ("00110000"))
                {
                    // Retrieve the partition value from 11-13 bits of EPC and
                    // get corresponding company prefix length
                    Int64 partition = BinaryToDecimal(binaryEpc.Substring(11, 3));
                    int companyPrefixLengthBinary = GetCompanyPrefixLength(Convert.ToInt32(partition), CompanyPrefixLengthMode.Bits);
                    int companyPrefixLengthDecimal = GetCompanyPrefixLength(Convert.ToInt32(partition), CompanyPrefixLengthMode.Digits);

                    // company prefix lengh + item reference length = 44 bits -
                    // get length of item reference
                    int itemReferenceLenghtBinary = 44 - companyPrefixLengthBinary;
                    int itemReferenceLenghtDecimal = 13 - companyPrefixLengthDecimal;
                    string companyPrefix = BinaryToDecimal(binaryEpc.Substring(14, companyPrefixLengthBinary)).ToString().PadLeft(companyPrefixLengthDecimal, '0');
                    string itemReference = BinaryToDecimal(binaryEpc.Substring(14 + companyPrefixLengthBinary, itemReferenceLenghtBinary)).ToString().PadLeft(itemReferenceLenghtDecimal, '0');

                    // The first digit of item reference moves to the first position of EPC
                    //MARIO: LINEA COMENTADA
                    //itemIdentifier = itemReference.Substring(0, 1) + companyPrefix + itemReference.Substring(1, itemReference.Length - 1);
                    companyPrefix = "7702035";
                    itemReference = "4311";
                    itemIdentifier = companyPrefix + itemReference.Substring(1, itemReference.Length - 1);
                    string checkDigit = CalculateCheckDigit(itemIdentifier).ToString();

                    return itemIdentifier + checkDigit;
                }
                //ORIGINAL LINE: Case Else
                else
                {
                    //Cabecera EPC Inválida. Solamente se soporten cabeceras de 64 y 96 bit SSCC y SGTIN
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
        #endregion

        #region Encode
        /// <summary>
        /// Encodes a barcode into its EPC equivalent
        /// </summary>
        /// <param name="barcodeNumber">Original barcode</param>
        /// <param name="sourceType">Original barcode type</param>
        /// <param name="targetType">EPC encoding required</param>
        /// <param name="serialNumber">Serial Number</param>
        /// <returns>Encoded EPC</returns>    
        public string Encode(string barcodeNumber, EPCEncoding targetType, int companyPrefixLength, int filterValue, int serialNumber)
        {
            string epc = "";
            int partitionValue;
            string companyPrefix;
            string itemReference;
            string binaryCompanyPrefix;
            string binaryItemReference;
            string binaryHeader;
            string binaryFilterValue;
            string binaryPartitionValue;
            string binarySerialNumber;
            int bitCompanyPrefixLength;

            string companyPrefixIndex;

            switch (targetType)
            {
                // Logic for encoding to SGTIN96 - Check EPC standards reference for 
                // more information
                case EPCEncoding.SGTIN96:
                    //Si el codigo que le llega tiene solo 12 digitos le falta el digito de control
                    if (barcodeNumber.Length == 12)
                    {
                        string digito = this.CalculateCheckDigit("EAN13", barcodeNumber);
                        barcodeNumber = barcodeNumber + digito.ToString();
                    }

                    partitionValue = GetPartition(companyPrefixLength);
                    companyPrefix = barcodeNumber.Substring(0, companyPrefixLength);
                    itemReference = barcodeNumber.Substring(0, 1) + barcodeNumber.Substring(companyPrefixLength + 1, 12 - companyPrefixLength);
                    switch (partitionValue)
                    {
                        case 5:
                            itemReference = barcodeNumber.Substring(7, 5);
                            break;
                        case 4:
                            itemReference = barcodeNumber.Substring(8, 4);
                            break;
                        default:
                            itemReference = barcodeNumber.Substring(9, 3);
                            break;
                    }

                    binaryCompanyPrefix = DecimalToBinary(Convert.ToInt64(companyPrefix)).PadLeft
                        (GetCompanyPrefixLength(partitionValue, CompanyPrefixLengthMode.Bits), '0');
                    binaryItemReference = DecimalToBinary(Convert.ToInt64(itemReference)).PadLeft
                        (44 - GetCompanyPrefixLength(partitionValue, CompanyPrefixLengthMode.Bits), '0');
                    // SGTIN header is decimal 48
                    binaryHeader = "00110000";
                    binaryFilterValue = DecimalToBinary(Convert.ToInt64(filterValue)).PadLeft(3, '0');
                    binaryPartitionValue = DecimalToBinary(Convert.ToInt64(partitionValue)).PadLeft(3, '0');
                    binarySerialNumber = DecimalToBinary(serialNumber).PadLeft(38, '0');

                    epc = BinToHex(binaryHeader + binaryFilterValue + binaryPartitionValue +
                        binaryCompanyPrefix + binaryItemReference + binarySerialNumber);
                    break;
                case EPCEncoding.SGTIN64:
                    {
                        partitionValue = GetPartition(companyPrefixLength);
                        companyPrefix = barcodeNumber.Substring(1, companyPrefixLength);
                        companyPrefixIndex = GetCompanyPrefixIndex(companyPrefix);
                        itemReference = barcodeNumber.Substring(0, 1) +
                            barcodeNumber.Substring(companyPrefixLength + 1, 12 - companyPrefixLength);
                        string binaryCompanyPrefixIndex = DecimalToBinary(Convert.ToInt64(companyPrefixIndex)).PadLeft
                            (14, '0');
                        binaryItemReference = DecimalToBinary(Convert.ToInt64(itemReference)).PadLeft
                            (20, '0');
                        // SGTIN 64 header is decimal 2
                        binaryHeader = "10";
                        binaryFilterValue = DecimalToBinary(Convert.ToInt64(filterValue)).PadLeft(3, '0');
                        binarySerialNumber = DecimalToBinary(serialNumber).PadLeft(25, '0');

                        epc = BinToHex(binaryHeader + binaryFilterValue + binaryCompanyPrefixIndex +
                            binaryItemReference + binarySerialNumber);
                        break;
                    }
                case EPCEncoding.SSCC96:
                    partitionValue = GetPartition(companyPrefixLength);
                    companyPrefix = barcodeNumber.Substring(1, companyPrefixLength);

                    bitCompanyPrefixLength = GetCompanyPrefixLength(partitionValue, CompanyPrefixLengthMode.Bits);
                    binaryCompanyPrefix = DecimalToBinary(Convert.ToInt64(companyPrefix)).PadLeft
                        (bitCompanyPrefixLength, '0');

                    // SSCC header is decimal 49
                    binaryHeader = "00110001";
                    binaryFilterValue = DecimalToBinary(Convert.ToInt64(filterValue)).PadLeft(3, '0');
                    binaryPartitionValue = DecimalToBinary(Convert.ToInt64(partitionValue)).PadLeft(3, '0');
                    binarySerialNumber = DecimalToBinary(serialNumber).PadLeft(57 - bitCompanyPrefixLength, '0');

                    // SSCC encoding ends in 25 binary zeros
                    string binaryFiller = "0".PadLeft(25, '0');

                    epc = BinToHex(binaryHeader + binaryFilterValue + binaryPartitionValue +
                        binaryCompanyPrefix + binarySerialNumber + binaryFiller);
                    break;
                case EPCEncoding.SSCC64:
                    {
                        partitionValue = GetPartition(companyPrefixLength);
                        companyPrefix = barcodeNumber.Substring(1, companyPrefixLength);
                        companyPrefixIndex = GetCompanyPrefixIndex(companyPrefix);
                        binaryHeader = "00001000";
                        binaryFilterValue = DecimalToBinary(Convert.ToInt64(filterValue)).PadLeft(3, '0');
                        string binaryCompanyPrefixIndex = DecimalToBinary(Convert.ToInt64(companyPrefixIndex)).PadLeft
                            (14, '0');
                        binarySerialNumber = DecimalToBinary(serialNumber).PadLeft(39, '0');

                        epc = BinToHex(binaryHeader + binaryFilterValue + binaryCompanyPrefixIndex +
                            binarySerialNumber);
                        break;
                    }
                default:
                    throw new Exception("Invalid EPC Header. Currently supported header types are 64 and 96 bit SSCC and SGTIN");
                    break;
            }
            return epc;
        }

        /// <summary>
        /// Encodes a barcode into its EPC equivalent
        /// </summary>
        /// <param name="barcodeNumber">Original barcode</param>
        /// <param name="sourceType">Original barcode type</param>
        /// <param name="targetType">EPC encoding required</param>
        /// <param name="serialNumber">Serial Number</param>
        /// <returns>Encoded EPC</returns>    
        public string EncodeEAN14(string barcodeNumber, EPCEncoding targetType, int companyPrefixLength, int filterValue, int serialNumber)
        {
            string epc = "";
            int partitionValue;
            string companyPrefix;
            string itemReference;
            string binaryCompanyPrefix;
            string binaryItemReference;
            string binaryHeader;
            string binaryFilterValue;
            string binaryPartitionValue;
            string binarySerialNumber;
            int bitCompanyPrefixLength;

            string companyPrefixIndex;

            switch (targetType)
            {
                // Logic for encoding to SGTIN96 - Check EPC standards reference for 
                // more information
                case EPCEncoding.SGTIN96:
                    //Si el codigo que le llega tiene solo 12 digitos le falta el digito de control
                    if (barcodeNumber.Length == 13)
                    {
                        string digito = this.CalculateCheckDigit("EAN14", barcodeNumber);
                        barcodeNumber = barcodeNumber + digito.ToString();
                    }

                    partitionValue = GetPartition(companyPrefixLength);
                    companyPrefix = barcodeNumber.Substring(1, companyPrefixLength);
                    itemReference = barcodeNumber.Substring(1, 1) + barcodeNumber.Substring(companyPrefixLength + 1, 12 - companyPrefixLength);
                    switch (partitionValue)
                    {
                        case 5:
                            itemReference = barcodeNumber.Substring(8, 5);
                            break;
                        case 4:
                            itemReference = barcodeNumber.Substring(9, 4);
                            break;
                        default:
                            itemReference = barcodeNumber.Substring(10, 3);
                            break;
                    }

                    //EAN 14, se agrega el indicador de contenido.
                    itemReference = barcodeNumber.Substring(0, 1) + itemReference;

                    binaryCompanyPrefix = DecimalToBinary(Convert.ToInt64(companyPrefix)).PadLeft
                        (GetCompanyPrefixLength(partitionValue, CompanyPrefixLengthMode.Bits), '0');
                    binaryItemReference = DecimalToBinary(Convert.ToInt64(itemReference)).PadLeft
                        (44 - GetCompanyPrefixLength(partitionValue, CompanyPrefixLengthMode.Bits), '0');

                    // SGTIN header is decimal 48
                    binaryHeader = "00110000";
                    binaryFilterValue = DecimalToBinary(Convert.ToInt64(filterValue)).PadLeft(3, '0');
                    binaryPartitionValue = DecimalToBinary(Convert.ToInt64(partitionValue)).PadLeft(3, '0');
                    binarySerialNumber = DecimalToBinary(serialNumber).PadLeft(38, '0');

                    epc = BinToHex(binaryHeader + binaryFilterValue + binaryPartitionValue +
                        binaryCompanyPrefix + binaryItemReference + binarySerialNumber);
                    break;
                default:
                    throw new Exception("Invalid EPC Header. Currently supported header types are 64 and 96 bit SSCC and SGTIN");
                    break;
            }
            return epc;
        }

        #endregion

        #region Decode
        /// <summary>
        /// Decodes an EPC number to its UPC equivalent. Currently supports only GTIN
        /// </summary>
        /// <param name="hexEPC">Hexadecimal EPC encoded number</param>
        /// <param name="targetType">Required barcode type</param>
        /// <returns>Barcode</returns>    
        public string Decode(string hexEPC, BarcodeType targetType, out string errorMessage)
        {
            errorMessage = null;
            if (hexEPC.Length == 0)
            {
                errorMessage = "EPC cannot be blank";
                return null;
            }
            string binaryEpc = HexToBin(hexEPC);
            string header;
            string itemIdentifier;

            // If EPC begins with "00", header field is 8 bits, else 2 bits
            // Ref: EPCTagDataSpecification11rev124 Page 16
            int headerLength;
            if (binaryEpc.Substring(0, 2).Equals("00"))
            {
                headerLength = 8;
            }
            else
            {
                headerLength = 2;
            }

            header = binaryEpc.Substring(0, headerLength);
            switch (header)
            {
                case (SGTIN96Header):
                    {
                        // Retrieve the partition value from 11-13 bits of EPC and
                        // get corresponding company prefix length
                        Int64 partition = BinaryToDecimal(binaryEpc.Substring(11, 3));
                        int companyPrefixLengthBinary = GetCompanyPrefixLength(Convert.ToInt32(partition), CompanyPrefixLengthMode.Bits);
                        int companyPrefixLengthDecimal = GetCompanyPrefixLength(Convert.ToInt32(partition), CompanyPrefixLengthMode.Digits);

                        // company prefix lengh + item reference length = 44 bits -
                        // get length of item reference
                        int itemReferenceLenghtBinary = 44 - companyPrefixLengthBinary;
                        int itemReferenceLenghtDecimal = 13 - companyPrefixLengthDecimal;
                        string companyPrefix = BinaryToDecimal(binaryEpc.Substring(14, companyPrefixLengthBinary)).ToString().PadLeft(companyPrefixLengthDecimal, '0');
                        string itemReference = BinaryToDecimal(binaryEpc.Substring(14 + companyPrefixLengthBinary, itemReferenceLenghtBinary)).ToString().PadLeft(itemReferenceLenghtDecimal, '0');

                        // The first digit of item reference moves to the first position of EPC
                        itemIdentifier = itemReference.Substring(0, 1) + companyPrefix + itemReference.Substring(1, itemReference.Length - 1);
                        string checkDigit = CalculateCheckDigit(itemIdentifier).ToString(); ;
                        return itemIdentifier + checkDigit;
                    }
                case (SGTIN64Header):
                    {
                        // Retrieve the company prefix index from bits 5-19
                        string companyPrefixIndex = binaryEpc.Substring(5, 14);
                        string decimalCompanyPrefixIndex = BinaryToDecimal(companyPrefixIndex).ToString();
                        string companyPrefix = GetCompanyPrefix(decimalCompanyPrefixIndex);
                        int itemReferenceLength = 13 - companyPrefix.Length;
                        string itemReference = binaryEpc.Substring(19, 20);
                        string decimalItemReference = BinaryToDecimal(itemReference).ToString().PadLeft(itemReferenceLength, '0'); ;
                        string checkDigitCalculator = decimalItemReference.Substring(0, 1) +
                            companyPrefix + decimalItemReference.Substring(1, itemReferenceLength - 1);

                        string checkDigit = CalculateCheckDigit(checkDigitCalculator).ToString(); ;
                        return checkDigitCalculator + checkDigit;
                    }
                case (SSCC96Header):
                    errorMessage = "SSCC encoded tags cannot be decoded to UPC";
                    return null;
                default:
                    errorMessage = "Invalid EPC Header. Currently supported header types are 64 and 96 bit SSCC and SGTIN";
                    return null;
            }
        }
        #endregion

        #region GetEAN
        /// <summary>
        /// Get the EAN-13 notation of GTIN
        /// </summary>
        /// <param name="GTIN">GTIN identifier of item</param>
        /// <returns></returns>    
        public string GetEAN(string GTIN)
        {
            return GTIN.Substring(1, GTIN.Length - 1);
        }
        #endregion

        #region Private Methods
        #region HexToBin
        /// <summary>
        /// Convert a hexadecimal number to Binary
        /// </summary>
        /// <param name="hexNumber">Hexadecimal number</param>
        /// <returns>Binary representation as string</returns>
        private static string HexToBin(string hexNumber)
        {
            string inputNumber;
            string binaryNumber = "";

            //Pad input number with leading zeros to make number of digits as
            // multiple of 4
            if (hexNumber.Length % 4 != 0)
            {
                int paddedSize = Convert.ToInt32(hexNumber.Length / 4) * 4 + 4;
                inputNumber = hexNumber.PadLeft(paddedSize, '0');
            }
            else
            {
                inputNumber = hexNumber;
            }

            // Convert each digit of hexadecimal number to
            // equivalent binary
            foreach (char hexDigit in inputNumber)
            {
                string binaryBlock;
                switch (hexDigit)
                {
                    case '0':
                        binaryBlock = "0000";
                        break;
                    case '1':
                        binaryBlock = "0001";
                        break;
                    case '2':
                        binaryBlock = "0010";
                        break;
                    case '3':
                        binaryBlock = "0011";
                        break;
                    case '4':
                        binaryBlock = "0100";
                        break;
                    case '5':
                        binaryBlock = "0101";
                        break;
                    case '6':
                        binaryBlock = "0110";
                        break;
                    case '7':
                        binaryBlock = "0111";
                        break;
                    case '8':
                        binaryBlock = "1000";
                        break;
                    case '9':
                        binaryBlock = "1001";
                        break;
                    case 'A':
                        binaryBlock = "1010";
                        break;
                    case 'B':
                        binaryBlock = "1011";
                        break;
                    case 'C':
                        binaryBlock = "1100";
                        break;
                    case 'D':
                        binaryBlock = "1101";
                        break;
                    case 'E':
                        binaryBlock = "1110";
                        break;
                    case 'F':
                        binaryBlock = "1111";
                        break;
                    default:
                        string errorMessage = string.Format(INV_HEX_DIGIT, hexDigit);
                        throw new Exception(errorMessage);
                }
                binaryNumber += binaryBlock;
            }

            return binaryNumber;
        }
        #endregion

        #region BinToHex
        /// <summary>
        /// Convert a Binary number to hexadecimal
        /// </summary>
        /// <param name="binaryNumber">Binary number</param>
        /// <returns>Hexadecimal representation as string</returns>
        private static string BinToHex(string binaryNumber)
        {
            string inputNumber;
            string hexNumber = "";

            //Pad input number with leading zeros to make number of digits as
            // multiple of 4
            if (binaryNumber.Length % 4 != 0)
            {
                int paddedSize = Convert.ToInt32(binaryNumber.Length / 4) * 4 + 4;
                inputNumber = binaryNumber.PadRight(paddedSize, '0');
            }
            else
            {
                inputNumber = binaryNumber;
            }

            // Convert each digit of hexadecimal number to
            // equivalent binary
            for (int i = 0; i < inputNumber.Length; i = i + 4)
            {
                string binaryBlock = inputNumber.Substring(i, 4);
                string hexDigit;
                switch (binaryBlock)
                {
                    case "0000":
                        hexDigit = "0";
                        break;
                    case "0001":
                        hexDigit = "1";
                        break;
                    case "0010":
                        hexDigit = "2";
                        break;
                    case "0011":
                        hexDigit = "3";
                        break;
                    case "0100":
                        hexDigit = "4";
                        break;
                    case "0101":
                        hexDigit = "5";
                        break;
                    case "0110":
                        hexDigit = "6";
                        break;
                    case "0111":
                        hexDigit = "7";
                        break;
                    case "1000":
                        hexDigit = "8";
                        break;
                    case "1001":
                        hexDigit = "9";
                        break;
                    case "1010":
                        hexDigit = "A";
                        break;
                    case "1011":
                        hexDigit = "B";
                        break;
                    case "1100":
                        hexDigit = "C";
                        break;
                    case "1101":
                        hexDigit = "D";
                        break;
                    case "1110":
                        hexDigit = "E";
                        break;
                    case "1111":
                        hexDigit = "F";
                        break;
                    default:
                        string errorMessage = string.Format(INV_HEX_DIGIT, binaryBlock);
                        throw new Exception(errorMessage);
                }
                hexNumber += hexDigit;
            }

            return hexNumber;
        }
        #endregion

        #region BinaryToDecimal
        /// <summary>
        /// Converts binary number to decimal
        /// </summary>
        /// <param name="binaryNumber">Number in binary format</param>
        /// <returns>Decimal number</returns>
        private static Int64 BinaryToDecimal(string binaryNumber)
        {
            Int64 decimalNumber = 0;
            int lastIndex = binaryNumber.Length - 1;
            // Starting from the last digit, convert each character into
            // 1 or 0 and use following logic -
            // For a number N5 N4 N3 N2 N1 N0 (eg. 10110)
            // 2^5*N5 + 2^4*N4 + 2^3*N3 + 2^2N2 + 2^1*N1 + 2^0*N0
            for (int index = lastIndex; index >= 0; index--)
            {
                int binaryDigit = Convert.ToInt16(binaryNumber.Substring(index, 1));
                decimalNumber += Convert.ToInt64(Math.Pow(2, Convert.ToDouble(lastIndex - index)) * binaryDigit);
            }

            return decimalNumber;
        }
        #endregion

        #region BinaryToDecimalString
        /// <summary>
        /// Converts binary number to decimal
        /// </summary>
        /// <param name="binaryNumber">Number in binary format</param>
        /// <returns>Decimal number</returns>
        private static string BinaryToDecimalString(string binaryNumber, int digitLength)
        {
            Int64 decimalNumber = 0;
            int lastIndex = binaryNumber.Length - 1;
            // Starting from the last digit, convert each character into
            // 1 or 0 and use following logic -
            // For a number N5 N4 N3 N2 N1 N0 (eg. 10110)
            // 2^5*N5 + 2^4*N4 + 2^3*N3 + 2^2N2 + 2^1*N1 + 2^0*N0
            for (int index = lastIndex; index >= 0; index--)
            {
                int binaryDigit = Convert.ToInt16(binaryNumber.Substring(index, 1));
                decimalNumber += Convert.ToInt64(Math.Pow(2, Convert.ToDouble(lastIndex - index)) * binaryDigit);
            }

            return decimalNumber.ToString().PadLeft(digitLength, '0');
        }
        #endregion

        #region DecimalToBinary
        /// <summary>
        /// Converts decimal number to binary
        /// </summary>
        /// <param name="decimalNumber">Number in decimal format</param>
        /// <returns>Decimal number</returns>
        private static string DecimalToBinary(Int64 decimalNumber)
        {
            string convertedNumber = "";
            Int64 inputNumber = decimalNumber;
            int digit;
            // Convert the decimal to binary using standard coversion routine
            while (inputNumber > 0)
            {
                digit = Convert.ToInt32(inputNumber % 2);
                inputNumber = inputNumber / 2;
                convertedNumber += digit.ToString();
            }
            // Reverse the number so formed
            string outputBinaryNumber = "";
            for (int i = convertedNumber.Length - 1; i >= 0; i--)
            {
                outputBinaryNumber += convertedNumber[i];
            }
            return outputBinaryNumber;
        }

        private static string DecimalToBinary(Int64 decimalNumber, int length)
        {
            return DecimalToBinary(decimalNumber).PadLeft(length, '0');
        }
        #endregion

        #region GetCompanyPrefixLength
        /// <summary>
        /// Returns the length of company prefix (binary) based on partition value
        /// </summary>
        /// <param name="partitionValue">Partition Value from EPC number</param>
        /// <param name="mode">Indicator for Binary or Decimal length</param>
        /// <returns></returns>
        private static int GetCompanyPrefixLength(int partitionValue, CompanyPrefixLengthMode mode)
        {
            switch (mode)
            {
                case CompanyPrefixLengthMode.Bits:
                    switch (partitionValue)
                    {
                        case 0:
                            return 40;
                        case 1:
                            return 37;
                        case 2:
                            return 34;
                        case 3:
                            return 30;
                        case 4:
                            return 27;
                        case 5:
                            return 24;
                        case 6:
                            return 20;
                        default:
                            string errorMessage = string.Format(INV_EPC_PARTITION, partitionValue);
                            throw new Exception(errorMessage);
                    }
                case CompanyPrefixLengthMode.Digits:
                    switch (partitionValue)
                    {
                        case 0:
                            return 12;
                        case 1:
                            return 11;
                        case 2:
                            return 10;
                        case 3:
                            return 9;
                        case 4:
                            return 8;
                        case 5:
                            return 7;
                        case 6:
                            return 6;
                        default:
                            string errorMessage = string.Format(INV_EPC_PARTITION, partitionValue);
                            throw new Exception(errorMessage);
                    }
                default:
                    throw new Exception(INV_EPC_LENGTHMODE);
            }
        }
        #endregion

        #region GetPartition
        /// <summary>
        /// Returns the partition value based on length of company prefix
        /// </summary>
        /// <param name="companyPrefixLength">Length of company prefix</param>
        /// <returns>Partition Value</returns>
        private static int GetPartition(int companyPrefixLength)
        {
            switch (companyPrefixLength)
            {
                case 12:
                    return 0;
                case 11:
                    return 1;
                case 10:
                    return 2;
                case 9:
                    return 3;
                case 8:
                    return 4;
                case 7:
                    return 5;
                case 6:
                    return 6;
                case 5:
                    return 6;
                case 4:
                    return 6;
                case 3:
                    return 6;
                case 2:
                    return 6;
                case 1:
                    return 6;
                default:
                    string errorMessage = string.Format(INV_EPC_PARTITION);
                    throw new Exception(errorMessage);
            }
        }
        #endregion

        #region GetItemReferenceLength
        /// <summary>
        /// Returns the length of company prefix (binary) based on partition value
        /// </summary>
        /// <param name="partitionValue">Partition Value from EPC number</param>
        /// <param name="mode">Indicator for Binary or Decimal length</param>
        /// <returns></returns>
        private static int GetItemReferenceLength(int partitionValue, CompanyPrefixLengthMode mode)
        {
            switch (mode)
            {
                case CompanyPrefixLengthMode.Bits:
                    switch (partitionValue)
                    {
                        case 0:
                            return 4;
                        case 1:
                            return 7;
                        case 2:
                            return 10;
                        case 3:
                            return 14;
                        case 4:
                            return 17;
                        case 5:
                            return 20;
                        case 6:
                            return 24;
                        default:
                            string errorMessage = string.Format(INV_EPC_PARTITION, partitionValue);
                            throw new Exception(errorMessage);
                    }
                case CompanyPrefixLengthMode.Digits:
                    switch (partitionValue)
                    {
                        case 0:
                            return 1;
                        case 1:
                            return 2;
                        case 2:
                            return 3;
                        case 3:
                            return 4;
                        case 4:
                            return 5;
                        case 5:
                            return 6;
                        case 6:
                            return 7;
                        default:
                            string errorMessage = string.Format(INV_EPC_PARTITION, partitionValue);
                            throw new Exception(errorMessage);
                    }
                default:
                    throw new Exception(INV_EPC_LENGTHMODE);
            }
        }
        #endregion

        #region CalculateCheckDigit
        /// <summary>
        /// Calculate check digit for UPC
        /// </summary>
        /// <param name="UPC"></param>
        /// <returns></returns>
        public int CalculateCheckDigit(string UPC)
        {
            int checkDigit;
            int factor = 3;
            int sum = 0;
            for (int index = UPC.Length; index > 0; index--)
            {
                sum = sum + Convert.ToInt32(UPC.Substring(index - 1, 1)) * factor;
                factor = 4 - factor;
            }
            checkDigit = ((1000 - sum) % 10);

            return checkDigit;
        }
        #endregion

        #region GetCompanyPrefixIndex
        public static string GetCompanyPrefixIndex(string companyPrefix)
        {
            //SqlConnection connection = null;
            //string errorMessage;
            //string connectionString = ConfigurationSettings.AppSettings["EPCConnectionString"];

            //try
            //{
            //    connection = new SqlConnection(connectionString);
            //    connection.Open();
            //}
            //catch
            //{
            //    errorMessage = Constants.DB_NO_CONNECTION;
            //    throw new Exception(errorMessage);
            //}

            //object result;
            //try
            //{
            //    result = SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, "getCompanyPrefixIndex", new SqlParameter("@CompanyPrefix", companyPrefix));
            //}
            //catch (Exception exp)
            //{
            //    throw exp;
            //}
            //finally
            //{
            //    if (connection != null)
            //        connection.Dispose();
            //}


            //if (result == null)
            //{
            //    errorMessage = string.Format(Constants.DB_PRODUCT_NOT_FOUND, "Company Prefix", companyPrefix);
            //    throw new Exception(errorMessage);
            //}

            //return result.ToString();
            return "";
        }
        #endregion

        #region GetCompanyPrefix
        public static string GetCompanyPrefix(string companyprefixIndex)
        {
            //SqlConnection connection = null;
            //string errorMessage;
            //string connectionString = ConfigurationSettings.AppSettings["EPCConnectionString"];

            //try
            //{
            //    connection = new SqlConnection(connectionString);
            //    connection.Open();
            //}
            //catch
            //{
            //    errorMessage = Constants.DB_NO_CONNECTION;
            //    throw new Exception(errorMessage);
            //}

            //object result;
            //try
            //{
            //    result = SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, "getCompanyPrefix", new SqlParameter("@CompanyPrefixIndex", companyprefixIndex));
            //}
            //catch (Exception exp)
            //{
            //    throw exp;
            //}
            //finally
            //{
            //    if (connection != null)
            //        connection.Dispose();
            //}


            //if (result == null)
            //{
            //    errorMessage = string.Format(Constants.DB_PRODUCT_NOT_FOUND, "Company Prefix Index", companyprefixIndex);
            //    throw new Exception(errorMessage);
            //}

            //return result.ToString();
            return "";
        }
        #endregion
        #endregion

        #region Enumerators
        /// <summary>
        /// Barcode types
        /// </summary>
        public enum BarcodeType
        {
            UCC12 = 1,
            EAN13 = 2,
            GTIN = 3
        }

        /// <summary>
        /// EPC encoding types
        /// </summary>
        public enum EPCEncoding
        {
            Invalid = 0,    // used to flag an EPC that is not of a supported SGTIN format
            SGTIN96 = 48,
            SSCC96 = 49,
            SGTIN64 = 2,
            SSCC64 = 8
        }

        /// <summary>
        /// Type of merchandise - This is the filter value field in EPC
        /// and is used to identify the type of merchandise
        /// </summary>
        public enum MerchandiseType
        {
            AllOthers = 0,          // tags that don't fit other categories
            PosItem = 1,            // retail item for sale, from EAN/UPC
            TransportCase = 2,      // packed case for shipping
            Reserved1 = 3,          // reserved for future use
            ItemGrouping = 4,       // inner trade pack for handling
            Reserved2 = 5,          // reserved for furture use
            UnitLoad = 6,           // 
            InternalComponents = 7  // not intended for sale
        }

        /// <summary>
        /// Has two values - Bits and Digits denoting length of company prefix in binary
        /// or decimal mode
        /// </summary>
        public enum CompanyPrefixLengthMode
        {
            Bits = 1,
            Digits = 2
        }
        #endregion

        public string CalculateCheckDigit(string format, string code)
        {
            int CheckDigit = 0;
            string retVal = "";
            if (format == "EAN14")
            {
                //It verified that the length is 13 (code should come without check digit)
                if (code.Length == 13)
                {
                    //Loop 13 times, through all the code
                    int sum = 0;
                    int num = 0;
                    for (int i = 0; i < code.Length; i++)
                    {
                        num = Convert.ToInt32(code.Substring(i, 1));

                        //add/multiply barcode contents                  
                        if (i == 0)
                            sum = num * 3;
                        else
                        {
                            if (i % 2 == 0)
                                sum = sum + (num * 3);
                            else
                                sum = sum + num;
                        }
                    }

                    //Round up tot he nearest 10 (eg If the sum is 79, then round up to 80)
                    int decade = 0;
                    while (decade < sum)
                    {
                        decade += 10;
                    }

                    //Check digit
                    CheckDigit = decade - sum;
                    retVal = CheckDigit.ToString();
                }
            }
            else if (format == "EAN13")
            {
                // It verified that the length is 12 (code should come without check digit)
                if (code.Length == 12)
                {
                    // Loop 12 times, through all the code
                    int sum = 0;
                    int num = 0;
                    for (int i = 0; i < code.Length; i++)
                    {
                        num = Convert.ToInt32(code.Substring(i, 1));

                        // add/multiply barcode contents                       
                        if (i == 0)
                            sum = num;
                        else
                        {
                            if (i % 2 == 0)
                                sum = sum + num;
                            else
                                sum = sum + (num * 3);
                        }
                    }

                    //Round up tot he nearest 10 (eg If the sum is 79, then round up to 80)
                    int decade = 0;
                    while (decade < sum)
                    {
                        decade += 10;
                    }

                    //Digito de control
                    CheckDigit = decade - sum;
                    retVal = CheckDigit.ToString();
                }
            }

            return retVal;
        }
    }

    internal class SgtinInfo
    {
        public byte Header { get; set; }
        public byte Filter { get; set; }
        public byte Partition { get; set; }
        public string CompanyPrefix { get; set; }
        public string ItemReference { get; set; }
        public long SerialNumber { get; set; }
        public SgtinTagCoder.EPCEncoding Encoding { get; set; }
        public SgtinTagCoder.MerchandiseType Type { get; set; }
        public string Gtin { get; set; }
    }
}