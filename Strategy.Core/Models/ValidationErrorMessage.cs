using System;

namespace Strategy.Core.Models
{
    public class ValidationErrorMessage : ErrorMessage
    {
        public DateTime Data { get; set; }

        public ValidationErrorMessage(string message, DateTime data)
        {
            Message = message;
            Data = data;
        }
    }
}
