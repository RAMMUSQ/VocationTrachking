﻿using Domain.Core;
using Microsoft.EntityFrameworkCore;
using Presentation.Models;


namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }


    public DbSet<User> Users { get; set; }

    public DbSet<UserGroups> UserGroups { get; set; }


    public DbSet<Token> Tokens { get; set; }

    public DbSet<UserGroupMember> UserGroupMembers { get; set; }
    public DbSet<LeaveRequest> LeaveRequests { get; set; }
    
    public DbSet<AnnualLeaveRight> AnnualLeaveRights { get; set; }

    //public DbSet<LeaveRequest> LeaveRequests { get; set; }
}


/*using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<PermissionRequest> PermissionRequests { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}
*/