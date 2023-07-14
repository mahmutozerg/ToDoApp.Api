using Microsoft.EntityFrameworkCore;
using TodoAPP.Core;
using TodoAPP.Core.DTOs;
using TodoAPP.Core.Mappers;
using TodoAPP.Core.Models;
using TodoAPP.Core.Repositories;
using TodoAPP.Core.Services;
using TodoAPP.Core.UnitOfWorks;

namespace TodoAPP.Service.Services;

public class ToDoService:Service<ToDo>,IToDoService
{
    //todo: System exception vs db exception farknı araştır
    private readonly IToDoRepository _toDoRepository;
    private readonly IUnitOfWork _unitOfWork;


    public async Task<CustomResponseListDataDto<ToDoResponseDto>> GetAllAsync(string userId)
    {
        
        var todos = await _toDoRepository.Where(t=> t!.UserId == userId && !t.IsDeleted).AsNoTracking().ToListAsync();
        
        return CustomResponseListDataDto<ToDoResponseDto>.Success(TodoMapper.ToResponseDtoList(todos),200);
    }
    public async Task<CustomResponseListDataDto<ToDoResponseDto>> GetBylistNameAsync(string listName, string userId)
    {
        
        var todos =  await _toDoRepository.GetByListNameAsync(listName,userId);
        return CustomResponseListDataDto<ToDoResponseDto>.Success(TodoMapper.ToResponseDtoList(todos),200);
    }
    
    public async Task<CustomResponseListDataDto<ToDoResponseDto>> GetCompletedToDos(string userId)
    {

        var completedToDo = await _toDoRepository.Where(t => t!.UserId == userId && t.IsDone &&!t.IsDeleted).ToListAsync();

        return  CustomResponseListDataDto<ToDoResponseDto>.Success(TodoMapper.ToResponseDtoList(completedToDo),200);
    }
    public async Task<CustomResponseDto<ToDoResponseDto>> Update(TodoDto todoDto, string id, string userId)
    {


        var toDo = await _toDoRepository.Where(t => t!.UserId == userId && t.Id == id &&!t.IsDeleted).FirstOrDefaultAsync();

        if (toDo != null)
        {
            toDo.TodoContext = todoDto.TodoContext == string.Empty ? toDo.TodoContext : todoDto.TodoContext;
            toDo.Title = todoDto.Title == string.Empty ? toDo.Title : todoDto.Title;
            toDo.ListName = todoDto.ListName == string.Empty ? toDo.ListName : todoDto.ListName;
            toDo.DueTime = todoDto.DueTime == toDo.DueTime ? toDo.DueTime : todoDto.DueTime;
            await Update(toDo);
            return  CustomResponseDto<ToDoResponseDto>.Success(TodoMapper.ToResponseDto(toDo),200);

        }
        throw new Exception(ResponseMessages.SessionInvalidAccesToDo);
    }
    public async Task<CustomResponseDto<ToDoResponseDto>> UpdateToDoStatusById(string userId, string id)
    {
        // get the todoo by id and update its status

        var toDo = await _toDoRepository.Where(t => t!.UserId == userId && t.Id == id && !t.IsDeleted)
            .FirstOrDefaultAsync();
        
        if (toDo == null)
            throw new Exception(ResponseMessages.SessionInvalidAccesToDo);
        
        toDo.IsDone = !toDo.IsDone;
        await Update(toDo);
        return  CustomResponseDto<ToDoResponseDto>.Success(TodoMapper.ToResponseDto(toDo),200);
    }
    public async Task<CustomResponseDto<ToDoResponseDto>> CreateToDo(TodoDto todoDto, string sessionId)
    {
 
        // creates a new  todoo entity and returns it
        var toDo  = new ToDo()
        {
            ListName = todoDto.ListName,
            CreatedAt = DateTime.Now,
            DueTime = todoDto.DueTime,
            Title = todoDto.ListName,
            TodoContext = todoDto.TodoContext,
            IsDone = false,
            IsDeleted = false
        };
        await  _unitOfWork.CommitAsync();
        return  CustomResponseDto<ToDoResponseDto>.Success(TodoMapper.ToResponseDto(toDo),200);
    }
    
    public   async Task<CustomResponseNoDataDto> Remove(string userId, string id)
    {

        
        var todo = await _toDoRepository.Where(t => t!.UserId == userId && t.Id == id && !t.IsDeleted).FirstOrDefaultAsync();
        
        if (todo != null)
        {
            todo.IsDeleted = true;
            await Update(todo);
            return CustomResponseNoDataDto.Success(204);
        }

        throw new Exception(ResponseMessages.SessionInvalidAccesToDo);
    }


    public ToDoService(IGenericRepository<ToDo?> repository, IUnitOfWork unitOfWork, IToDoRepository toDoRepository) : base(repository, unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _toDoRepository = toDoRepository;
    }
}