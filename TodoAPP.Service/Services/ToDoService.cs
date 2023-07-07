using Microsoft.EntityFrameworkCore;
using TodoAPP.Core;
using TodoAPP.Core.DTOs;
using TodoAPP.Core.Mappers;
using TodoAPP.Core.Models;
using TodoAPP.Core.Repositories;
using TodoAPP.Core.Services;
using TodoAPP.Core.UnitOfWorks;

namespace TodoAPP.Service.Services;

public class ToDoService:Service<ToDoEntity>,IToDoService
{
    //todo: System exception vs db exception farknı araştır
    private readonly IUserService _userService;
    private readonly IToDoRepository _toDoRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly  ISessionService _sessionService;
    public ToDoService(IGenericRepository<ToDoEntity> repository, IUnitOfWork unitOfWork, IToDoRepository toDoRepository, ISessionService sessionService, IUserService userService) : base(repository, unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _toDoRepository = toDoRepository;
        _sessionService = sessionService;
        _userService = userService;
    }

    public async Task<CustomResponseListDataDto<ToDoResponseDto>> GetAllAsync(string sessionId)
    {
        var session = await _sessionService.GetSessionWithUserBySessionIdAsync(sessionId);
        
        if (session == null)
            throw new Exception(ResponseMessages.SessionNotFound);

        var todos = await _toDoRepository.Where(t=> t!.UserId == session.UserId && !t.IsDeleted).AsNoTracking().ToListAsync();
        
        return CustomResponseListDataDto<ToDoResponseDto>.Success(TodoMapper.ToResponseDtoList(todos),200);
    }
    public async Task<CustomResponseListDataDto<ToDoResponseDto>> GetBylistNameAsync(string listName, string sessionId)
    {
        var session = await _sessionService.GetSessionWithUserBySessionIdAsync(sessionId);
        if (session == null)
        {
            throw new Exception(ResponseMessages.SessionNotFound);
        }
        
        var todos =  await _toDoRepository.GetBylistNameAsync(listName,session.UserId);
        return CustomResponseListDataDto<ToDoResponseDto>.Success(TodoMapper.ToResponseDtoList(todos),200);
    }
    
    public async Task<CustomResponseListDataDto<ToDoResponseDto>> GetCompletedToDos(string sessionId)
    {
        var session = await _sessionService.GetSessionWithUserBySessionIdAsync(sessionId);
        if (session == null)
        {
            throw new Exception(ResponseMessages.SessionNotFound);
        }

        var completedToDo = await _toDoRepository.Where(t => t!.UserId == session.UserId && t.IsDone &&!t.IsDeleted).ToListAsync();

        return  CustomResponseListDataDto<ToDoResponseDto>.Success(TodoMapper.ToResponseDtoList(completedToDo),200);
    }
    public async Task<CustomResponseDto<ToDoResponseDto>> Update(TodoDto todoDto, string id, string sessionId)
    {
        var session = await _sessionService.GetSessionWithUserBySessionIdAsync(sessionId);
        if (session == null)
        {
            throw new Exception(ResponseMessages.SessionNotFound);
        }

        var toDo = await _toDoRepository.Where(t => t!.UserId == session.UserId && t.Id == id &&!t.IsDeleted).FirstOrDefaultAsync();

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
    public async Task<CustomResponseDto<ToDoResponseDto>> UpdateToDoStatusById(string sessionId, string id)
    {
        // get the todoo by id and update its status
        var session = await _sessionService.GetSessionWithUserBySessionIdAsync(sessionId);
        if (session == null)
        {
            throw new Exception(ResponseMessages.SessionNotFound);
        }

        var toDo = await _toDoRepository.Where(t => t!.UserId == session.UserId && t.Id == id && !t.IsDeleted)
            .FirstOrDefaultAsync();
        
        if (toDo == null)
            throw new Exception(ResponseMessages.SessionInvalidAccesToDo);
        
        toDo.IsDone = !toDo.IsDone;
        await Update(toDo);
        return  CustomResponseDto<ToDoResponseDto>.Success(TodoMapper.ToResponseDto(toDo),200);
    }
    public async Task<CustomResponseDto<ToDoResponseDto>> CreateToDo(TodoDto todoDto, string sessionId)
    {
        var session = await _sessionService.GetSessionWithUserBySessionIdAsync(sessionId);
        if (session == null)
        {
            throw new Exception(ResponseMessages.SessionNotFound);
        }
        
        // creates a new  todoo entity and returns it
        var toDo  = new ToDoEntity()
        {
            ListName = todoDto.ListName,
            CreatedAt = DateTime.Now,
            DueTime = todoDto.DueTime,
            Title = todoDto.ListName,
            TodoContext = todoDto.TodoContext,
            IsDone = false,
            IsDeleted = false
        };
        toDo.UserEntity = session.UserEntity;
        session.UserEntity.ToDos.Add(toDo);

        await _userService.Update(session.UserEntity);
        await  _unitOfWork.CommitAsync();
        return  CustomResponseDto<ToDoResponseDto>.Success(TodoMapper.ToResponseDto(toDo),200);
    }
    
    public   async Task<CustomResponseNoDataDto> Remove(string sessionId, string id)
    {
        var session = await _sessionService.GetSessionWithUserBySessionIdAsync(sessionId);
        if (session == null)
        {
            throw new Exception(ResponseMessages.SessionNotFound);
        }
        
        var todo = await _toDoRepository.Where(t => t!.UserId == session.UserId && t.Id == id && !t.IsDeleted).FirstOrDefaultAsync();
        
        if (todo != null)
        {
            todo.IsDeleted = true;
            await Update(todo);
            return CustomResponseNoDataDto.Success(204);
        }

        throw new Exception(ResponseMessages.SessionInvalidAccesToDo);
    }

 
}