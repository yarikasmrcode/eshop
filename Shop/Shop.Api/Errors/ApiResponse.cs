namespace Shop.Api.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        //string is null cuz we may not have a message related to a response
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessage(statusCode);
        }

        private string GetDefaultMessage(int statusCode)
        {
            return statusCode switch
            {
                400 => "A Bad Request Was Made",
                401 => "You Are Not Authorized",
                404 => "Resource Was Not Found",
                500 => "Server Error Was Made. Sorry",
                _ => null
            };
        }
    }
}
