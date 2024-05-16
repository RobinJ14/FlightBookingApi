using Microsoft.EntityFrameworkCore;
using WebApiSimplyFly.Context;
using WebApiSimplyFly.Exceptions;
using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Repositories
{
    public class UserRepository : IRepository<User, string>
    {

        private readonly newContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(newContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<User> Add(User item)
        {
            _context.Add(item);
            _context.SaveChanges();
            _logger.LogInformation($"User added with username {item.Username}");
            return item;
        }

        public async Task<User> Delete(string key)
        {

            var user = await GetAsync(key);
            if (user != null)
            {
                _context.Remove(user);
                _context.SaveChanges();
                _logger.LogInformation($"User deleted with username {key}");
                return user;
            }
            throw new NoSuchUserException();
        }

        public async Task<User> GetAsync(string key)
        {
            var users = _context.Users.ToList(); 
            var user = users.SingleOrDefault(u => string.Equals(u.Username, key, StringComparison.Ordinal));
            return user;
        }

        public async Task<List<User>> GetAsync()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        public async Task<User> Update(User item)
        {
             var user = await GetAsync(item.Username);
            if (user != null)
            {
                _context.Entry<User>(item).State = EntityState.Modified;
                _context.SaveChanges();
                _logger.LogInformation($"User updated with username {item.Username}");
                return item;
            }
            throw new NoSuchUserException();
        }
        
    }
}
