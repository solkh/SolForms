using Microsoft.AspNetCore.Mvc;
using SolForms.Extentions;
using SolForms.Models;
using SolForms.Models.Questions;
using SolForms.Services;

namespace SolFormsApi.Controllers
{
    [ApiController]
    [Route("SolForms/Submissions")]
    public class SubmissionController : ControllerBase
    {
        private readonly ILogger<SFSubmition> _logger;
        private readonly SFService _service;
        public SubmissionController(ILogger<SFSubmition> logger, SFService service)
        {
            _logger = logger;
            _service = service;
        }
        //Get
        [HttpGet("{id:guid}")]
        public async Task<SFSubmition?> Get(Guid id) =>
            await _service.GetSubmittion(id);

        [HttpGet("All/{formId:guid}")]
        public async Task<SFSubmition?[]> GetAll(Guid formId) =>
            await _service.GetSubmittions(formId);


        //Post
        [HttpPost]
        public async Task Create(SFSubmition answeringSession) =>
            await _service.SubmitForm(answeringSession);
        public async Task Create(AnsweringSessionDto answeringSession)
        {
            var sid = Guid.NewGuid();
            var answers = new AnsweringSession()
            {
                Id = sid,
                FormId = answeringSession.FormId,
                UserPhone = answeringSession.UserPhone,
                BirthDate = answeringSession.BirthDate,
                ConsultationDate = answeringSession.ConsultationDate,                
                UserEmail = answeringSession.UserEmail,
                UserName = answeringSession.UserName,
                Answers = answeringSession.Answers.Select(x => new Answer { SubmissionId = sid, QuestionId = x.QuestionId ?? Guid.Empty, Value = x.Value }).ToList(),
            };
            foreach (var answer in answers.Answers)
            {                
                answer.Type = await _service.GetQuestionTypeById(answer.QuestionId);
            }
            await _service.SubmitForm(answers);
        }

        [HttpPost("CreateAnsweringSessionTemplte")]
        public async Task Create(string userName, string userEmail, string userPhone) =>
            await _service.SubmitForm(SolFormsFactory.CreateAnsweringSession(userName, userEmail, userPhone));


        //Put
        [HttpPut("{id:guid}")]
        public async Task Update(Guid id, SFSubmition answeringSession) =>
            await _service.UpdateSubmission(id, answeringSession);


        //Delete
        [HttpDelete("Delete/{id:guid}")]
        public async Task<bool> Delete(Guid id) =>
            await _service.DeleteSubmittion(id);

    }
}
