using Newtonsoft.Json;
using QuestionApi.Util;

namespace QuestionApi.Models
{
    public class Question
    {
        [JsonProperty("id")] 
        public string Id { get; set; }
        public string Text { get; set; }
        public QuestionType Type { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
