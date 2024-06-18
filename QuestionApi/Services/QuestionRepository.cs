using Microsoft.Azure.Cosmos;
using QuestionApi.Models;
using QuestionApi.Util;

namespace QuestionApi.Services
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly Container _questionContainer;

        public QuestionRepository(CosmosClient cosmosClient, IConfiguration configuration)
        {
            var databaseName = configuration["CosmosDb:DatabaseName"];
            var questionContainer = configuration["CosmosDb:QuestionContainer"];
            _questionContainer = cosmosClient.GetContainer(databaseName, questionContainer);
        }

        private readonly List<Question> _questions = new List<Question>();
        private readonly List<ApplicationData> _applications = new List<ApplicationData>();

        public async Task<Question> AddQuestionAsync(Question question)
        {
            question.Id = Guid.NewGuid().ToString();
            question.CreatedAt = DateTime.UtcNow;
            _questions.Add(question);
            return await Task.FromResult(question);
        }

        public async Task<bool> SubmitApplicationAsync(ApplicationData applicationData)
        {
            _applications.Add(applicationData);
            return await Task.FromResult(true);
        }


        public async Task UpdateQuestionAsync(string id, Question question)
        {
            await _questionContainer.UpsertItemAsync(question, new PartitionKey(id));
        }

        public async Task<IEnumerable<Question>> GetQuestionsByTypeAsync(QuestionType type)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.Type = @type")
                .WithParameter("@type", type.ToString());

            var iterator = _questionContainer.GetItemQueryIterator<Question>(query);
            var results = new List<Question>();

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }
    }
}
