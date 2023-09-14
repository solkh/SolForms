
namespace SolForms.Data
{
    public class FormsKeyValue
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }


    public class FormsKeyTypeValue
    {
        public Guid Id { get; set; }
        public EntityType EntityType { get; set; }
        public required string Value { get; set; }
    }


    public enum EntityType
    {
        SolForm = 0,        
        SFSection = 1,
        SFQuestion = 2,
        SFOption = 3,
        SFShowCondition = 4,
        SFSubmission = 5,
        SFAnswer = 6,
    }
}
