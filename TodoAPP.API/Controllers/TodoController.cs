using Microsoft.AspNetCore.Mvc;
using TodoAPP.Core.DTOs;
using TodoAPP.Core.Services;

namespace TodoAPP.API.Controllers;

public class TodoController : CustomBaseController
{

    private readonly IToDoService _toDoService;

    public TodoController(IToDoService toDoService)
    {
        _toDoService = toDoService;
    }
    
    [HttpGet("[action]/{sessionId}")]
    public async Task<IActionResult> GetAll(string sessionId)
    {
        var todoResponseDtos= await _toDoService.GetAllAsync(sessionId);

        return CreateActionResult(todoResponseDtos);

    }

    [HttpGet("[action]/{sessionId}/{ListName}")]
    public async Task<IActionResult> GetAllByListName(string sessionId, string ListName)
    {
        var todoDtos = await _toDoService.GetBylistNameAsync(ListName,sessionId);
        
        return CreateActionResult(todoDtos);
        
    }
    
    [HttpGet("[action]/{sessionId}")]
    public async Task<IActionResult> GetCompletedToDos(string sessionId)
    {

        var todoDtos = await _toDoService.GetCompletedToDos(sessionId);

        return CreateActionResult(todoDtos);
    }

    [HttpPost("[action]/{sessionId}/{id}")]
    public async Task<IActionResult> UpdateToDo(string sessionId,TodoDto todoDto,int id)
    {
   
        var updatedTodo = await _toDoService.Update(todoDto,id,sessionId);

        
        return CreateActionResult(updatedTodo);

    }

    [HttpPost("[action]/{sessionId}/{id}")]
    public async Task<IActionResult> UpdateToDoStatusById(string sessionId,int id)
    {
        var updatedTodo = await _toDoService.UpdateToDoStatusById(sessionId,id);

        
        return CreateActionResult(updatedTodo);

    }
    
    [HttpPost("[action]/{sessionId}")]

    public async Task<IActionResult> Add(TodoDto todoDto,string sessionId)
    {

        var createdTodo = await _toDoService.CreateToDo(todoDto, sessionId);
        return CreateActionResult(createdTodo);

    }
    [HttpPost("[action]/{sessionId}/{id}")]
    public async Task<IActionResult> Remove (string sessionId, int id)
    {

        var todo = await _toDoService.Remove(sessionId, id);
        return CreateActionResult(todo);
    }

}