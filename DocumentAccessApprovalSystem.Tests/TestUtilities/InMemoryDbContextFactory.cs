using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentAccessApprovalSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DocumentAccessApprovalSystem.Tests.TestUtilities
{
    public static class InMemoryDbContextFactory
    {
        public static AppDbContext Create(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new AppDbContext(options);
        }
    }
}
