
namespace QuestionApi.Models
{
    public class ApplicationData
    {
        public string ApplicantId { get; set; } // For identifying the applicant
        public List<QuestionResponse> Responses { get; set; }
    }

    public class QuestionResponse
    {
        public string QuestionId { get; set; }
        public string Response { get; set; }
    }
}
