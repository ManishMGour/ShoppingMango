﻿
using Mango.Services.EmailAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.EmailAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<EmailLogger> emailLoggers { get; set; }



    }
}
