using TodoList.Basic.Contracts;
using TodoList.Basic.Models;

namespace TodoList.Basic.Interfaces
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoModel>> GetAllAsync();
        Task<TodoModel> GetByIdAsync(Guid id);
        Task CreateTodoAsync(CreateTodoRequest request);
        Task UpdateTodoAsync(Guid id, UpdateTodoRequest request);
        Task DeleteTodoAsync(Guid id);
    }
}
