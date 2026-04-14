using Microsoft.AspNetCore.Mvc;

namespace Backend.Models
{
    public class ApiResult<T>
    {
        public T? Data { get; set; }
        public bool IsSuccessful { get; set; }
        public IEnumerable<ProblemDetails>? Errors { get; set; }
        public int StatusCode { get; set; }
    }
}
