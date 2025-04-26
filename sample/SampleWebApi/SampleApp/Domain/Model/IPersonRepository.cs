using Arpl.Core;

namespace SampleWebApi.SampleApp.Domain.Model
{
    public interface IPersonRepository
    {
        Task<Either<Error, Person>> GetByIdAsync(Guid id);
        Task<Either<Error, IEnumerable<Person>>> GetAllAsync();
        Task<Either<Error, Person>> CreateAsync(Person person);
     
    }
}
