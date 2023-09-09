using FluentAssertions.Common;
using Microsoft.AspNetCore.Mvc;
using SolForms.Extentions;
using SolForms.Models;
using SolForms.Services;


namespace SolFormsApi.Controllers
{
    [ApiController]
    [Route("SolForms")]
    public class FormController : ControllerBase
    {
        private readonly ILogger<SolForm> _logger;
        private readonly ISolFormsService _service;
        public FormController(ILogger<SolForm> logger, ISolFormsService service)
        {
            _logger = logger;
            _service = service;
        }


        //Get
        [HttpGet("{id:guid}")]
        public async Task<SolForm?> Get(Guid id) =>
           await _service.GetForm(id);

        [HttpGet("All")]
        public async Task<SolForm?[]> GetAll() =>
            await _service.GetForms();

        [HttpGet("Count")]
        public async Task<int?> Count() =>
            await _service.CountForms();


        //Post
        [HttpPost]
        public async Task Create(SolForm form) =>
            await _service.CreateForm(form);

        //[HttpPost("CreateFormTemplte")]
        //public async Task Create(string title, string description) =>
        //    await _service.CreateForm(SolFormsFactory.CreateForm(title, description));

        //Put
        [HttpPut("{id:guid}")]
        public async Task<bool> Update(Guid id, SolForm form) =>
            await _service.UpdateForm(id, form);

        [HttpPut("SetActive/{id:guid}/{isActive:bool}")]
        public async Task<bool> SetActive(Guid id, bool isActive) =>
            await _service.SetActive(id, isActive);


        //Delete
        [HttpDelete("{id:guid}")]
        public async Task<bool> Delete(Guid id) =>
            await _service.DeleteForm(id);                      
    }
}
