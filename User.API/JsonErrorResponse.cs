namespace User.API
{
    public class JsonErrorResponse
    {
        public JsonErrorResponse()
        {

        }
        public JsonErrorResponse(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
        public object DevelopMessage { get; set; }
    }
}