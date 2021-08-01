using System;
using System.Collections.Generic;
using BlueWater.OrderManagement.Common.Contracts;
using Microsoft.AspNetCore.Mvc;


namespace BlueWater.OrderManagement.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TodoListController : ControllerBase
    {
        // In-memory TodoList
        private static readonly Dictionary<int, TodoItem> TodoStore = new Dictionary<int, TodoItem>();

        public TodoListController()
        {
            // Pre-populate with sample data
            if (TodoStore.Count == 0)
            {
                TodoStore.Add(1, new TodoItem() { Id = 1, Task = "Pick up groceries" });
                TodoStore.Add(2, new TodoItem() { Id = 2, Task = "Finish invoice report" });
                TodoStore.Add(3, new TodoItem() { Id = 3, Task = "Water plants" });
            }
        }

        /// <summary>
        /// Only Use to test Exception handling 
        /// </summary>
        /// <returns></returns>
        // GET: api/todolist
        [HttpGet]
        public IActionResult Get()
        {
            string test = null;
            throw new ArgumentNullException(nameof(test));
            //return Ok(TodoStore.Values);
        }
    }
}
