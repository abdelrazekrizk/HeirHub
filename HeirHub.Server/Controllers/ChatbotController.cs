using System;
using System.Linq;
using Azure;
using Azure.AI.Language.QuestionAnswering;


class Program
{
    static void Main(string[] args)
    {
        // Replace with your endpoint and key  
        Uri endpoint = new Uri("https://myaccount.api.cognitive.microsoft.com");
        AzureKeyCredential credential = new AzureKeyCredential("{api-key}");

        // Create client  
        QuestionAnsweringClient client = new QuestionAnsweringClient(endpoint, credential);

        // Your project and deployment names from Language Studio  
        string projectName = "FAQ";
        string deploymentName = "prod";

        // Ask a question  
        var options = new QueryKnowledgeBaseOptions("How long should my Surface battery last?")
        {
            ProjectName = projectName,
            DeploymentName = deploymentName,
            Top = 1
        };

        Response<KnowledgeBaseAnswers> response = client.QueryKnowledgeBase(options);

        foreach (KnowledgeBaseAnswer answer in response.Value.Answers)
        {
            Console.WriteLine($"({answer.ConfidenceScore:P2}) {answer.Answer}");
            Console.WriteLine($"Source: {answer.Source}");
            Console.WriteLine();
        }

        // Example of follow-up question (chit-chat)  
        KnowledgeBaseAnswer previousAnswer = response.Value.Answers.First();

        var followUpOptions = new QueryKnowledgeBaseOptions("How long should charging take?")
        {
            ProjectName = projectName,
            DeploymentName = deploymentName,
            Context = new KnowledgeBaseAnswerRequestContext(previousAnswer.Id.Value),
            Top = 1
        };

        Response<KnowledgeBaseAnswers> followUpResponse = client.QueryKnowledgeBase(followUpOptions);

        foreach (KnowledgeBaseAnswer answer in followUpResponse.Value.Answers)
        {
            Console.WriteLine($"({answer.ConfidenceScore:P2}) {answer.Answer}");
            Console.WriteLine($"Source: {answer.Source}");
            Console.WriteLine();
        }
    }
}