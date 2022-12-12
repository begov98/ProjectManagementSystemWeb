using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
using ProjectManagementSystem.Contracts;
using ProjectManagementSystem.Controllers;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Data.Models;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Services;
using System.Data.Common;

namespace ProjectManagementSystem.UnitTests
{
    [TestFixture]
    public class SubtaskServiceTests
    {
        public ISubtaskService _service;

        [Test]
        public void TestSubtaskAdding()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection).Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
            }
            using (var context = new ApplicationDbContext(options))
            {
                context.Projects.Add(new Project { Id = 1, Name = "TestProject", Description = "TestDescr", Picture = "url", ProjectManagerId = context.Users.FirstOrDefault().Id });
                context.SaveChanges();
            }

            var model = new AddSubtaskViewModel()
            {
                Name = "Test",
                Description = "Test",
                StatusId = 1,
                ProjectId = 1,
                CategoryId = 1
            };

            using (var context = new ApplicationDbContext(options))
            {
                _service = new SubtaskService(context);
                _service.AddSubtaskAsync(model);
                context.SaveChanges();
                var subtask = context.Subtasks.FirstOrDefault();
                Assert.That(subtask, Is.Not.Null);
            }
        }

    }
}