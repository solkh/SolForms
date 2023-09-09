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
        private readonly ISolFormsService _service;
        public AnswerController(ILogger<AnswerController> logger, ISolFormsService service)
        {
            _logger = logger;
            _service = service;
        }
        //Get
        [HttpGet("{id:guid}")]
        public async Task<Answer?> Get(Guid id) =>
            await _service.GetAnswer(id);

        [HttpGet("All/{sessionId:guid}")]
        public async Task<Answer?[]> GetBySessionId(Guid sessionId) =>
            await _service.GetAnswers(sessionId);


        //Post
        [HttpPost]
        public async Task Create(Answer answer) =>
            await _service.CreateAnswer(answer);


        //Put
        [HttpPut("{id:guid}")]
        public async Task Update(Guid id, Answer answer) =>
            await _service.UpdateAnswer(id, answer);


        //Delete
        [HttpDelete("{id:guid}")]
        public async Task<bool> Delete(Guid id) =>
            await _service.DeleteAnswer(id);
    }
}
