using Arpl.Core;
using SampleWebApi.SampleApp.Domain.Errors;

namespace SampleWebApi.SampleApp.Domain.Model
{
    public class Person
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public int Age { get; private set; }

        private Person(string name, int age)
        {
            Id = Guid.NewGuid();
            Name = name;
            Age = age;
        }

        public static Either<Error, Person> Create(string name, int age)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Fail<Person>(ValidateError.New("Name cannot be empty", "VAL001"));

            if (age <= 18)
                return Fail<Person>(ValidateError.New("Age must be greater than 18", "VAL002"));

            return Success(new Person(name, age));
        }

    }
}
