using Microsoft.AspNetCore.Mvc;
using SolForms.Extentions;
using SolForms.Models;
using SolForms.Models.Questions;
using SolForms.Services;

namespace SolFormsApi.Controllers
{
    [ApiController]
    [Route("SolForms/Answers")]
    public class AnswerController : ControllerBase
    {
        private readonly ILogger<AnswerController> _logger;
        private readonly SFService _service;
        public AnswerController(ILogger<AnswerController> logger, SFService service)
        {
            _logger = logger;
            _service = service;
        }
        //Get
        [HttpGet("{id:guid}")]
        public async Task<SFAnswer?> Get(Guid id) =>
            await _service.GetAnswer(id);

        [HttpGet("All/{sessionId:guid}")]
        public async Task<SFAnswer?[]> GetBySessionId(Guid sessionId) =>
            await _service.GetAnswers(sessionId);


        //Post
        [HttpPost]
        public async Task Create(SFAnswer answer) =>
            await _service.CreateAnswer(answer);


        //Put
        [HttpPut("{id:guid}")]
        public async Task Update(Guid id, SFAnswer answer) =>
            await _service.UpdateAnswer(id, answer);


        //Delete
        [HttpDelete("{id:guid}")]
        public async Task<bool> Delete(Guid id) =>
            await _service.DeleteAnswer(id);
    }
}
