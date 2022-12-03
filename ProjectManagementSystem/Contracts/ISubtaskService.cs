using Microsoft.AspNetCore.Identity;
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

        Task<IEnumerable<ApplicationUser>> GetEmployeesAsync();

        Task AddSpecialistsToSubtask(string specialistId, int subtaskId);

        Task RemoveSpecialistsAsync(string specialistId, int subtaskId);

        Task ChangeStatusAsync(int statusId, int subtaskId);
    }
}
