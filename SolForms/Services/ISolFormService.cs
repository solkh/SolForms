using SolForms.Models.Questions;
using SolForms.Models;
using SolForms.Models.Enums;

namespace SolForms.Services
{
    public interface ISFService
    {
        //Forms
        Task<SolForm?> GetForm(Guid formId);
        Task<SolForm?[]> GetForms();
        Task<int> CountForms();
        Task CreateForm(SolForm form);
        Task<bool> UpdateForm(SolForm form);
        Task<bool> UpdateForm(Guid id, SolForm form);
        Task<bool> DeleteForm(Guid id);        
        Task<bool> SetActive(Guid id, bool active);

        //Sections
        Task<SFSection?> GetSection(Guid sectionId);
        Task<SFSection?[]> GetSections(Guid formId);                  
        Task CreateSection(SFSection section);
        Task UpdateSection(SFSection section);
        Task UpdateSection(Guid id, SFSection section);
        Task<bool> DeleteSection(Guid sectionId);

        //Questions
        Task<SFQuestion?> GetQuestion(Guid questionId);
        Task<SFQuestion?[]> GetQuestions(Guid sectionId);                        
        Task CreateQuestion(SFQuestion question);
        Task UpdateQuestion(SFQuestion question);
        Task UpdateQuestion(Guid id, SFQuestion question);
        Task<bool> DeleteQuestion(Guid questionId);        

        //Conditions        
        Task<SFShowCondition?> GetCondtion(Guid conditionId);           
        Task CreateCondition(SFShowCondition condition);
        Task UpdateCondtion(SFShowCondition condition);
        Task UpdateCondtion(Guid id, SFShowCondition condition);
        Task<bool> DeleteCondtion(Guid conditionId);

        //Options
        Task<SFOption?> GetOption(Guid optionId);
        Task<SFOption?[]> GetOptions(Guid questionId);
        Task<bool> IsRedFlag(Guid optionId);
        Task CreateOption(SFOption option);
        Task UpdateOption(SFOption option);
        Task UpdateOption(Guid id, SFOption option);
        Task<bool> DeleteOption(Guid OptionId);

        //Submittions
        Task<SFSubmition?> GetSubmittion(Guid sessionId);
        Task<SFSubmition?[]> GetSubmittions(Guid formId);
        Task SubmitForm(SFSubmition answeringSession);
        Task UpdateSubmission(SFSubmition answeringSession);
        Task UpdateSubmission(Guid id, SFSubmition answeringSession);
        Task<bool> DeleteSubmittion(Guid sessionId); 
        Task<int> CountSubmittions(Guid formId);        

        //Answers
        Task<SFAnswer?> GetAnswer(Guid sessionId);
        Task<SFAnswer?[]> GetAnswers(Guid sesstionId);        
        Task CreateAnswers(SFAnswer[] answers);
        Task CreateAnswer(SFAnswer answer);
        Task UpdateAnswer(SFAnswer answer);
        Task UpdateAnswer(Guid id, SFAnswer answer);
        Task<bool> DeleteAnswer(Guid answerId);        
    }
}
// Laterrrrrrrrrrrrr: Save Submition in Local Storage?
// Laterrrrrrrrrrrrr: Set Expiry Date
// Laterrrrrrrrrrrrr: Get Marks
// Get Histogram Answers of A question
// GetSubmittions as CSV? 
// Get Time Histogram <day1:55><day2:22> ....
// Should Show Question

