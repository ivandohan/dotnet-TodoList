using Microsoft.AspNetCore.Mvc;
using TodoList.Basic.Contracts;
using TodoList.Basic.Interfaces;

namespace TodoList.Basic.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodoAsync(CreateTodoRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _todoService.CreateTodoAsync(request);
                return Ok(new
                {
                    message = "New todo successfully created."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message = "An error occured while creating the todo item",
                        error = ex.Message
                    });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var todo = await _todoService.GetAllAsync();
                if (todo == null || !todo.Any())
                {
                    return Ok(new
                    {
                        message = "No todo items found."
                    });
                }

                return Ok(new
                {
                    message = "Successfully retrieve all todo items",
                    data = todo,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message = "An error occured while retrieving all todo items",
                        error = ex.Message
                    });
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            try
            {
                var todo = await _todoService.GetByIdAsync(id);
                if (todo == null)
                {
                    return NotFound(new
                    {
                        message = $"No todo item with id:{id} found."
                    });
                }

                return Ok(new
                {
                    message = $"Successfully retrieve todo item with id:{id}.",
                    data = todo,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message = $"An error occured while retrieving todo item with id:{id}.",
                        error = ex.Message,
                    });
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateTodoAsync(Guid id, UpdateTodoRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var todo = await _todoService.GetByIdAsync(id);
                if (todo == null)
                {
                    return NotFound(new
                    {
                        message = $"Todo item with id:{id} not found."
                    });
                }

                await _todoService.UpdateTodoAsync(id, request);
                return Ok(new
                {
                    message = $"Todo item with id:{id} successfully updated."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500, new
                    {
                        message = $"An error occured while updating todo item with id:{id}.",
                        error = ex.Message,
                    });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteTodoAsync(Guid id)
        {
            try
            {
                await _todoService.DeleteTodoAsync(id);
                return Ok(new
                {
                    message = $"Todo item with id:{id} successfully deleted."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500, new
                    {
                        message = $"An error occured while deleting todo item with id:{id}"
                    });
            }
        }
    }
}
