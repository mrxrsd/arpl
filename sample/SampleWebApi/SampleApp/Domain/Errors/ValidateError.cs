using Arpl.Core;

namespace SampleWebApi.SampleApp.Domain.Errors
{
    public record ValidateError : ExpectedError
    {
        public ValidateError(string message, string code = "0") : base(message, code)
        {
        }

        public static ValidateError New(string message, string code = "VAL000")
        {
            return new ValidateError(message, code);
        }
    }
}
