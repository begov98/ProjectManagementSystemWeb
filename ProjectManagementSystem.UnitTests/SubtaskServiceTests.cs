using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProjectManagementSystem.Contracts;
using ProjectManagementSystem.Controllers;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Data.Models;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Services;

namespace ProjectManagementSystem.UnitTests
{
    [TestFixture]
    public class SubtaskServiceTests
    {


        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task TestSubtaskAdding()
        {

        }


        [TearDown]
        public void TearDown()
        {
            //applicationDbContext.Dispose();
        }
    }
}