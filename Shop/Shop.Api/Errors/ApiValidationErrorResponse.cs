namespace Shop.Api.Errors
{
    public class ApiValidationErrorResponse : ApiResponse
    { 
        public ApiValidationErrorResponse() : base(404)
        {

        }

        public IEnumerable<string> Errors { get; set; }
    }
}
