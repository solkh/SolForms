using SolForms.Models.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolForms.Models
{
    public interface IAnsweringSession
    {
        Guid Id { get; set; }
        Guid FromId { get; set; }
        string UserName { get; set; }
        string UserEmail { get; set; }
        string UserPhone { get; set; }
        List<Answer> Answers { get; set; }
    }
    public class AnsweringSession : IAnsweringSession
    {
        public Guid Id { get; set; }
        public Guid FromId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime ConsultationDate { get; set; }
        public List<Answer> Answers { get; set; } = new List<Answer>();
    }
}
