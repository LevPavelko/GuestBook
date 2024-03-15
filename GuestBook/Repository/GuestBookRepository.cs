using System.Collections;
using GuestBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GuestBook.Repository
{
    public class GuestBookRepository : IRepository  
    {
    
        private readonly GuestBookContext _context;

        public GuestBookRepository(GuestBookContext context)
        {
            _context = context;
        }

        public IEnumerable GetUser()
        {
            return _context.Users;
        }
        public async Task<User> GetUserByIdAsync(int userId)
        {
           
            return await _context.Users.FindAsync(userId);
        }

        public async Task<List<Messages>> GetBookList()
        {
            return await _context.Messages.ToListAsync();
        }
        public async Task<List<User>> GetUserList()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task Create(Messages c)
        {
            await _context.Messages.AddAsync(c);
        }
        public async Task CreateUser(User c)
        {
            await _context.Users.AddAsync(c);
        }


        public async Task<User> InOrOut(string firstName, string lastName)
        {
           

            return await _context.Users.FirstOrDefaultAsync(u => u.FirstName == firstName && u.LastName == lastName);
        
        }

        public async Task<List<Messages>> IncludeMessage()
        {
            return await _context.Messages.Include(m => m.User).ToListAsync();
            
        }
        public async Task<User> Login(string login)
        {
            return await _context.Users.FirstOrDefaultAsync(a => a.Login == login);
        }
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }


    }
}
