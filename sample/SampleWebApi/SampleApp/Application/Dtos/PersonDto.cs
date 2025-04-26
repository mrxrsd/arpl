using SampleWebApi.SampleApp.Domain.Model;

namespace SampleWebApi.SampleApp.Application.Dtos
{
    public class PersonDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public static PersonDto ToDto(Person person) =>
            new() { Id = person.Id, Name = person.Name, Age = person.Age };
    }

    public class CreatePersonDto
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class UpdatePersonDto
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
