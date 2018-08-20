# EMV Complaint QR Codes

[![NuGet](https://img.shields.io/nuget/v/StandardizedQR.svg?label=NuGet)](https://www.nuget.org/packages/StandardizedQR/)

[![Build status](https://ci.appveyor.com/api/projects/status/wt1h1vpvec4mcfqc?svg=true)](https://ci.appveyor.com/project/juanroman/emvqr)

EMV(R) Compliant Library built in .NET Standard for generating and parsing QR Codes. The library is based on the **EMV(R) QR Code Specification for Payment Systems: Merchant-Presented Mode _Version 1_**.

## Creating a Merchant-Presented QR
To create a static (_used for many transactions_) you can use the static constructor

```csharp
var globalUniqueIdentifier = Guid.NewGuid().ToString().Replace("-", string.Empty);
var merchantPayload = MerchantPayload.CreateStatic(
    merchantGlobalUniqueIdentifier: globalUniqueIdentifier,
    merchantCategoryCode: 4111,
    transactionCurrencyNumericCode: Iso4217Currency.MexicoPeso.Value.NumericCode,
    countryCode: Iso3166Countries.Mexico,
    merchantName: "My Super Shop",
    merchantCity: "Mexico City");

var payload = merchantPayload.GeneratePayload();
```

The above would produce some of the following fields.

Value String | Field Reference | Spec ID | Value Length | Spec Value | Explanation
------------ | --------------- | ------- | ----------- | ---------- | -----------
000201 | Payload Format Indicator | "00" | 2 | 1 | The Payload Format Indicator shall contain a value of "01"
000211 | Point of Initiation Method | "01" | 2 | 11 | The value of "11" should be used when the same QR Code is shown for more than one transaction
52044111 | Merchant Category Code | "52" | 4 | 4111 | The Merchant Category Code (MCC) shall contain an MCC as defined by **ISO 18245**.
6011Mexico City | Merchant City | "60" | 11 | Mexico City | The Merchant City should indicate the city of the merchant's physical location.

To see the full spec reference please read the specification provided by EMV(R) or read it from the [file included](../master/specs/EMVCo-Merchant-Presented-QR-Specification-v1-1.pdf)

You can also go full fluent. The following example shows most of the fields used with the easier fluent approach:

```csharp
var globalUniqueIdentifier = Guid.NewGuid().ToString().Replace("-", string.Empty);

var merchantPayload = MerchantPayload
    .CreateDynamic(globalUniqueIdentifier, 4111, Iso4217Currency.MexicoPeso.Value.NumericCode, Iso3166Countries.Mexico, "Chocolate Powder", "Mexico City")
    .WithAlternateLanguage(Iso639Languages.SpanishCastilian, "Chocolate en Polvo", "CDMX")
    .WithTransactionAmount(34.95m)
    .WithTipByUser()
    .WithAdditionalData(
        billNumber: "1234",
        mobileNumber: "5512341234",
        storeLabel: "The large store",
        loyaltyNumber: "A12341234",
        referenceLabel: "***",
        terminalLabel: "T12341",
        purposeOfTransaction: "We do commerce",
        additionalConsumerDataRequest: "AME")
    .WithUnreservedTemplate(globalUniqueIdentifier, new Dictionary<int, string>
    {
        {1, "Some value" },
        {2, "Another value" }
    });

var payload = merchantPayload.GeneratePayload();
```

## Decoding a Merchant-Presented QR
You can easily decode a QR code by using the **FromQR** static constructor. Please note that the data will be validated against the spec automatically.

```csharp
var merchantPayload = MerchantPayload.FromQR(qr);
```

### Sample
The following sample which is included in the Unit Tests shows how to generate the QR data and then decode it back to an object.
```csharp
var globalUniqueIdentifier = Guid.NewGuid().ToString().Replace("-", string.Empty);
var merchantPayload = MerchantPayload
    .CreateDynamic(globalUniqueIdentifier, 4111, Iso4217Currency.MexicoPeso.Value.NumericCode, Iso3166Countries.Mexico, "Chocolate Powder", "Mexico City")
    .WithAlternateLanguage(Iso639Languages.SpanishCastilian, "Chocolate en Polvo", "CDMX")
    .WithTransactionAmount(34.95m)
    .WithTipByUser()
    .WithAdditionalData(
        billNumber: "1234",
        mobileNumber: "5512341234",
        storeLabel: "The large store",
        loyaltyNumber: "A12341234",
        referenceLabel: "***",
        terminalLabel: "T12341",
        purposeOfTransaction: "We do commerce",
        additionalConsumerDataRequest: "AME")
    .WithUnreservedTemplate(globalUniqueIdentifier, new Dictionary<int, string>
    {
        {1, "Some value" },
        {2, "Another value" }
    });
merchantPayload.PostalCode = "12345";

var qr = merchantPayload.GeneratePayload();

merchantPayload = MerchantPayload.FromQR(qr);
Assert.Equal(globalUniqueIdentifier, merchantPayload.MerchantAccountInformation.First().Value.GlobalUniqueIdentifier);
Assert.Equal(4111, merchantPayload.MerchantCategoryCode);
Assert.Equal(Iso4217Currency.MexicoPeso.Value.NumericCode, merchantPayload.TransactionCurrency);
Assert.Equal(Iso3166Countries.Mexico, merchantPayload.CountyCode);
Assert.Equal("Chocolate Powder", merchantPayload.MerchantName);
Assert.Equal("Mexico City", merchantPayload.MerchantCity);
Assert.Equal(Iso639Languages.SpanishCastilian, merchantPayload.MerchantInformation.LanguagePreference);
Assert.Equal("Chocolate en Polvo", merchantPayload.MerchantInformation.MerchantNameAlternateLanguage);
Assert.Equal("CDMX", merchantPayload.MerchantInformation.MerchantCityAlternateLanguage);
Assert.Equal(34.95m, merchantPayload.TransactionAmount);
Assert.Equal(1, merchantPayload.TipOrConvenienceIndicator);
Assert.Equal("1234", merchantPayload.AdditionalData.BillNumber);
Assert.Equal("5512341234", merchantPayload.AdditionalData.MobileNumber);
Assert.Equal("The large store", merchantPayload.AdditionalData.StoreLabel);
Assert.Equal("A12341234", merchantPayload.AdditionalData.LoyaltyNumber);
Assert.Equal("***", merchantPayload.AdditionalData.ReferenceLabel);
Assert.Equal("T12341", merchantPayload.AdditionalData.TerminalLabel);
Assert.Equal("We do commerce", merchantPayload.AdditionalData.PurposeOfTransaction);
Assert.Equal("AME", merchantPayload.AdditionalData.AdditionalConsumerDataRequest);
Assert.Equal(globalUniqueIdentifier, merchantPayload.UnreservedTemplate.First().Value.GlobalUniqueIdentifier);
Assert.Equal("Some value", merchantPayload.UnreservedTemplate.First().Value.ContextSpecificData[1]);
Assert.Equal("Another value", merchantPayload.UnreservedTemplate.First().Value.ContextSpecificData[2]);
Assert.Equal("12345", merchantPayload.PostalCode);
Assert.NotNull(merchantPayload.CRC);
```
