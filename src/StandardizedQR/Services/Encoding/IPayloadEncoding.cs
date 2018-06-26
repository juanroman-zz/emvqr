namespace StandardizedQR.Services.Encoding
{
    public interface IPayloadEncoding<T>
    {
        string GeneratePayload(T instance);
    }
}
