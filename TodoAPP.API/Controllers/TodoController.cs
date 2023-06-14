using Microsoft.AspNetCore.Mvc;
using TodoAPP.Core;
using TodoAPP.Core.DTOs;
using TodoAPP.Core.Mappers;
using TodoAPP.Core.Models;
using TodoAPP.Core.Services;

namespace TodoAPP.API.Controllers;

public class TodoController : CustomBaseController
{

    private readonly IToDoService _toDoService;

    // todo: Action resultları diret returnla var diyip
    public TodoController(IToDoService toDoService)
    {
        _toDoService = toDoService;
    }
    
    [HttpGet("[action]/{sessionId}")]
    public async Task<IActionResult> GetAll(string sessionId)
    {
        var entities= await _toDoService.GetAllAsync(sessionId);

        return CreateActionResult(CustomResponseListDataDto<ToDo>.Success(StatusCodes.Status200OK,entities));

    }

    [HttpGet("[action]/{sessionId}/{ListName}")]
    public async Task<IActionResult> GetAllByListName(string sessionId, string ListName)
    {
        var toDos = await _toDoService.GetBylistNameAsync(ListName,sessionId);
        
        var toDoResponseDtos = TodoMapper.ToResponseDtoList(toDos);
        return CreateActionResult(
            CustomResponseListDataDto<ToDoResponseDto>.Success(StatusCodes.Status200OK,toDoResponseDtos));
        
    }
    
    [HttpGet("[action]/{sessionId}")]
    public async Task<IActionResult> GetCompletedToDos(string sessionId)
    {

        var toDos = await _toDoService.GetCompletedToDos(sessionId);
        var toDoResponseDtos = TodoMapper.ToResponseDtoList(toDos);

        return CreateActionResult(
            CustomResponseListDataDto<ToDoResponseDto>.Success(StatusCodes.Status200OK,toDoResponseDtos));
    }

    [HttpPost("[action]/{sessionId}/{id}")]
    public async Task<IActionResult> UpdateToDo(string sessionId,TodoDto todoDto,int id)
    {
   
        var updatedTodo = await _toDoService.Update(todoDto,id,sessionId);

        
        return CreateActionResult(
            CustomResponseDto<ToDo>.Success(StatusCodes.Status200OK,updatedTodo));

    }

    [HttpPost("[action]/{sessionId}/{id}")]
    public async Task<IActionResult> UpdateToDoStatusById(string sessionId,int id)
    {
        var toDo = await _toDoService.UpdateToDoStatusById(sessionId,id);

        var toDoResponseDto = TodoMapper.ToResponseDtoList(new List<ToDo> { toDo });
        
        return CreateActionResult(CustomResponseListDataDto<ToDoResponseDto>.Success(StatusCodes.Status200OK,toDoResponseDto));

    }
    
    [HttpPost("[action]/{sessionId}")]

    public async Task<IActionResult> Add(TodoDto todoDto,string sessionId)
    {
        await _toDoService.CreateToDo(todoDto,sessionId);
        
        return CreateActionResult(
            CustomResponseDto<TodoDto>.Success(StatusCodes.Status201Created, todoDto));

    }
    [HttpPost("[action]/{sessionId}/{id}")]
    public async Task<IActionResult> Remove (string sessionId, int id)
    {

        var todo = await _toDoService.Remove(sessionId, id);
        if (todo == null)
        { 
            return CreateActionResult(
                CustomResponseNoDataDto.Fail(StatusCodes.Status404NotFound,ResponseMessages.ToDoDoesNotExist));
        }
        return CreateActionResult(
            CustomResponseNoDataDto.Success(StatusCodes.Status200OK));
    }

}