namespace SolForms.Models
{
    public record SolForm
    {
        public Guid Id { get; set; }
        public string FormTitle { get; set; }
        public string ShortDescrption { get; set; }
        public bool IsActive { get; set; }
        public virtual List<SFSection> FormSections { get; set; } = new List<SFSection>();
    }
}
