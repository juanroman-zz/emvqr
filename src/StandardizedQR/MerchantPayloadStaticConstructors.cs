using StandardizedQR.Services.Decoding;
using System;
using System.ComponentModel.DataAnnotations;

namespace StandardizedQR
{
    public partial class MerchantPayload
    {
        /// <summary>
        /// Creates the static merchant-presented QR with mandatory fields.
        /// </summary>
        /// <param name="merchantGlobalUniqueIdentifier">The merchant global unique identifier.</param>
        /// <param name="merchantCategoryCode">The merchant category code.</param>
        /// <param name="transactionCurrencyNumericCode">The transaction currency numeric code.</param>
        /// <param name="countryCode">The country code.</param>
        /// <param name="merchantName">Name of the merchant.</param>
        /// <param name="merchantCity">The merchant city.</param>
        /// <returns>
        /// A new instance of the <see cref="MerchantPayload"/> class.
        /// </returns>
        public static MerchantPayload CreateStatic(string merchantGlobalUniqueIdentifier, int merchantCategoryCode, int transactionCurrencyNumericCode, string countryCode, string merchantName, string merchantCity)
        {
            return new MerchantPayload
            {
                PayloadFormatIndicator = 1,
                PointOfInitializationMethod = 11,
                MerchantCategoryCode = merchantCategoryCode,
                TransactionCurrency = transactionCurrencyNumericCode,
                CountyCode = countryCode,
                MerchantName = merchantName,
                MerchantCity = merchantCity,
                MerchantAccountInformation = new MerchantAccountInformationDictionary
                {
                    {26, new MerchantAccountInformation { GlobalUniqueIdentifier = merchantGlobalUniqueIdentifier} }
                }
            };
        }

        /// <summary>
        /// Creates the dynamic merchant-presented QR with mandatory fields.
        /// </summary>
        /// <param name="merchantGlobalUniqueIdentifier">The merchant global unique identifier.</param>
        /// <param name="merchantCategoryCode">The merchant category code.</param>
        /// <param name="transactionCurrencyNumericCode">The transaction currency numeric code.</param>
        /// <param name="countryCode">The country code.</param>
        /// <param name="merchantName">Name of the merchant.</param>
        /// <param name="merchantCity">The merchant city.</param>
        /// <returns>
        /// A new instance of the <see cref="MerchantPayload"/> class.
        /// </returns>
        public static MerchantPayload CreateDynamic(string merchantGlobalUniqueIdentifier, int merchantCategoryCode, int transactionCurrencyNumericCode, string countryCode, string merchantName, string merchantCity)
        {
            return new MerchantPayload
            {
                PayloadFormatIndicator = 1,
                PointOfInitializationMethod = 12,
                MerchantCategoryCode = merchantCategoryCode,
                TransactionCurrency = transactionCurrencyNumericCode,
                CountyCode = countryCode,
                MerchantName = merchantName,
                MerchantCity = merchantCity,
                MerchantAccountInformation = new MerchantAccountInformationDictionary
                {
                    {26, new MerchantAccountInformation { GlobalUniqueIdentifier = merchantGlobalUniqueIdentifier} }
                }
            };
        }

        /// <summary>
        /// Decodes QR data into a <see cref="MerchantPayload"/> instance.
        /// </summary>
        /// <param name="qrData">The qr data.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="qrData"/> is <c>null</c> or an empty string.</exception>
        /// <exception cref="System.Security.SecurityException">If the CRC of the QR is invalid.</exception>
        /// <exception cref="ValidationException">If the payload is invalid.</exception>
        public static MerchantPayload FromQR(string qrData)
        {
            if (string.IsNullOrWhiteSpace(qrData))
            {
                throw new ArgumentNullException(nameof(qrData));
            }

            var merchantDecoder = new MerchantDecoder();
            var crc = merchantDecoder.ValidateCrc(qrData);
            var tlvs = merchantDecoder.DecodeQR(qrData);
            var payload = merchantDecoder.BuildPayload(tlvs);
            payload.CRC = crc;

            var validationContext = new ValidationContext(payload);
            Validator.ValidateObject(payload, validationContext, true);

            return payload;
        }
    }
}
