namespace Arpl.Core
{
    public record ErrorCollection : Error
    {

        public ErrorCollection()
        {
            Errors = new List<Error>();
        }

        public ErrorCollection(IEnumerable<Error> errors)
        {
            Errors = errors.ToList();
        }

        public List<Error> Errors { get; }

        public override string Code => $"[{String.Join(", ", Errors.Select(x=> x.Code))}]";
        public override string Message => $"[{String.Join(", ", Errors.Select(x => x.Message))}]";

        public override int Count => Errors.Count;

        public override bool IsExpected => Errors.All(x => x.IsExpected);

        public override IEnumerable<Error> AsEnumerable()
        {
            return Errors;
        }


    }
}