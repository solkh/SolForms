using Microsoft.AspNetCore.Mvc;
using SolForms.Extentions;
using SolForms.Models;
using SolForms.Models.Enums;
using SolForms.Models.Questions;
using SolForms.Services;

namespace SolFormsApi.Controllers
{
    [ApiController]
    [Route("SolForms/Conditions")]
    public class ConditionController : ControllerBase
    {
        private readonly ILogger<ShowCondition> _logger;
        private readonly ISolFormsService _service;
        public ConditionController(ILogger<ShowCondition> logger, ISolFormsService service)
        {
            _logger = logger;
            _service = service;
        }
        //Get
        [HttpGet("{id:guid}")]
        public async Task<ShowCondition?> Get(Guid id) =>
            await _service.GetCondtion(id);

        //Post
        [HttpPost]
        public async Task Create(ShowCondition condition) =>
            await _service.CreateCondition(condition);

        [HttpPost("CreateConditionTemplte")]
        public async Task Create(ConditionType type) =>
            await _service.CreateCondition(SolFormsFactory.CreateCondition(type));


        //Put
        [HttpPut("{id:guid}")]
        public async Task Update(Guid id, ShowCondition condition) =>
            await _service.UpdateCondtion(id, condition);


        //Delete
        [HttpDelete("{id:guid}")]
        public async Task<bool> Delete(Guid id) =>
            await _service.DeleteCondtion(id);

    }
}
