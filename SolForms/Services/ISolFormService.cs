using SolForms.Models.Questions;
using SolForms.Models;

namespace SolForms.Services
{
    public interface ISolFormsService
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
        Task<SolFormSection?> GetSection(Guid sectionId);
        Task<SolFormSection?[]> GetSections(Guid formId);                  
        Task CreateSection(SolFormSection section);
        Task UpdateSection(SolFormSection section);
        Task UpdateSection(Guid id, SolFormSection section);
        Task<bool> DeleteSection(Guid sectionId);

        //Questions
        Task<BaseQuestion?> GetQuestion(Guid questionId);
        Task<BaseQuestion?[]> GetQuestions(Guid sectionId);                        
        Task CreateQuestion(BaseQuestion question);
        Task UpdateQuestion(BaseQuestion question);
        Task UpdateQuestion(Guid id, BaseQuestion question);
        Task<bool> DeleteQuestion(Guid questionId);        

        //Conditions        
        Task<ShowCondition?> GetCondtion(Guid conditionId);           
        Task CreateCondition(ShowCondition condition);
        Task UpdateCondtion(ShowCondition condition);
        Task UpdateCondtion(Guid id, ShowCondition condition);
        Task<bool> DeleteCondtion(Guid conditionId);

        //Options
        Task<Option?> GetOption(Guid optionId);
        Task<Option?[]> GetOptions(Guid questionId);
        Task CreateOption(Option option);
        Task UpdateOption(Option option);
        Task UpdateOption(Guid id, Option option);
        Task<bool> DeleteOption(Guid OptionId);

        //Submittions
        Task<AnsweringSession?> GetSubmittion(Guid sessionId);
        Task<AnsweringSession?[]> GetSubmittions(Guid formId);
        Task SubmitForm(AnsweringSession answeringSession);
        Task UpdateSubmission(AnsweringSession answeringSession);
        Task UpdateSubmission(Guid id, AnsweringSession answeringSession);
        Task<bool> DeleteSubmittion(Guid sessionId); 
        Task<int> CountSubmittions(Guid formId);        

        //Answers
        Task<Answer?> GetAnswer(Guid sessionId);
        Task<Answer?[]> GetAnswers(Guid sesstionId);        
        Task CreateAnswers(Answer[] answers);
        Task CreateAnswer(Answer answer);
        Task UpdateAnswer(Answer answer);
        Task UpdateAnswer(Guid id, Answer answer);
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

