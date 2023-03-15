using Newtonsoft.Json;

namespace Een.Socket.Responses;

public class SocketResponse
{   
    #region Properties

    public string Message { get; set; }

    public bool Success { get; set; }
    

    #endregion

    #region Constructor

    public SocketResponse(bool success, string message)
    {
        Success = success;
        Message = message;
    }

    #endregion

    #region Public Methods

    
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
    #endregion




}