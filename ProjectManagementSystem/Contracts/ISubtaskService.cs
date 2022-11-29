using ProjectManagementSystem.Data.Models;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Contracts
{
    public interface ISubtaskService
    {
        Task AddSubtaskAsync(AddSubtaskViewModel model);

        Task<IEnumerable<Status>> GetStatusesAsync();

        Task<IEnumerable<Project>> GetProjectsAsync();

        Task<SubtaskViewModel> GetSubtaskAsync(int subtaskId);
    }
}
