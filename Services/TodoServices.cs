using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoList.AppDataContext;
using TodoList.Basic.Contracts;
using TodoList.Basic.Interfaces;
using TodoList.Basic.Models;

namespace TodoList.Basic.Services
{
    public class TodoServices : ITodoService
    {
        private readonly TodoDbContext _context;
        private readonly ILogger<TodoServices> _logger;
        private readonly IMapper _mapper;

        public TodoServices(TodoDbContext context, ILogger<TodoServices> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }




        //  Create Todo for it be save in the datbase 

        public async Task CreateTodoAsync(CreateTodoRequest request)
        {
            try
            {
                var todo = _mapper.Map<TodoModel>(request);
                todo.CreatedAt = DateTime.Now;
                _context.Todos.Add(todo);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the todo item.");
                throw new Exception("An error occurred while creating the todo item.");
            }
        }

        public async Task<IEnumerable<TodoModel>> GetAllAsync()
        {
            var todo = await _context.Todos.ToListAsync();
            if (todo == null)
            {
                throw new Exception(" No Todo items found");
            }
            return todo;

        }
        public async Task DeleteTodoAsync(Guid id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                throw new Exception($"No todo item with id:{id} found.");
            }
            else
            {
                _context.Todos.Remove(todo);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<TodoModel> GetByIdAsync(Guid id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                throw new KeyNotFoundException($"No todo item with id:{id} found.");
            }

            return todo;
        }

        public async Task UpdateTodoAsync(Guid id, UpdateTodoRequest request)
        {
            try
            {
                var todo = await _context.Todos.FindAsync(id);
                if (todo == null)
                {
                    throw new Exception($"Todo item with id:{id} not found.");
                }

                if (request.Title != null)
                {
                    todo.Title = request.Title;
                }

                if (request.Description != null)
                {
                    todo.Description = request.Description;
                }

                if (request.IsCompleted != null)
                {
                    todo.IsComplete = request.IsCompleted.Value;
                }

                if (request.DueDate != null)
                {
                    todo.DueDate = request.DueDate.Value;
                }

                if (request.Priority != null)
                {
                    todo.Priority = request.Priority.Value;
                }

                todo.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occured while updating todo item with id:{id}.");
                throw;
            }
        }
    }
}
