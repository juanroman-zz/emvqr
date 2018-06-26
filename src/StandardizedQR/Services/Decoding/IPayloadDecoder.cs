using StandardizedQR.Utils;
using System.Collections.Generic;

namespace StandardizedQR.Services.Decoding
{
    internal interface IPayloadDecoder<T>
    {
        T BuildPayload(ICollection<Tlv> tlvs);

        ICollection<Tlv> DecodeQR(string qrData);

        string ValidateCrc(string qrData);
    }
}
