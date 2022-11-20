using ProjectManagementSystem.Data.Models;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Contracts
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectViewModel>> GetAllAsync();

        Task<IEnumerable<ApplicationUser>> GetProjectManagersAsync();

        Task AddProjectAsync(AddProjectViewModel model);
    }
}
