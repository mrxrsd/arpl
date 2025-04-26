using Arpl.Core;
using SampleWebApi.SampleApp.Domain.Errors;
using SampleWebApi.SampleApp.Domain.Model;

namespace SampleWebApi.SampleApp.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private static readonly List<Person> _people = new();

        public async Task<Either<Error, Person>> GetByIdAsync(Guid id)
        {

            try
            {
                var person = _people.FirstOrDefault(p => p.Id == id);

                if (person == null)
                    return Fail<Person>(NotFoundError.New($"Person with id {id} not found"));

                return Success(person);

            }
            catch (Exception ex)
            {
                // Log Error
                return Fail<Person>(Errors.New(ex));
            }
        }

        public async Task<Either<Error, IEnumerable<Person>>> GetAllAsync()
        {
            try
            {
                var people = _people.AsEnumerable();

                return Success(people);
            }
            catch (Exception ex)
            {
                // Log Error
                return Fail<IEnumerable<Person>>(Errors.New(ex));
            }
        }

        public async Task<Either<Error, Person>> CreateAsync(Person person)
        {
            try
            {
                _people.Add(person);

                return Success(person);
            }
            catch (Exception ex)
            {
                // Log Error
                return Fail<Person>(Errors.New(ex));
            }
        }
    }
}
