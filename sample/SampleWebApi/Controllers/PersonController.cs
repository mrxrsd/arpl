using Microsoft.AspNetCore.Mvc;
using SampleWebApi.SampleApp.Application;
using SampleWebApi.SampleApp.Application.Dtos;
using Arpl.AspNetCore.Extensions;

namespace SampleWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly PersonService _personService;

        public PersonController(PersonService personService)
        {
            _personService = personService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonDto>>> GetAll()
        {
            return await _personService.GetAllAsync().ToActionResultAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PersonDto>> GetById(Guid id)
        {
            return await _personService.GetByIdAsync(id).ToActionResultAsync();
        }

        [HttpPost]
        public async Task<ActionResult<PersonDto>> Create([FromBody] CreatePersonDto dto)
        {
            return await _personService.CreateAsync(dto).ToActionResultAsync();
        }
    }
}
