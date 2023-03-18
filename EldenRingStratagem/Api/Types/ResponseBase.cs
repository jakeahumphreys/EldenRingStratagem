namespace EldenRingStratagem.Api.Types;

public class ResponseBase
{
    public Error Error { get; set; }
    
    public T WithError<T>(Error error) where T : ResponseBase
    {
        Error = error;

        return (T) this;
    }
}