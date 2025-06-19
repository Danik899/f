using KBIPMobileBackend.DTOs;
using System.Threading.Tasks;

namespace KBIPMobileBackend.Services
{
    public interface IChatService
    {
        Task<string> AskAsync(string question);
    }
}