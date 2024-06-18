
namespace QuestionApi.Models
{
    public class ApplicationData
    {
        public string CandidateId { get; set; }
        public List<QuestionResponse> Responses { get; set; }
    }

    public class QuestionResponse
    {
        public string QuestionId { get; set; }
        public string Response { get; set; }
    }
}
