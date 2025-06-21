# HeirHub – AI-Powered Legal Aid Platform for Heirs’ Property Issues

**A central place for heirs' resources and support.**

## Table of Contents

- [Project Overview](#project-overview)
- [Installation](#installation)
- [Usage](#usage)
- [Contributing](#contributing)
- [Resources](#resources)

## Project Overview

HeirHub is an AI-powered legal aid platform designed to assist heirs with property-related issues. Leveraging advanced AI language services, the platform provides users with accessible, accurate legal information and support tailored to heirs’ property challenges.

## Installation

Follow these steps to set up HeirHub locally:

````bash
# Clone the repository
git clone https://github.com/yourusername/HeirHub.git

# Navigate into the project directory
cd HeirHub

# Install backend dependencies
dotnet restore

### Install Azure SDK NuGet Packages

In Visual Studio, open the **Package Manager Console** and run:

```powershell
Install-Package Azure.AI.FormRecognizer
Install-Package Azure.AI.TextAnalytics
Install-Package Azure.AI.Language.QuestionAnswering
```

# Install frontend dependencies (if applicable)

cd heirhub.client
npm install

````
### Configure API Keys and Endpoints

Add your Azure service keys and endpoints to `appsettings.json`:

```json
{
    "Azure": {
        "FormRecognizerEndpoint": "https://<your-form-recognizer-resource>.cognitiveservices.azure.com/",
        "FormRecognizerApiKey": "<your-form-recognizer-key>",
        "TextAnalyticsEndpoint": "https://<your-text-analytics-resource>.cognitiveservices.azure.com/",
        "TextAnalyticsApiKey": "<your-text-analytics-key>",
        "LanguageEndpoint": "https://<your-language-resource>.cognitiveservices.azure.com/",
        "LanguageApiKey": "<your-language-service-key>",
        "ProjectName": "<your-question-answering-project-name>",
        "DeploymentName": "<your-deployment-name>"
    }
}
```

# Run the backend server
```
dotnet run
```
# Run the frontend app (if separate)
```
npm start
```
## Usage

Once installed, you can:

- Upload legal documents related to heirs’ property.
- Interact with the AI-powered chatbot for legal guidance.
- Access resources and FAQs tailored to heirs’ property issues.

## Resources

- [Azure AI Language Service Documentation](https://learn.microsoft.com/en-us/azure/ai-services/language-service/)
- [Heirs’ Property Legal Information](https://www.americanbar.org/groups/crsj/publications/human_rights_magazine_home/wealth-disparities-in-the-black-community/heirs-property/)
