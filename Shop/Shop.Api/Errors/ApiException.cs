
namespace Shop.Api.Errors
{
    public class ApiException : ApiResponse
    {
        public ApiException(int statusCode, string message = null, string details = null) 
            : base(statusCode, message)
        {
            Details = details;
        }

        //for stack trace
        public string Details { get; set; }
    }
}
