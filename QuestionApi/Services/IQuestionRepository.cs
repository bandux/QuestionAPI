using QuestionApi.Models;
using QuestionApi.Util;

namespace QuestionApi.Services
{
    public interface IQuestionRepository
    {
        Task<Question> AddQuestionAsync(Question question);
        Task UpdateQuestionAsync(string id, Question question);
        Task<IEnumerable<Question>> GetQuestionsByTypeAsync(QuestionType type);
        Task<bool> SubmitApplicationAsync(ApplicationData applicationData);
    }
}
