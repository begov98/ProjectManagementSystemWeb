using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;

namespace ProjectManagementSystem.UnitTests
{
    public static class DatabaseMock
    {
        public static ApplicationDbContext Instance
        {
            get
            {
                var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

                return new ApplicationDbContext(dbContextOptions);
            }
        }
    }
}
