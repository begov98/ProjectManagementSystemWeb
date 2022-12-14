using ProjectManagementSystem.Data.Models;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Contracts
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectViewModel>> GetAllAsync();

        Task<IEnumerable<ApplicationUser>> GetProjectManagersAsync();

        Task AddProjectAsync(AddProjectViewModel model);

        Task <ProjectViewModel>GetProjectAsync(int projectId);

        Task<IEnumerable<SubtaskViewModel>> GetTasksAsync();

        Task DeleteProjectAsync(int projectId);

        Task<EditProjectViewModel> GetProjectEditInfoAsync(int projectId);

        Task EditProjectAsync(EditProjectViewModel model, int projectId);

    }
}
