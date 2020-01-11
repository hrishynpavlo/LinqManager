using System;
using System.Threading.Tasks;

namespace BookStore.Utils.Interfaces
{
    public interface IEmailService: IDisposable
    {
        Task SendTextEmailAsync(string to, string subject, string text);
    }
}