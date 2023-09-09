namespace SolForms.Models
{
    public interface ISolForm
    {
        Guid Id { get; set; }
        string FormTitle { get; set; }
        string ShortDescrption { get; set; }
        bool IsActive { get; set; }
        List<SolFormSection> FormSections { get; set; }
    }
    public record SolForm : ISolForm
    {
        public Guid Id { get; set; }
        public string FormTitle { get; set; }
        public string ShortDescrption { get; set; }
        public bool IsActive { get; set; }
        public List<SolFormSection> FormSections { get; set; } = new List<SolFormSection>();
    }
}
