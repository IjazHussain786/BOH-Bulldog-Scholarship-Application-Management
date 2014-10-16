namespace BohFoundation.WebApi.Models
{
    public class ServerMessage
    {
        public ServerMessage(object objectToSend)
        {
            Data = objectToSend;
        }

        public ServerMessage()
        {
        }

        public object Data { get; set; }
    }
}