using SolForms.Models.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SolForms.Models
{
    public class SFSubmission
    {
        [JsonIgnore]
        public Guid? Id { get; set; }
        public Guid FormId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime ConsultationDate { get; set; }
        public bool IsRedFlag { get; set; }
        public DateTime SubmitionDate { get; set; }
        public virtual List<SFAnswer>? Answers { get; set; } = new List<SFAnswer>();
    }
    public class SFSubmitionDto
    {
        public Guid FormId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime ConsultationDate { get; set; }
        public DateTime SubmitionDate { get; set; }
        public List<SFAnswerDto> Answers { get; set; } = new List<SFAnswerDto>();
    }
}
