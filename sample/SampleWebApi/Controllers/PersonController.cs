using Microsoft.AspNetCore.Mvc;
using SampleWebApi.Controllers.Dtos;
using SampleWebApi.Controllers.Results;
using SampleWebApi.SampleApp.Application;
using SampleWebApi.SampleApp.Application.Dtos;

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
        public async Task<HttpResultDto<IEnumerable<PersonDto>>> GetAll()
        {
            var result = await _personService.GetAllAsync();
            return HttpResult.Handle(result);
        }

        [HttpGet("{id}")]
        public async Task<HttpResultDto<PersonDto>> GetById(Guid id)
        {
            var result = await _personService.GetByIdAsync(id);
            return HttpResult.Handle(result);
        }

        [HttpPost]
        public async Task<HttpResultDto<PersonDto>> Create([FromBody] CreatePersonDto dto)
        {
            var result = await _personService.CreateAsync(dto);
            return HttpResult.Handle(result);
        }
    }
}
