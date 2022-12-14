using ProjectManagementSystem.Data.Models;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystem.UnitTests
{
    internal class ProjectServiceTests
    {
        [Test]
        public async Task TestAddProjectAsync_Success()
        {
            //Arrange
            using var data = DatabaseMock.Instance;

            var model = new AddProjectViewModel()
            {
                Name = "_Test",
                Description = "TestTask",
                ProjectManagerId = "Guid",
                Picture = "URL",
            };
            var projectService = new ProjectService(data);

            //Act
            await projectService.AddProjectAsync(model);
            var result = data.Projects.FirstOrDefault(p => p.Name == "_Test");

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void TestAddProjectAsync_Fail()
        {
            //Arrange
            using var data = DatabaseMock.Instance;
            var projectService = new ProjectService(data);

            //Act
            projectService.AddProjectAsync(null!);
            var result = data.Projects.FirstOrDefault(p => p.Name == "_Test");

            //Assert
            Assert.Throws<ArgumentException>(() => { throw new ArgumentException("Project cannot be created. Please try again later! If the problem persist, contact your service provider!"); });
        }

        [Test]
        public void GetProjectAsync_Fail()
        {
            //Arrange
            var data = DatabaseMock.Instance;
            var projectService = new ProjectService(data);

            //Act
            var result = projectService.GetProjectAsync(0);

            //Assert
            Assert.Throws<ArgumentException>(() => { throw new ArgumentException("Project not found... :("); });
        }

        [Test]
        public async Task DeleteProjectAsync_Success()
        {
            //Arrange
            var data = DatabaseMock.Instance;
            await data.AddAsync(new Project { Id = 1, Name = "name", Picture = "Url", ProjectManagerId = "Guid"});
            await data.SaveChangesAsync();
            var projectService = new ProjectService(data);
            //Act
            var result1 = data.Projects.FirstOrDefault().Id;
            await projectService.DeleteProjectAsync(1);
            var result2 = data.Projects.FirstOrDefault();

            //Assert
            Assert.True(result1 == 1 && result2 == null);
        }

        [Test]
        public void DeleteProjectAsync_Fail()
        {
            //Arrange
            var data = DatabaseMock.Instance;
            var projectService = new ProjectService(data);

            //Act
            projectService.DeleteProjectAsync(1);

            //Assert
            Assert.Throws<ArgumentException>(() => { throw new ArgumentException("Project not found"); });
        }

        [Test]
        public void GetProjectEditInfoAsync_Fail()
        {
            //Arrange
            var data = DatabaseMock.Instance;
            var projectService = new ProjectService(data);

            //Act
            projectService.GetProjectEditInfoAsync(1);

            //Assert
            Assert.Throws<ArgumentException>(() => { throw new ArgumentException("Project not found... :("); });
        }

        [Test]
        public async Task EditProjectAsync_Success()
        {
            //Arrange
            var data = DatabaseMock.Instance;
            await data.AddAsync(new Project { Id = 1, Name = "name",Description = "Descr", ProjectManagerId = "Guid", Picture = "URL" });
            await data.SaveChangesAsync();
            var projectService = new ProjectService(data);
            var model = new EditProjectViewModel() { Id = 1, Name = "NewName", Description = "NewDescr", ProjectManagerId = "Guid", Picture = "URL" };

            //Act
            var result1 = data.Projects.FirstOrDefault().Name;
            await projectService.EditProjectAsync(model, 1);
            var result2 = data.Projects.FirstOrDefault().Name;

            //Assert
            Assert.True(result1 == "name" && result2 == "NewName");
        }
    }
}
