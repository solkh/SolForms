using Microsoft.AspNetCore.Mvc;
using SolForms.Extentions;
using SolForms.Models;
using SolForms.Models.Enums;
using SolForms.Models.Questions;
using SolForms.Services;

namespace SolFormsApi.Controllers
{
    [ApiController]
    [Route("SolForms/Questions")]
    public class QuestionController : ControllerBase
    {
        private readonly ILogger<BaseQuestion> _logger;
        private readonly ISolFormsService _service;
        public QuestionController(ILogger<BaseQuestion> logger, ISolFormsService service)
        {
            _logger = logger;
            _service = service;
        }

        //Get
        [HttpGet("{id:guid}")]
        public async Task<BaseQuestion?> Get(Guid id) =>
            await _service.GetQuestion(id);

        [HttpGet("All/{sectionId:guid}")]
        public async Task<BaseQuestion?[]> GetBySectionId(Guid sectionId) =>
            await _service.GetQuestions(sectionId);            


        //Post
        [HttpPost]
        public async Task Create(BaseQuestion question) =>
            await _service.CreateQuestion(question);

        [HttpPost("CreateQuestionTemplte")]
        public async Task Create(string text, QuestionType type) =>
            await _service.CreateQuestion(SolFormsFactory.CreateQuestion(text, type));


        //Put
        [HttpPut("{id:guid}")]
        public async Task Update(Guid id, BaseQuestion question) =>
            await _service.UpdateQuestion(id, question);


        //Delete
        [HttpDelete("{id:guid}")]
        public async Task<bool> Delete(Guid id) =>
            await _service.DeleteQuestion(id);

    }
}
