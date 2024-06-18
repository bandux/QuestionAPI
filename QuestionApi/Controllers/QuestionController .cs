using Microsoft.AspNetCore.Mvc;
using QuestionApi.DTOs;
using QuestionApi.Models;
using QuestionApi.Services;
using QuestionApi.Util;

namespace QuestionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionController(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateQuestionDto createQuestionDto)
        {
            var question = new Question
            {
                Id = Guid.NewGuid().ToString(),
                Text = createQuestionDto.Text,
                Type = Enum.Parse<QuestionType>(createQuestionDto.Type, true),
                CreatedAt = DateTime.UtcNow
            };

            await _questionRepository.AddQuestionAsync(question);

            return CreatedAtAction(nameof(Post), new { id = question.Id }, question);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateQuestionDto updateQuestionDto)
        {
            if (id != updateQuestionDto.Id)
            {
                return BadRequest("Id in URL does not match Id in body");
            }

            var question = new Question
            {
                Id = id,
                Text = updateQuestionDto.Text,
                Type = Enum.Parse<QuestionType>(updateQuestionDto.Type, true),
                CreatedAt = DateTime.UtcNow
            };

            await _questionRepository.UpdateQuestionAsync(id, question);

            return NoContent();
        }
        [HttpGet("type/{type}")]
        public async Task<IActionResult> GetQuestionsByType(string type)
        {
            if (!Enum.TryParse<QuestionType>(type, true, out var questionType))
            {
                return BadRequest("Invalid question type");
            }

            var questions = await _questionRepository.GetQuestionsByTypeAsync(questionType);

            return Ok(questions);
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitApplication([FromBody] ApplicationData applicationData)
        {
            if (applicationData == null || applicationData.Responses == null || applicationData.Responses.Count == 0)
            {
                return BadRequest("Invalid application data");
            }
            return Ok("Application submitted successfully");
        }

    }
}
