using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoAPP.Core.DTOs;
using TodoAPP.Core.Services;

namespace TodoAPP.API.Controllers;


[Authorize]
public class TodoController : CustomBaseController
{

    private readonly IToDoService _toDoService;

    public TodoController(IToDoService toDoService)
    {
        _toDoService = toDoService;
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> GetAll()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var todoResponseDtos= await _toDoService.GetAllAsync(userId);

        return CreateActionResult(todoResponseDtos);

    }

    [HttpGet("[action]/{ListName}")]
    public async Task<IActionResult> GetAllByListName( string ListName)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var todoDtos = await _toDoService.GetBylistNameAsync(ListName,userId);
        
        return CreateActionResult(todoDtos);
        
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> GetCompletedToDos()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var todoDtos = await _toDoService.GetCompletedToDos(userId);

        return CreateActionResult(todoDtos);
    }

    [HttpPost("[action]/{id}")]
    public async Task<IActionResult> UpdateToDo(TodoDto todoDto,string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var updatedTodo = await _toDoService.Update(todoDto,id,userId);

        
        return CreateActionResult(updatedTodo);

    }

    [HttpPost("[action]/{id}")]
    public async Task<IActionResult> UpdateToDoStatusById(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var updatedTodo = await _toDoService.UpdateToDoStatusById(userId,id);

        
        return CreateActionResult(updatedTodo);

    }
    
    [HttpPost("[action]")]

    public async Task<IActionResult> Add(TodoDto todoDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var createdTodo = await _toDoService.CreateToDo(todoDto, userId);
        return CreateActionResult(createdTodo);

    }
    [HttpPost("[action]/{id}")]
    public async Task<IActionResult> Remove (string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var todo = await _toDoService.Remove(userId, id);
        return CreateActionResult(todo);
    }

}