using Microsoft.AspNetCore.Mvc;
using SolForms.Extentions;
using SolForms.Models;
using SolForms.Models.Questions;
using SolForms.Services;

namespace SolFormsApi.Controllers
{
    [ApiController]
    [Route("SolForms/Options")]
    public class OptionController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ISolFormsService _service;
        public OptionController(ILogger<OptionController> logger, ISolFormsService service)
        {
            _logger = logger;
            _service = service;
        }

        //Get
        [HttpGet("{id:guid}")]
        public async Task<Option?> Get(Guid id) =>
            await _service.GetOption(id);

        [HttpGet("All/{questionId:guid}")]
        public async Task<Option?[]> GetByQuestionId(Guid questionId) =>
            await _service.GetOptions(questionId);

        [HttpGet("{id:guid}")]
        public async Task<bool> IsRedFlag(Guid optionId)
        {
            return await _service.IsRedFlag(optionId);
        }

        //Post
        [HttpPost]
        public async Task Create(Option option) =>
            await _service.CreateOption(option);

        [HttpPost("CreateOptionTemplte")]
        public async Task Create(string text) =>
            await _service.CreateOption(SolFormsFactory.CreateOption(text));


        //Put
        [HttpPut("{id:guid}")]
        public async Task Update(Guid id, Option option) =>
            await _service.UpdateOption(id, option);


        //Delete
        [HttpDelete("{id:guid}")]
        public async Task<bool> Delete(Guid id) =>
            await _service.DeleteOption(id);

    }
}
