﻿using com.teamseven.musik.be.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;



namespace com.teamseven.musik.be.Repositories.impl
{
    public class UserRepository : IUserRepository
    {
        private readonly MusikDbContext _context; public UserRepository(MusikDbContext context) { _context = context; }
        public async Task<User> GetByIdAsync(int id) { return await _context.Users.FindAsync(id); }
        public async Task<User> GetByEmailAsync(string email) { return await _context.Users.FirstOrDefaultAsync(u => u.Email == email); }
        public async Task AddUserAsync(User user)
        {
            Console.WriteLine($"Before SaveChangesAsync - User ID: {user.UserId}");

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            Console.WriteLine($"After SaveChangesAsync - User ID: {user.UserId}");
        }


        public async Task DeleteUserAsync(int id) { var user = await _context.Users.FindAsync(id); if (user != null) { _context.Users.Remove(user); await _context.SaveChangesAsync(); } }
        public async Task UpdateUserAsync(User user) { _context.Users.Update(user); await _context.SaveChangesAsync(); }
    }
}