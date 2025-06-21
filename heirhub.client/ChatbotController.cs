using Azure;
using Azure.AI.Language.QuestionAnswering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace HeirAssist.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatbotController : ControllerBase
    {
        private readonly QuestionAnsweringClient _qaClient;
        private readonly QuestionAnsweringProject _project;

        public ChatbotController(IConfiguration config)
        {
            var endpoint = new Uri(config["Azure:LanguageEndpoint"]);
            var credential = new AzureKeyCredential(config["Azure:LanguageApiKey"]);
            _qaClient = new QuestionAnsweringClient(endpoint, credential);

            string projectName = config["Azure:ProjectName"];
            string deploymentName = config["Azure:DeploymentName"];
            _project = new QuestionAnsweringProject(projectName, deploymentName);
        }

        [HttpPost("ask")]
        public async Task<IActionResult> AskQuestion([FromBody] UserQuestion question)
        {
            if (string.IsNullOrWhiteSpace(question?.Text))
                return BadRequest("Question text is required.");

            Response<AnswersResult> response = await _qaClient.GetAnswersAsync(question.Text, _project);

            var answer = "Sorry, I don't have an answer for that.";
            if (response.Value.Answers.Count > 0)
            {
                // Pick the top answer  
                var topAnswer = response.Value.Answers[0];
                answer = topAnswer.Answer;
            }

            return Ok(new { answer });
        }
    }

    public class UserQuestion
    {
        public string Text { get; set; }
    }
}