using Arpl.Core;
using SampleWebApi.SampleApp.Application.Dtos;
using SampleWebApi.SampleApp.Domain.Model;

namespace SampleWebApi.SampleApp.Application
{
    public class PersonService
    {
        private readonly IPersonRepository _repository;

        public PersonService(IPersonRepository repository)
        {
            _repository = repository;
        }

        public async Task<SResult<PersonDto>> GetByIdAsync(Guid id)
        {
            var result = await _repository.GetByIdAsync(id);

            return result.Map(PersonDto.ToDto);
        }

        public async Task<SResult<IEnumerable<PersonDto>>> GetAllAsync()
        {
            var result = await _repository.GetAllAsync();

            return result.Map(people => people.Select(PersonDto.ToDto));
        }

        public async Task<SResult<PersonDto>> CreateAsync(CreatePersonDto dto)
        {
            var eitherPerson = Person.Create(dto.Name, dto.Age);

            return await eitherPerson.BindAsync(async person => await _repository.CreateAsync(person))
                                     .MapAsync(m => Task.FromResult(PersonDto.ToDto(m)));
                               
                               
        }
    }
}
