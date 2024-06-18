using Microsoft.AspNetCore.Mvc;
using Moq;
using QuestionApi.Controllers;
using QuestionApi.DTOs;
using QuestionApi.Models;
using QuestionApi.Services;
using QuestionApi.Util;

namespace QuestionApi.Tests
{
    public class QuestionControllerTests
    {
        private readonly Mock<IQuestionRepository> _mockRepo;
        private readonly QuestionController _controller;

        public QuestionControllerTests()
        {
            _mockRepo = new Mock<IQuestionRepository>();
            _controller = new QuestionController(_mockRepo.Object);
        }

        [Fact]
        public async Task Post_ValidData_ReturnsCreatedAtAction()
        {
            // Arrange
            var questionDto = new CreateQuestionDto
            {
                Text = "What is your favorite color?",
                Type = "MultipleChoice"
            };

            var expectedQuestion = new Question
            {
                Id = "7128cde1-5493-439f-a78a-13b937d155dc",
                Text = questionDto.Text,
                Type = QuestionType.MultipleChoice,
                CreatedAt = DateTime.UtcNow
            };

            _mockRepo.Setup(repo => repo.AddQuestionAsync(It.IsAny<Question>()))
                     .ReturnsAsync(expectedQuestion);

            // Act
            var result = await _controller.Post(questionDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var model = Assert.IsAssignableFrom<Question>(createdAtActionResult.Value);

            Assert.True((expectedQuestion.CreatedAt - model.CreatedAt).Duration() < TimeSpan.FromSeconds(1)); // Adjust the tolerance window as needed
        }



        [Fact]
        public async Task Put_ValidIdAndData_ReturnsNoContent()
        {
            // Arrange
            var questionId = "1";
            var updateDto = new UpdateQuestionDto
            {
                Id = questionId,
                Text = "What is your favorite food?",
                Type = "Dropdown"
            };

            var updatedQuestion = new Question
            {
                Id = questionId,
                Text = updateDto.Text,
                Type = QuestionType.Dropdown,
                CreatedAt = DateTime.UtcNow
            };

            _mockRepo.Setup(repo => repo.UpdateQuestionAsync(questionId, It.IsAny<Question>()))
                     .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Put(questionId, updateDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetQuestionsByType_ValidType_ReturnsOkObjectResult()
        {
            // Arrange
            var questionType = "MultipleChoice";
            var expectedQuestions = new List<Question>
            {
                new Question { Id = "1", Text = "Question 1", Type = QuestionType.MultipleChoice, CreatedAt = DateTime.UtcNow },
                new Question { Id = "2", Text = "Question 2", Type = QuestionType.MultipleChoice, CreatedAt = DateTime.UtcNow }
            };

            _mockRepo.Setup(repo => repo.GetQuestionsByTypeAsync(QuestionType.MultipleChoice))
                     .ReturnsAsync(expectedQuestions);

            // Act
            var result = await _controller.GetQuestionsByType(questionType);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Question>>(okResult.Value);
            Assert.Equal(expectedQuestions.Count, model.Count());
        }

        [Fact]
        public async Task SubmitApplication_ValidData_ReturnsOkObjectResult()
        {
            // Arrange
            var applicationData = new ApplicationData
            {
                CandidateId = "12345",
                Responses = new List<QuestionResponse>
                {
                    new QuestionResponse { QuestionId = "q1", Response = "Blue" },
                    new QuestionResponse { QuestionId = "q2", Response = "Yes" }
                }
            };

            // Act
            var result = await _controller.SubmitApplication(applicationData);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Application submitted successfully", okResult.Value);
        }
    }
}
