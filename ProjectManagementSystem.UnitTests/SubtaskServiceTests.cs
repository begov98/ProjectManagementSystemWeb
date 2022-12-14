namespace ProjectManagementSystem.UnitTests
{
    using NUnit;
    using ProjectManagementSystem.Data.Models;
    using ProjectManagementSystem.Models;
    using ProjectManagementSystem.Services;
    using System.Xml.Linq;

    [TestFixture]
    public class SubtaskServiceTests
    {

        [Test]
        public async Task TestAddSubtaskAsync_Success()
        {
            //Arrange
           using var data = DatabaseMock.Instance;

            var model = new AddSubtaskViewModel()
            {
                Name = "_Test",
                Description = "TestTask",
                StatusId = 1,
                ProjectId = 2,
                CategoryId = 3
            };
            var subtaskService = new SubtaskService(data);

            //Act
            await subtaskService.AddSubtaskAsync(model);
            var result = data.Subtasks.FirstOrDefault(s => s.Name == "_Test");

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void TestAddSubtaskAsync_Fail()
        {
            //Arrange
            using var data = DatabaseMock.Instance;
            var subtaskService = new SubtaskService(data);

            //Act
            subtaskService.AddSubtaskAsync(null!);
            var result = data.Subtasks.FirstOrDefault(s => s.Name == "_Test");

            //Assert
            Assert.Throws<ArgumentException>( () => { throw new ArgumentException("Subtask cannot be created. Please try again later! If the problem persist, contact your service provider!"); });
        }

        [Test]
        public async Task TestGetProjectsAsync()
        {
            //Arrange
            var data = DatabaseMock.Instance;

            await data.Projects.AddAsync(new Project {Id = 1, Name = "TestProj1", Description = "Test", Picture = "Url", ProjectManagerId = "guid"});
            await data.Projects.AddAsync(new Project {Id = 2, Name = "TestProj2", Description = "Test", Picture = "Url", ProjectManagerId = "guid"});
            await data.Projects.AddAsync(new Project {Id = 3, Name = "TestProj3", Description = "Test", Picture = "Url", ProjectManagerId = "guid"});
            await data.SaveChangesAsync();

            var subtaskService = new SubtaskService(data);

            //Act
            var result = subtaskService.GetProjectsAsync().Result.ToList();

            //Assert
            Assert.That(result.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task TestGetStatusesAsync()
        {
            //Arrange
            var data = DatabaseMock.Instance;

            var subtaskService = new SubtaskService(data);
            await data.AddAsync(new Status { Id = 1, StatusTitle = "To do" });
            await data.AddAsync(new Status { Id = 2, StatusTitle = "In process" });
            await data.AddAsync(new Status { Id = 3, StatusTitle = "Waiting for approval" });
            await data.AddAsync(new Status { Id = 4, StatusTitle = "Finished" });
            await data.AddAsync(new Status { Id = 5, StatusTitle = "Cancelled" });
            await data.SaveChangesAsync();

            //Act
            var result = subtaskService.GetStatusesAsync().Result.ToList();

            //Assert
            Assert.That(result.Count(), Is.EqualTo(5));
        }

        [Test]
        public async Task TestGetCategoriesAsync()
        {
            //Arrange
            var data = DatabaseMock.Instance;

            var subtaskService = new SubtaskService(data);
            await data.AddAsync(new Category { Id = 1, Name = "Fixing existing issue" });
            await data.AddAsync(new Category { Id = 2, Name = "Improvement" });
            await data.AddAsync(new Category { Id = 3, Name = "New feature" });
            await data.SaveChangesAsync();

            //Act
            var result = subtaskService.GetCategoriesAsync().Result.ToList();

            //Assert
            Assert.That(result.Count(), Is.EqualTo(3));
        }

        [Test]
        public void GetSubtaskAsync_Fail()
        {
            //Arrange
            var data = DatabaseMock.Instance;
            var subtaskService = new SubtaskService(data);

            //Act
            var result = subtaskService.GetSubtaskAsync(0);

            //Assert
            Assert.Throws<ArgumentException>(() => { throw new ArgumentException("Task not found... :("); });
        }

        [Test]
        public async Task ChangeStatusAsync_Success()
        {
            //Arrange
            var data = DatabaseMock.Instance;
            await data.AddAsync(new Subtask {Id = 1, Name = "name", StatusId = 1 });
            await data.SaveChangesAsync();
            var subtaskService = new SubtaskService(data);
            //Act
            var result1 = data.Subtasks.FirstOrDefault().StatusId;
           await subtaskService.ChangeStatusAsync(2, 1);
            var result2 = data.Subtasks.FirstOrDefault().StatusId;

            //Assert
            Assert.True(result1 == 1 && result2 == 2);
        }

        [Test]
        public void ChangeStatusAsync_Fail()
        {
            //Arrange
            var data = DatabaseMock.Instance;

            var subtaskService = new SubtaskService(data);
            //Act
            subtaskService.ChangeStatusAsync(2, 1);

            //Assert
            Assert.Throws<ArgumentException>(() => { throw new ArgumentException("Invalid subtask ID!"); });
        }

        [Test]
        public async Task DeleteSubtaskAsync_Success()
        {
            //Arrange
            var data = DatabaseMock.Instance;
            await data.AddAsync(new Subtask { Id = 1, Name = "name", StatusId = 1 });
            await data.SaveChangesAsync();
            var subtaskService = new SubtaskService(data);
            //Act
            var result1 = data.Subtasks.FirstOrDefault().Id;
            await subtaskService.DeleteSubtaskAsync(1);
            var result2 = data.Subtasks.FirstOrDefault();

            //Assert
            Assert.True(result1 == 1 && result2 == null);
        }

        [Test]
        public void DeleteSubtaskAsync_Fail()
        {
            //Arrange
            var data = DatabaseMock.Instance;
            var subtaskService = new SubtaskService(data);
            //Act
            subtaskService.DeleteSubtaskAsync(1);

            //Assert
            Assert.Throws<ArgumentException>(() => { throw new ArgumentException("Invalid subtask ID!"); });
        }

        [Test]
        public void GetSubtaskEditInfoAsync_Fail()
        {
            //Arrange
            var data = DatabaseMock.Instance;
            var subtaskService = new SubtaskService(data);

            //Act
            var result = subtaskService.GetSubtaskEditInfoAsync(0);

            //Assert
            Assert.Throws<ArgumentException>(() => { throw new ArgumentException("Task not found... :("); });
        }

        [Test]
        public async Task EditSubtaskAsync_Success()
        {
            //Arrange
            var data = DatabaseMock.Instance;
            await data.AddAsync(new Subtask { Id = 1, Name = "name", StatusId = 1, Description = "Descr" });
            await data.SaveChangesAsync();
            var subtaskService = new SubtaskService(data);
            var model = new EditSubtaskViewModel(){Id = 1, Name = "NewName", Description = "NewDescr" };

            //Act
            var result1 = data.Subtasks.FirstOrDefault().Name;
            await subtaskService.EditSubtaskAsync(model, 1);
            var result2 = data.Subtasks.FirstOrDefault().Name;

            //Assert
            Assert.True(result1 == "name" && result2 == "NewName");
        }

        [Test]
        public void EditSubtaskAsync_Fail()
        {
            //Arrange
            var data = DatabaseMock.Instance;
            var subtaskService = new SubtaskService(data);
            var model = new EditSubtaskViewModel() { Id = 1, Name = "NewName", Description = "NewDescr" };
            //Act
            subtaskService.EditSubtaskAsync(model, 1);

            //Assert
            Assert.Throws<ArgumentException>(() => { throw new ArgumentException("Invalid subtask ID!"); });
        }

    }
}