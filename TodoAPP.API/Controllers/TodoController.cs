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

    public TodoController(IUserInfoService userInfoService, IUserInfoDetailedService userInfoDetailedService,
        IToDoService toDoService)
    {
        _userInfoService = userInfoService;
        _userInfoDetailedService = userInfoDetailedService;
        _toDoService = toDoService;
    }


    [HttpPost("[action]/{sessionId}")]

    public async Task<IActionResult> Add(TodoDto todoDto,string sessionId)
    {
        var user = await _userInfoService.GetUserInfoWithDetailsBySessionId(sessionId);
        if (user == null)
            return CreateActionResult(
                CustomResponseDto<NoContentDto>.Fail(StatusCodes.Status404NotFound, $"Sessionid {sessionId} does not exist"));

        var todoEntity = await _toDoService.Create(todoDto);
        todoEntity.UserInfo = user;
        user.ToDos.Add(todoEntity);
        await _userInfoService.Update(user);


        return CreateActionResult(
            CustomResponseDto<TodoDto>.Succes(StatusCodes.Status201Created, todoDto));

    }
    
    [HttpGet("[action]/{sessionId}")]
    public async Task<List<ToDo>> GetAll(string sessionId)
    {
        return await _toDoService.GetAllAsync(sessionId);
    }

    [HttpGet("[action]/{sessionId}/{ListName}")]
    public async Task<IActionResult> GetAllByListName(string sessionId, string ListName)
    {
        var user =await  _userInfoService.GetUserInfoWithDetailsBySessionId(sessionId);
        if(user == null)
            return CreateActionResult(
                CustomResponseDto<TodoDto>.Fail(StatusCodes.Status401Unauthorized,$"User does not exist or Logged out sessionid:{sessionId}"));

        
        var toDos = await _toDoService.GetBylistNameAsync(ListName,sessionId);
        if (toDos.Count <= 0)
            return CreateActionResult(
                CustomResponseDto<TodoDto>.Fail(StatusCodes.Status404NotFound,$"ListName: {ListName} does not exist"));
   
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
        return CreateActionResult(
            CustomResponseDto<ToDoResponseDto>.SuccesWithList(StatusCodes.Status200OK,toDoResponseDtos));
    }

    [HttpPost("[action]/{sessionId}/{id}")]
    public async Task<IActionResult> UpdateToDoStatusById(string sessionId,int id)
    {
        var user = await  _userInfoService.GetUserInfoWithDetailsBySessionId(sessionId);
        if (user == null)
            return CreateActionResult(
                CustomResponseDto<TodoDto>.Fail(StatusCodes.Status401Unauthorized,"User does not exist or Logged out"));


        var toDo = await _toDoService.UpdateToDoStatusById(sessionId,id);
        
        if(toDo == null)
            return CreateActionResult(
                CustomResponseDto<TodoDto>.Fail(StatusCodes.Status404NotFound,"ToDo does not exist"));
        var toDoResponseDto = new ToDoResponseDto()
        {
                
            Title = toDo.Title,
            TodoContext = toDo.TodoContext,
            DueTime = toDo.DueTime,
            IsDone = toDo.IsDone
        };

        return CreateActionResult(
            CustomResponseDto<ToDoResponseDto>.Succes(StatusCodes.Status200OK,toDoResponseDto));    }


    [HttpGet("[action]/{sessionId}")]
    public async Task<IActionResult> GetCompletedToDos(string sessionId)
    {
        var user =await  _userInfoService.GetUserInfoWithDetailsBySessionId(sessionId);
        if(user == null)
            return CreateActionResult(
                CustomResponseDto<TodoDto>.Fail(StatusCodes.Status401Unauthorized,"User does not exist or Logged out"));

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
        return CreateActionResult(
            CustomResponseDto<ToDoResponseDto>.SuccesWithList(StatusCodes.Status200OK,toDoResponseDtos));
    }
    
    [HttpPost("[action]/{sessionId}/{id}")]
    public async Task<IActionResult> UpdateToDo(string sessionId,TodoDto todoDto,int id)
    {
        var user = await _userInfoService.GetUserInfoWithDetailsBySessionId(sessionId);
        if (user == null)
            return CreateActionResult(
                CustomResponseDto<TodoDto>.Fail(StatusCodes.Status401Unauthorized,"User does not exist or Logged out"));


        var updatedTodo = await _toDoService.Update(todoDto,id,user.UserInfoDetailed.SessionGUID);
        if(updatedTodo == null)
            return CreateActionResult(
                CustomResponseDto<TodoDto>.Fail(StatusCodes.Status404NotFound,"ToDo id does not exist"));

        await _toDoService.Update(updatedTodo);
        
        return CreateActionResult(
            CustomResponseDto<ToDo>.Succes(StatusCodes.Status200OK,updatedTodo));   

    }
    
    
    [HttpPost("[action]/{sessionId}/{id}")]
    public async Task<IActionResult> Remove (string sessionId, int id)
    {
        var user = await _userInfoService.GetUserInfoWithDetailsBySessionId(sessionId);
        if (user == null)
            return CreateActionResult(
                CustomResponseDto<TodoDto>.Fail(StatusCodes.Status401Unauthorized,"User does not exist or Logged out"));


        var todo = await _toDoService.Remove(sessionId, id);
        if (todo == null)
            return CreateActionResult(
                CustomResponseDto<TodoDto>.Fail(StatusCodes.Status404NotFound,"ToDo id does not exist"));

        return CreateActionResult(
            CustomResponseDto<ToDo>.Succes(StatusCodes.Status200OK));   
        
    }

}