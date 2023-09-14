namespace SolFormsApi.Options
{
    public class FormEndPointsOptions
    {
        public string BaseUrl { get; set; } = "SolForms";
        public string Forms { get; set; } = "";
        public string Section { get; set; } = "Section";
        public string Question { get; set; } = "Question";        
        public string Condition { get; set; } = "Condition";
        public string Option { get; set; } = "Option";
        public string Answer { get; set; } = "Answer";
        public string Submissions { get; set; } = "Submissions";        
    }
}
