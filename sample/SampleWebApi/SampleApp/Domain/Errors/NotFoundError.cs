using Arpl.Core;

namespace SampleWebApi.SampleApp.Domain.Errors
{
    public record NotFoundError : ExpectedError
    {
        public NotFoundError(string message, string code = "0") : base(message, code)
        {
        }

        public static NotFoundError New(string message, string code = "NOT_FOUND")
        {
            return new NotFoundError(message, code);
        }
    }
}
