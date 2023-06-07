using Microsoft.AspNetCore.Mvc;
using TodoAPP.Core.DTOs;
using TodoAPP.Core.Models;
using TodoAPP.Core.Services;

namespace TodoAPP.API.Controllers;

public class TodoController : CustomBaseController
{
    private readonly IUserInfoService _userInfoService;
    private readonly IUserInfoDetailedService _userInfoDetailedService;
    private readonly IToDoService _toDoService;

    public TodoController(IUserInfoService userInfoService, IUserInfoDetailedService userInfoDetailedService, IToDoService toDoService)
    {
        _userInfoService = userInfoService;
        _userInfoDetailedService = userInfoDetailedService;
        _toDoService = toDoService;
    }
    
    [HttpGet("[action]/{sessionId}")]
    public async Task<IActionResult> GetAll(string sessionId)
    {
        IActionResult actionResult;
        var user =await  _userInfoService.GetUserInfoWithDetailsBySessionId(sessionId);
        var entities= await _toDoService.GetAllAsync(sessionId);

        if (user == null)
        {
            actionResult =  CreateActionResult(CustomResponseNoDataDto.Fail(StatusCodes.Status404NotFound, $"Check session id:{sessionId}"));
            return actionResult;
        }

        actionResult =  CreateActionResult(CustomResponseListDataDto<ToDo>.Success(StatusCodes.Status200OK,entities));
        return actionResult;

    }

    [HttpGet("[action]/{sessionId}/{ListName}")]
    public async Task<IActionResult> GetAllByListName(string sessionId, string ListName)
    {
        IActionResult actionResult;
        string responseMessage = string.Empty;
        var user =await  _userInfoService.GetUserInfoWithDetailsBySessionId(sessionId);
        var toDos = await _toDoService.GetBylistNameAsync(ListName,sessionId);

        if (user == null)
            responseMessage += $"User does not exist or Logged out sessionid:{sessionId}\n";
        
        if (toDos.Count <= 0)
            responseMessage += $"ListName: {ListName} does not exist";

        if (!string.IsNullOrEmpty(responseMessage))
        {
            actionResult =
                CreateActionResult(CustomResponseNoDataDto.Fail(StatusCodes.Status404NotFound, responseMessage));
            return actionResult;
        }

        
        List<ToDoResponseDto> toDoResponseDtos = new List<ToDoResponseDto>();
        foreach (var toDo in toDos)
        {
            toDoResponseDtos.Add(new ToDoResponseDto()
            {
                Id = toDo.Id,
                Title = toDo.Title,
                TodoContext = toDo.TodoContext,
                DueTime = toDo.DueTime,
                IsDone = toDo.IsDone
            });
        }
        actionResult = CreateActionResult(
            CustomResponseListDataDto<ToDoResponseDto>.Success(StatusCodes.Status200OK,toDoResponseDtos));
        return actionResult;

    }
    
    [HttpGet("[action]/{sessionId}")]
    public async Task<IActionResult> GetCompletedToDos(string sessionId)
    {
        IActionResult actionResult;
        var user =await  _userInfoService.GetUserInfoWithDetailsBySessionId(sessionId);
        if (user == null)
        {
            actionResult = CreateActionResult(
                CustomResponseDto<TodoDto>.Fail(StatusCodes.Status401Unauthorized,"User does not exist or Logged out"));
            return actionResult;
        }
           

        var toDos = await _toDoService.GetCompletedToDos(sessionId);
        List<ToDoResponseDto> toDoResponseDtos = new List<ToDoResponseDto>();
        foreach (var toDo in toDos)
        {
            toDoResponseDtos.Add(new ToDoResponseDto()
            {
                Id = toDo.Id,
                Title = toDo.Title,
                TodoContext = toDo.TodoContext,
                DueTime = toDo.DueTime,
                IsDone = toDo.IsDone
            });
        }
        actionResult = CreateActionResult(
            CustomResponseListDataDto<ToDoResponseDto>.Success(StatusCodes.Status200OK,toDoResponseDtos));
        return actionResult;
    }

    [HttpPost("[action]/{sessionId}/{id}")]
    public async Task<IActionResult> UpdateToDo(string sessionId,TodoDto todoDto,int id)
    {
        IActionResult actionResult;
        var user = await _userInfoService.GetUserInfoWithDetailsBySessionId(sessionId);
        if (user == null)
        {
            actionResult = CreateActionResult(
                CustomResponseNoDataDto.Fail(StatusCodes.Status401Unauthorized,"User does not exist or Logged out"));
            return actionResult;
        }
        var updatedTodo = await _toDoService.Update(todoDto,id,user.UserInfoDetailed.SessionGUID);
        if (updatedTodo == null)
        {
            actionResult = CreateActionResult(
                CustomResponseNoDataDto.Fail(StatusCodes.Status404NotFound,"ToDo id does not exist"));
            return actionResult;
        }

        await _toDoService.Update(updatedTodo);
        
        actionResult = CreateActionResult(
            CustomResponseDto<ToDo>.Succes(StatusCodes.Status200OK,updatedTodo));
        return actionResult;

    }

    [HttpPost("[action]/{sessionId}/{id}")]
    public async Task<IActionResult> UpdateToDoStatusById(string sessionId,int id)
    {
        IActionResult actionResult;
        var user = await  _userInfoService.GetUserInfoWithDetailsBySessionId(sessionId);
        if (user == null)
        {
            actionResult = CreateActionResult(
                CustomResponseNoDataDto.Fail(StatusCodes.Status401Unauthorized,"User does not exist or Logged out"));
            return actionResult;
        }
        
        var toDo = await _toDoService.UpdateToDoStatusById(sessionId,id);

        if (toDo == null)
        {
            actionResult = CreateActionResult(
                CustomResponseNoDataDto.Fail(StatusCodes.Status404NotFound,"ToDo does not exist"));
            return actionResult;
        }
             
        var toDoResponseDto = new ToDoResponseDto()
        {
                
            Title = toDo.Title,
            TodoContext = toDo.TodoContext,
            DueTime = toDo.DueTime,
            IsDone = toDo.IsDone
        };

        actionResult = CreateActionResult(
            CustomResponseDto<ToDoResponseDto>.Succes(StatusCodes.Status200OK,toDoResponseDto));
        return actionResult;

    }
    
    [HttpPost("[action]/{sessionId}")]

    public async Task<IActionResult> Add(TodoDto todoDto,string sessionId)
    {
        IActionResult actionResult;
        var user = await _userInfoService.GetUserInfoWithDetailsBySessionId(sessionId);
        if (user == null)
        {
            actionResult = CreateActionResult(
                CustomResponseNoDataDto.Fail(StatusCodes.Status404NotFound, $"Sessionid {sessionId} does not exist"));
            return actionResult;
        }

        var todoEntity = await _toDoService.Create(todoDto);
        todoEntity.UserInfo = user;
        
        user.ToDos.Add(todoEntity);
        await _userInfoService.Update(user);
        
        actionResult = CreateActionResult(
            CustomResponseDto<TodoDto>.Succes(StatusCodes.Status201Created, todoDto));
        return actionResult;

    }
    [HttpPost("[action]/{sessionId}/{id}")]
    public async Task<IActionResult> Remove (string sessionId, int id)
    {
        IActionResult actionResult;
        var user = await _userInfoService.GetUserInfoWithDetailsBySessionId(sessionId);
        if (user == null)
        {
            actionResult =CreateActionResult(
                CustomResponseNoDataDto.Fail(StatusCodes.Status401Unauthorized,"User does not exist or Logged out"));
            return actionResult;

        }
        
        var todo = await _toDoService.Remove(sessionId, id);
        if (todo == null)
        {
            actionResult = CreateActionResult(
                CustomResponseNoDataDto.Fail(StatusCodes.Status404NotFound,"ToDo id does not exist"));
            return actionResult;

        }

        actionResult = CreateActionResult(
            CustomResponseDto<ToDo>.Succes(StatusCodes.Status200OK));
        return actionResult;   
        
    }

}