using Microsoft.AspNetCore.Mvc;
using SolForms.Extentions;
using SolForms.Models;
using SolForms.Services;

namespace SolFormsApi.Controllers
{
    [ApiController]
    [Route("SolForms/Sections")]
    public class SectionController : ControllerBase
    {
        private readonly ILogger<SolFormSection> _logger;
        private readonly ISolFormsService _service;
        public SectionController(ILogger<SolFormSection> logger, ISolFormsService service)
        {
            _logger = logger;
            _service = service;
        }
        //Get
        [HttpGet("{id:guid}")]
        public async Task<SolFormSection?> Get(Guid id) =>
            await _service.GetSection(id);

        [HttpGet("Count/All/{formId:guid}")]
        public async Task<SolFormSection?[]> GetByFormId(Guid formId) =>
            await _service.GetSections(formId);


        //Post
        [HttpPost]
        public async Task Create(SolFormSection section) =>
            await _service.CreateSection(section);

        [HttpPost("CreateSectionTemplte")]
        public async Task Create(string title) =>
            await _service.CreateSection(SolFormsFactory.CreateSection(title));

        //Put
        [HttpPut("{id:guid}")]
        public async Task Update(Guid id, SolFormSection section) =>
            await _service.UpdateSection(id, section);


        //Delete
        [HttpDelete("{id:guid}")]
        public async Task<bool> Delete(Guid id) =>
            await _service.DeleteSection(id);

    }
}
