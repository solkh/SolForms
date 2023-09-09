using SolForms.Models;
using SolForms.Models.Questions;
using SolForms.Services;
using System.Web.Http;

namespace SolForms.ApiControllers
{
    [Route("api/{controller}/{action}/{id?:int}")]
    //[ApiVersion("1")]
    public class FormsController : ApiController
    {
        private readonly ISolFormsService _service;
        public FormsController(ISolFormsService service)
        {
            _service = service;
        }
        public static List<string> FormToCsvLines(SolForm solForm)
        {
            var csvRows = new List<string>();

            var row = new List<string>
            {
                solForm.Id.ToString(),
                solForm.FormTitle,
                solForm.ShortDescrption,
                solForm.IsActive.ToString(),

            };
            foreach (var section in solForm.FormSections)
            {
                row.Add(section.FormSectionTitle.ToString());
                row.Add(section.Id.ToString());

                foreach (var question in section.Questions ?? new List<BaseQuestion>())
                {
                    row.Add(question.Id.ToString());
                    row.Add(question.QuestionText ?? "");
                    row.Add(question.Type.ToString());
                    row.Add(question.ShowCondition.QuestionId.ToString() ?? Guid.Empty.ToString());
                    row.Add(question.ShowCondition.OptionId.ToString() ?? Guid.Empty.ToString());
                    row.Add(question.ShowCondition.Type.ToString());

                    foreach (var option in question.Options ?? new List<Option>())
                    {
                        row.Add(option.Id.ToString());
                        row.Add(option.Text ?? "");
                        row.Add(option.Order.ToString() ?? 0.ToString());
                    }
                }
            }
            csvRows.Add(string.Join(",", row));

            return csvRows;
        }
        public async Task<SolForm?[]> GetAllForms()
        {
            return await _service.GetForms();
        }
        public async Task<SolForm?> GetFormById(Guid id)
        {
            return await _service.GetForm(id);
        }
        public async Task CreateForm(SolForm form)
        {
             await _service.CreateForm(form);
        }
        public async Task<bool> UpdateForm(Guid formId, SolForm form)
        {
            return await _service.UpdateForm(formId, form);
        }
        public async Task<bool> DeleteForm(Guid formId)
        {
            return await _service.DeleteForm(formId);
        }
        //[HttpGet]
        //public Task<File> Download(Guid id)
        //{            
        //    var solForm = GetSolFormById(id);
        //
        //    var csvLines = SolFormsService.FormToCsvLines(solForm);
        //
        //    var csvData = string.Join("\n", csvLines);
        //
        //    var content = new System.Text.UTF8Encoding().GetBytes(csvData);
        //    var contentType = "text/csv";
        //    var fileName = "solForm.csv";
        //
        //    return File(content, contentType, fileName);
        //}

    }
}
