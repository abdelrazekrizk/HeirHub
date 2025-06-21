using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Azure.AI.TextAnalytics;
using Azure.AI.TextAnalytics.Models;
using System.Text;
using System.Linq; // Add this using directive
using System.Collections.Generic; // Add this using directive for LINQ methods like ToList
using System.Threading; // Add this directive for CancellationToken

namespace HeirAssist.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly DocumentAnalysisClient _formRecognizerClient;
        private readonly TextAnalyticsClient _textAnalyticsClient;

        public DocumentController(IConfiguration configuration)
        {
            var formRecognizerEndpoint = new Uri(configuration["Azure:FormRecognizerEndpoint"]);
            var formRecognizerKey = new AzureKeyCredential(configuration["Azure:FormRecognizerApiKey"]);
            _formRecognizerClient = new DocumentAnalysisClient(formRecognizerEndpoint, formRecognizerKey);

            var textAnalyticsEndpoint = new Uri(configuration["Azure:TextAnalyticsEndpoint"]);
            var textAnalyticsKey = new AzureKeyCredential(configuration["Azure:TextAnalyticsApiKey"]);
            _textAnalyticsClient = new TextAnalyticsClient(textAnalyticsEndpoint, textAnalyticsKey);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadDocument(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            using var stream = file.OpenReadStream();

            AnalyzeDocumentOperation operation = await _formRecognizerClient.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-layout", stream);
            AnalyzeResult result = operation.Value;

            var sb = new StringBuilder();
            foreach (var page in result.Pages)
            {
                sb.AppendLine($"Page {page.PageNumber}:");
                foreach (var line in page.Lines)
                {
                    sb.AppendLine(line.Content);
                }
                sb.AppendLine();
            }

            string extractedText = sb.ToString();

            if (string.IsNullOrWhiteSpace(extractedText))
                return Ok(new { summary = "No text extracted from document." });

            var summaryOperation = await _textAnalyticsClient.ExtractiveSummarizeAsync(WaitUntil.Completed, new[] { extractedText });
            var summaryResults = new List<ExtractiveSummarizeResultCollection>();

            await foreach (var page in summaryOperation.GetValuesAsync(CancellationToken.None))
            {
                summaryResults.Add(page);
            }

            StringBuilder summaryBuilder = new StringBuilder();
            foreach (var summaryResultCollection in summaryResults)
            {
                foreach (var summaryResult in summaryResultCollection)
                {
                    foreach (var sentence in summaryResult.Sentences)
                    {
                        summaryBuilder.AppendLine(sentence.Text);
                    }
                }
            }

            string summary = summaryBuilder.ToString();

            return Ok(new { summary });
        }
    }
}