using Mscc.GenerativeAI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AppOfNexusNinjas.Services
{
    // Define the UTSInfo class to match the structure of your JSON data
    public class UTSInfo
    {
        public string Question { get; set; }
        public string Answer { get; set; }

        public class ApiService
        {
            private readonly HttpClient _httpClient; // HttpClient instance for making HTTP requests
            private readonly Dictionary<string, string> _dataset; // Holds predefined responses
            private readonly string _foundationJsonPath; // Path to FoundationInfo.json
            private readonly string _utsJsonPath; // Path to UTSInfo.json

            public string GeminiPro { get; private set; }

            public ApiService(string apiKey, string foundationJsonPath, string utsJsonPath)
            {
                _httpClient = new HttpClient(); // Initialize the HttpClient instance
                _foundationJsonPath = foundationJsonPath;
                _utsJsonPath = utsJsonPath;

                // Load predefined responses from JSON files
                _dataset = LoadDataFromJson();
            }

            // Method to load predefined responses from JSON files
            private Dictionary<string, string> LoadDataFromJson()
            {
                var dataset = new Dictionary<string, string>();

                try
                {
                    // Read FoundationInfo.json
                    var foundationData = File.ReadAllText(_foundationJsonPath);
                    var foundationInfos = JsonConvert.DeserializeObject<List<UTSInfo>>(foundationData);
                    foreach (var info in foundationInfos)
                    {
                        dataset.Add(info.Question, info.Answer);
                    }

                    // Read UTSInfo.json
                    var utsData = File.ReadAllText(_utsJsonPath);
                    var utsInfos = JsonConvert.DeserializeObject<List<UTSInfo>>(utsData);
                    foreach (var info in utsInfos)
                    {
                        dataset.Add(info.Question, info.Answer);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error loading data from JSON files: " + ex.Message);
                }

                return dataset;
            }

            // Method to generate response based on prompt
            public async Task<string> GenerateResponse(string prompt)
            {
                // Check if the prompt exists in the loaded dataset and does not contain "University of Technology Sydney"
                if (_dataset.TryGetValue(prompt, out string response) && !prompt.Contains("University of Technology Sydney"))
                {
                    return response; // Return the response directly from the dataset
                }

                // If no dataset match is found or the prompt contains "University of Technology Sydney", call the Google AI model
                return await GetResponseFromGoogleAIModelAsync(prompt);
            }

            // Method to connect to the Google AI model and get response asynchronously
            public async Task<string> GetResponseFromGoogleAIModelAsync(string userInput)
            {
                // Example API key for Google AI (replace "YOUR_API_KEY_HERE" with your actual API key)
                string googleApiKey = "AIzaSyBMOIN0eQ-ySkJLJx67pM96n_ZWUWdnMns";

                try
                {
                    // Initialize Google AI service with the API key
                    var googleAI = new GoogleAI(apiKey: googleApiKey);

                    // Specify the model to use (replace "YOUR_MODEL_NAME_HERE" with the actual model name)
                    var model = googleAI.GenerativeModel(model: GeminiPro);

                    // Generate content using the specified model
                    var response = await model.GenerateContent(userInput);

                    // Return the generated text
                    return response.Text;
                }
                catch (Exception ex)
                {
                    // Return error message if an exception occurs
                    return "Error: " + ex.Message;
                }
            }

            // Method to test the Google AI model with a prompt
            public async Task<string> Test(string prompt)
            {
                // Example API key and URL
                string apiKey = "AIzaSyBMOIN0eQ-ySkJLJx67pM96n_ZWUWdnMns";
                string requestUrl = $"https://aistudio.google.com/app/apikey{Uri.EscapeDataString(prompt)}";

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                try
                {
                    var response = await _httpClient.GetAsync(requestUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        return responseContent; // Return the successful response content
                    }
                    else
                    {
                        // Log the error response
                        var errorResponse = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error: {response.StatusCode} - {errorResponse}");
                        return "Error: Unable to connect to the AI model."; // Provide a user-friendly error message
                    }
                }
                catch (HttpRequestException e)
                {
                    // Log the exception
                    Console.WriteLine($"Exception: {e.Message}");
                    return "Error: Unable to connect to the AI model."; // Provide a user-friendly error message
                }
            }
        }
    }
}
