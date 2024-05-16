using Microsoft.EntityFrameworkCore;
using WebApiSimplyFly.Context;
using WebApiSimplyFly.Exceptions;
using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Repositories
{
    public class AdminRepository : IRepository<Admin, int>
    {

        readonly newContext _context;
        ILogger<AdminRepository> _logger;
       
        public AdminRepository(newContext context, ILogger<AdminRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Admin> Add(Admin items)
        {
            _context.Add(items);
            _context.SaveChanges();
            _logger.LogInformation("Admin added with id " + items.AdminId);
            return items;
        }

        public  async Task<Admin> Delete(int key)
        {
            var admin =await GetAsync(key);
            if (admin != null)
            {
                _context.Remove(admin);
                _context.SaveChanges();
                _logger.LogInformation("Admin deleted with id " + key);
                return admin;
            }
            throw new NoSuchAdminException();
        }

        public async Task<Admin> GetAsync(int key)
        {
            var admins = await GetAsync();
            var admin = admins.FirstOrDefault(e => e.AdminId == key);
            if (admin != null)
            {
                return admin;
            }
            throw new NoSuchAdminException();
        }

        public async Task<List<Admin>> GetAsync()
        {
            var admins = _context.Admins.ToList();
            return admins;
        }

        public async Task<Admin> Update(Admin items)
        {
            var admin = await GetAsync(items.AdminId);
            if (admin != null)
            {
                _context.Entry<Admin>(items).State = EntityState.Modified;
                _context.SaveChanges();
                _logger.LogInformation("Admin updated with id " + items.AdminId);
                return admin;
            }
            throw new NoSuchAdminException();
        }
    }
}
