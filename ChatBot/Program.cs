using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;

// Configuring the API Key
var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
string model = config["ModelName"];
string key = config["OpenAIKey"];

// Create the IChatClient
IChatClient chatClient =
    new OpenAIClient(key).GetChatClient(model).AsIChatClient();

// Start the conversation with context for the AI model
List<ChatMessage> chatHistory =
    [
        new ChatMessage(ChatRole.System, """
            You're a professional advisor and also an expert programmer. You always review code and suggest valuable improvements in both code and security.
            You always follow these rules:
            1. DRY
            2. SOLID
            3. CLEAN
            You also highlight both strong and weak parts. You can also find vulnerabilities in the code, and provide suggestions to fix them.
        """)
    ];

            // Loop to get user input and stream AI response
            while (true)
            {
                // Get user prompt and add to chat history
                Console.WriteLine("Your prompt:");
                string? userPrompt = Console.ReadLine();
chatHistory.Add(new ChatMessage(ChatRole.User, userPrompt));

                // Stream the AI response and add to chat history
                Console.WriteLine("AI Response:");
                string response = "";
await foreach (ChatResponseUpdate item in
                    chatClient.GetStreamingResponseAsync(chatHistory))
                {
                    Console.Write(item.Text);
                    response += item.Text;
                }
                chatHistory.Add(new ChatMessage(ChatRole.Assistant, response));
Console.WriteLine();
            }
