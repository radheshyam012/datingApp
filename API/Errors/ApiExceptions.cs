using Microsoft.AspNetCore.Diagnostics;

namespace API.Errors
{
    public class ApiExceptions
    {
        public ApiExceptions(int statusCode, string message, string details)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }

        public int StatusCode { get; set; }
        public String Message { get; set; }
        public String Details { get; set; }


       
    }
}