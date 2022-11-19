using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Contracts
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectViewModel>> GetAllAsync();

        Task AddProjectAsync(AddProjectViewModel model);
    }
}
