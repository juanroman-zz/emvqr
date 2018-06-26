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

To see the full spec reference please read the specification provided by EMV(R).

In order to create a _dynamic_ qr code with a Transaction Amount you could do the following:

```csharp
var globalUniqueIdentifier = Guid.NewGuid().ToString().Replace("-", string.Empty);

var merchantPayload = MerchantPayload.CreateDynamic(
    globalUniqueIdentifier,
    4111,
    Iso4217Currency.MexicoPeso.Value.NumericCode,
    Iso3166Countries.Mexico,
    "My Super Shop",
    "Mexico City")
    .WithTransactionAmount(100);

var payload = merchantPayload.GeneratePayload();
```
