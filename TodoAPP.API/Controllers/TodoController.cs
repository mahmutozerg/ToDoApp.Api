using Microsoft.AspNetCore.Mvc;
using TodoAPP.Core;
using TodoAPP.Core.DTOs;
using TodoAPP.Core.Models;
using TodoAPP.Core.Services;

namespace TodoAPP.API.Controllers;

public class TodoController : CustomBaseController
{
    private readonly IUserService _userService;
    private readonly ISessionService _sessionService;
    private readonly IToDoService _toDoService;

    public TodoController(IUserService userService, ISessionService sessionService, IToDoService toDoService)
    {
        _userService = userService;
        _sessionService = sessionService;
        _toDoService = toDoService;
    }
    
    [HttpGet("[action]/{sessionId}")]
    public async Task<IActionResult> GetAll(string sessionId)
    {
        IActionResult actionResult;
        var entities= await _toDoService.GetAllAsync(sessionId);

        actionResult =  CreateActionResult(CustomResponseListDataDto<ToDo>.Success(StatusCodes.Status200OK,entities));
        return actionResult;

    }

    [HttpGet("[action]/{sessionId}/{ListName}")]
    public async Task<IActionResult> GetAllByListName(string sessionId, string ListName)
    {
        IActionResult actionResult;
        var toDos = await _toDoService.GetBylistNameAsync(ListName,sessionId);
        
        var toDoResponseDtos = _toDoService.ConvertToDoListToResponseDtoList(toDos);
        actionResult = CreateActionResult(
            CustomResponseListDataDto<ToDoResponseDto>.Success(StatusCodes.Status200OK,toDoResponseDtos));
        
        return actionResult;

    }
    
    [HttpGet("[action]/{sessionId}")]
    public async Task<IActionResult> GetCompletedToDos(string sessionId)
    {
        IActionResult actionResult;

        var toDos = await _toDoService.GetCompletedToDos(sessionId);
        var toDoResponseDtos = _toDoService.ConvertToDoListToResponseDtoList(toDos);

        actionResult = CreateActionResult(
            CustomResponseListDataDto<ToDoResponseDto>.Success(StatusCodes.Status200OK,toDoResponseDtos));
        return actionResult;
    }

    [HttpPost("[action]/{sessionId}/{id}")]
    public async Task<IActionResult> UpdateToDo(string sessionId,TodoDto todoDto,int id)
    {
        IActionResult actionResult;
   
        var updatedTodo = await _toDoService.Update(todoDto,id,sessionId);

        
        actionResult = CreateActionResult(
            CustomResponseDto<ToDo>.Success(StatusCodes.Status200OK,updatedTodo));
        return actionResult;

    }

    [HttpPost("[action]/{sessionId}/{id}")]
    public async Task<IActionResult> UpdateToDoStatusById(string sessionId,int id)
    {
        IActionResult actionResult;

        
        var toDo = await _toDoService.UpdateToDoStatusById(sessionId,id);

        var toDoResponseDto = _toDoService.ConvertToDoListToResponseDtoList(new List<ToDo> { toDo });
        
        actionResult = CreateActionResult(CustomResponseListDataDto<ToDoResponseDto>.Success(StatusCodes.Status200OK,toDoResponseDto));
        return actionResult;

    }
    
    [HttpPost("[action]/{sessionId}")]

    public async Task<IActionResult> Add(TodoDto todoDto,string sessionId)
    {
        IActionResult actionResult;
        await _toDoService.CreateToDo(todoDto,sessionId);
        
        actionResult = CreateActionResult(
            CustomResponseDto<TodoDto>.Success(StatusCodes.Status201Created, todoDto));
        return actionResult;

    }
    [HttpPost("[action]/{sessionId}/{id}")]
    public async Task<IActionResult> Remove (string sessionId, int id)
    {
        IActionResult actionResult;

        var todo = await _toDoService.Remove(sessionId, id);
        if (todo == null)
        {
            actionResult = CreateActionResult(
                CustomResponseNoDataDto.Fail(StatusCodes.Status404NotFound,ResponseMessages.ToDoDoesNotExist));
            return actionResult;

        }
        actionResult = CreateActionResult(
            CustomResponseNoDataDto.Success(StatusCodes.Status200OK));
        return actionResult;   
        
    }

}