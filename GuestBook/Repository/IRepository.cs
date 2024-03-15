using System.Collections;
using GuestBook.Models;
using Microsoft.EntityFrameworkCore;

namespace GuestBook.Repository
{
    public interface IRepository
    {
        Task<List<Messages>> GetBookList();
        Task<List<User>> GetUserList();

        Task Create(Messages item);
        Task CreateUser(User item);
        Task<User> InOrOut(string a, string b);
        Task <List<Messages>> IncludeMessage();

        Task <User> Login(string login);
        IEnumerable GetUser();
        Task <User> GetUserByIdAsync(int  id);
        Task Save();



    }
}
