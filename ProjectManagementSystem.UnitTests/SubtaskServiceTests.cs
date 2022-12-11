using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProjectManagementSystem.Contracts;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Data.Models;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Services;

namespace ProjectManagementSystem.UnitTests
{
    [TestFixture]
    public class SubtaskServiceTests
    {
        private IEnumerable<Subtask> listOfSubtasks;
        private IEnumerable<ApplicationUser> listOfUsers;
        private IEnumerable<Project> listOfProjects;
        private ApplicationDbContext applicationDbContext;


        [SetUp]
        public void Setup()
        {
            ///Lists of data for adding entities to the in memory DB.
            listOfSubtasks = new List<Subtask>()
            {
                new Subtask(){Id = 100, Description = "TestTaskDescription1", ProjectId = 1, StatusId = 1, Name = "Test task1", CategoryId = 1},
                new Subtask(){Id = 200, Description = "TestTaskDescription2", ProjectId = 1, StatusId = 1, Name = "Test task2", CategoryId = 1},
                new Subtask(){Id = 300, Description = "TestTaskDescription3", ProjectId = 1, StatusId = 1, Name = "Test task3", CategoryId = 1}
            };
            listOfUsers = new List<ApplicationUser>()
            {
             new ApplicationUser(){ 
                 Id = "TestUser1", 
                 Email = "test@test.UnitTest", 
                 EmailConfirmed = true, 
                 Name = "TestName1", 
                 Surname = "TestUserSurname1", 
                 UserName = "testUserName1", 
                 NormalizedUserName = "TESTUSERNAME1", 
                 PasswordHash = "312312132" },
             new ApplicationUser(){
                 Id = "TestUser2",
                 Email = "ttt@ttttt.ttt",
                 EmailConfirmed = true,
                 Name = "TestName1",
                 Surname = "TestUserSurname2",
                 UserName = "testUserName2",
                 NormalizedUserName = "TESTUSERNAME2",
                 PasswordHash = "312312132sdasad" },
             new ApplicationUser(){
                 Id = "TestUser3",
                 Email = "ttt@tttttttt.tt",
                 EmailConfirmed = true,
                 Name = "TestName3",
                 Surname = "TestUserSurname3",
                 UserName = "testUserName3",
                 NormalizedUserName = "TESTUSERNAME3",
                 PasswordHash = "312312sdasad" },
            };
            listOfProjects = new List<Project>()
            {
                new Project(){Id = 100,Name = "Test Project1", Description = "TestProjectDescription1", ProjectManagerId = "123", Picture = "TestUrl" },
                new Project(){Id = 200,Name = "Test Project2", Description = "TestProjectDescription2", ProjectManagerId = "123", Picture = "TestUrl" },
                new Project(){Id = 300,Name = "Test Project3", Description = "TestProjectDescription3", ProjectManagerId = "123", Picture = "TestUrl" },
            };

            ///Set options for the inmemory DB.
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("PMSystemDB")
                .Options;

            ///Create the inmemory DB and append the lists with data from above.
            applicationDbContext = new ApplicationDbContext(contextOptions);
            applicationDbContext.Database.EnsureDeleted();
            applicationDbContext.Database.EnsureCreated();
            this.applicationDbContext.AddRange(listOfSubtasks);
            // var users = this.applicationDbContext.Subtasks.Local.ToList();
            this.applicationDbContext.AddRange(listOfUsers);
            // var users = this.applicationDbContext.Users.Local.ToList();
            this.applicationDbContext.AddRange(listOfProjects);
            // var users = this.applicationDbContext.Projects.Local.ToList();

        }
        //public async Task AddSubtaskAsync(AddSubtaskViewModel model)
        [Test]
        public async Task TestSubtaskAdding()
        {
            ISubtaskService service = new SubtaskService(applicationDbContext);
            var model = new AddSubtaskViewModel() { Description = "TestTaskDescription4", ProjectId = 1, StatusId = 1, Name = "Test task5", CategoryId = 1 };
            await service.AddSubtaskAsync(model);
            var testList = await this.applicationDbContext.Subtasks.ToListAsync();

            Assert.That(testList.Count, Is.EqualTo(4));
        }

        [TearDown]
        public void TearDown()
        {
            applicationDbContext.Dispose();
        }
    }
}