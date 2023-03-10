using Newtonsoft.Json;

namespace Api.Model;

public class SocketResponse
{
    public string Message { get; set; }

    public bool Success { get; set; }

    public SocketResponse(bool success, string message)
    {
        Success = success;
        Message = message;
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}