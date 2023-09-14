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
        private readonly SFService _service;
        public OptionController(ILogger<OptionController> logger, SFService service)
        {
            _logger = logger;
            _service = service;
        }

        //Get
        [HttpGet("{id:guid}")]
        public async Task<SFOption?> Get(Guid id) =>
            await _service.GetOption(id);

        [HttpGet("All/{questionId:guid}")]
        public async Task<SFOption?[]> GetByQuestionId(Guid questionId) =>
            await _service.GetOptions(questionId);

        //Post
        [HttpPost]
        public async Task Create(SFOption option) =>
            await _service.CreateOption(option);

        [HttpPost("CreateOptionTemplte")]
        public async Task Create(string text) =>
            await _service.CreateOption(SolFormsFactory.CreateOption(text));


        //Put
        [HttpPut("{id:guid}")]
        public async Task Update(Guid id, SFOption option) =>
            await _service.UpdateOption(id, option);


        //Delete
        [HttpDelete("{id:guid}")]
        public async Task<bool> Delete(Guid id) =>
            await _service.DeleteOption(id);

    }
}
