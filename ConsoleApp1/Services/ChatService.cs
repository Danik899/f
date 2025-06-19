using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace KBIPMobileBackend.Services
{
    public class ChatService : IChatService
    {
        private readonly HttpClient _http;
        
        public ChatService(HttpClient http)
        {
            _http = http;
            _http.BaseAddress = new Uri("http://localhost:5000/Chat");
        }
        
        public string ProcessQuestion(string question)
        { 
            return "Ответ на: " + question;
        }

        public async Task<string> AskAsync(string question)
        {
            question = question.ToLower();

            foreach (var item in BasicVocabulary.TokenToWord)
            {
                if (question.Contains(item.Value.ToLower())) 
                {
                    if (BasicVocabulary.Descriptions.ContainsKey(item.Value))
                    {
                        return BasicVocabulary.Descriptions[item.Value];
                    }

                    return $"Вы задали вопрос по теме: {item.Value}";
                }
            }

            return "Извините, я не понял ваш вопрос.";
        }
    }
}