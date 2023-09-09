
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
        SolFormSection = 1,
        BaseQuestion = 2,
        Option = 3,
        ShowCondition = 4,
        AnsweringSession = 5,
        Answer = 6,
    }
}
