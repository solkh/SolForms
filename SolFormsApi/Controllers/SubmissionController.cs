using Microsoft.AspNetCore.Mvc;
using SolForms.Extentions;
using SolForms.Models;
using SolForms.Services;

namespace SolFormsApi.Controllers
{
    [ApiController]
    [Route("SolForms/Submissions")]
    public class SubmissionController : ControllerBase
    {
        private readonly ILogger<AnsweringSession> _logger;
        private readonly ISolFormsService _service;
        public SubmissionController(ILogger<AnsweringSession> logger, ISolFormsService service)
        {
            _logger = logger;
            _service = service;
        }
        //Get
        [HttpGet("{id:guid}")]
        public async Task<AnsweringSession?> Get(Guid id) =>
            await _service.GetSubmission(id);

        [HttpGet("All/{formId:guid}")]
        public async Task<AnsweringSession?[]> GetAll(Guid formId) =>
            await _service.GetSubmissions(formId);


        //Post
        [HttpPost]
        public async Task Create(AnsweringSession answeringSession)
        {
            var sid = Guid.NewGuid();
            answeringSession.Id = sid;
            foreach (var answer in answeringSession.Answers)
            {
               answer.SubmissionId = sid;
            }         
            await _service.SubmitForm(answeringSession);
        }

        [HttpPost("CreateAnsweringSessionTemplte")]
        public async Task Create(string userName, string userEmail, string userPhone) =>
            await _service.SubmitForm(SolFormsFactory.CreateAnsweringSession(userName, userEmail, userPhone));


        //Put
        [HttpPut("{id:guid}")]
        public async Task Update(Guid id, AnsweringSession answeringSession) =>
            await _service.UpdateSubmission(id, answeringSession);


        //Delete
        [HttpDelete("Delete/{id:guid}")]
        public async Task<bool> Delete(Guid id) =>
            await _service.DeleteSubmittion(id);       
        
    }
}
