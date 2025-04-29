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
        /// Gets an AggregateException containing all exceptions from the errors in this collection.
        /// Returns null if no errors have exceptions.
        /// </summary>
        public override Exception? Exception
        {
            get
            {
                var exceptions = Errors
                    .Select(error => error.Exception)
                    .Where(ex => ex != null)
                    .ToList();

                return exceptions.Count > 0
                    ? new AggregateException(exceptions)
                    : null;
            }
        }

        /// <summary>
        /// Checks if this error collection contains an error of the specified type T.
        /// </summary>
        /// <typeparam name="T">The type of error to check for.</typeparam>
        /// <returns>True if the collection contains at least one error of type T, false otherwise.</returns>
        public override bool HasErrorOf<T>() 
        {
            return Errors.Any(error => error is T);
        }

        /// <summary>
        /// Returns an enumerable of all errors in this collection.
        /// </summary>
        /// <returns>An IEnumerable containing all errors in this collection.</returns>
        public override IEnumerable<Error> AsEnumerable()
        {
            return Errors;
        }

        /// <summary>
        /// Adds an error to this collection.
        /// </summary>
        /// <param name="error">The error to add to the collection. If null, the error will be ignored.</param>
        public void Add(Error error)
        {
            if (error != null)
            {
                Errors.Add(error);
            }
        }
    }
}