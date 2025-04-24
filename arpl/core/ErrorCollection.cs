namespace Arpl.Core
{
    /// <summary>
    /// Represents a collection of errors that can be composed and aggregated.
    /// This class allows multiple errors to be grouped together and treated as a single error.
    /// </summary>
    public record ErrorCollection : Error
    {

        /// <summary>
        /// Initializes a new instance of the ErrorCollection class with an empty list of errors.
        /// </summary>
        public ErrorCollection()
        {
            Errors = new List<Error>();
        }

        /// <summary>
        /// Initializes a new instance of the ErrorCollection class with the specified collection of errors.
        /// </summary>
        /// <param name="errors">The collection of errors to initialize with.</param>
        public ErrorCollection(IEnumerable<Error> errors)
        {
            Errors = errors.ToList();
        }

        /// <summary>
        /// Gets the list of errors contained in this collection.
        /// </summary>
        public List<Error> Errors { get; }

        /// <summary>
        /// Gets a string representation of all error codes in the collection, joined by commas and enclosed in brackets.
        /// </summary>
        public override string Code => $"[{String.Join(", ", Errors.Select(x=> x.Code))}]";

        /// <summary>
        /// Gets a string representation of all error messages in the collection, joined by commas and enclosed in brackets.
        /// </summary>
        public override string Message => $"[{String.Join(", ", Errors.Select(x => x.Message))}]";

        /// <summary>
        /// Gets the total number of errors in this collection.
        /// </summary>
        public override int Count => Errors.Count;

        /// <summary>
        /// Gets a value indicating whether all errors in this collection are expected errors.
        /// Returns true only if all contained errors are expected.
        /// </summary>
        public override bool IsExpected => Errors.All(x => x.IsExpected);

        /// <summary>
        /// Returns an enumerable of all errors in this collection.
        /// </summary>
        /// <returns>An IEnumerable containing all errors in this collection.</returns>
        public override IEnumerable<Error> AsEnumerable()
        {
            return Errors;
        }


    }
}