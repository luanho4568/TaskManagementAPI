using DataBase.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<Group_Level> Group_Level { get; set; }
        public DbSet<Group_Member> Group_Member { get; set; }
        public DbSet<ChatGroup> ChatGroup { get; set; }
        public DbSet<ChatGroupMember> ChatGroupMember { get; set; }
        public DbSet<ChatMessage> ChatMessage { get; set; }
        public DbSet<ChatMessageSeen> ChatMessageSeen { get; set; }
    }
}
