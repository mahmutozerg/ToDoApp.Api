using Microsoft.EntityFrameworkCore;
using TodoAPP.Core;
using TodoAPP.Core.DTOs;
using TodoAPP.Core.Models;
using TodoAPP.Core.Repositories;
using TodoAPP.Core.Services;
using TodoAPP.Core.UnitOfWorks;

namespace TodoAPP.Service.Services;

public class ToDoService:Service<ToDo>,IToDoService
{
    //todo: System exception vs db exception farknı araştır
    private readonly IUserService _userService;
    private readonly IToDoRepository _toDoRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly  ISessionService _sessionService;
    public ToDoService(IGenericRepository<ToDo> repository, IUnitOfWork unitOfWork, IToDoRepository toDoRepository, ISessionService sessionService, IUserService userService) : base(repository, unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _toDoRepository = toDoRepository;
        _sessionService = sessionService;
        _userService = userService;
    }

    public async Task<List<ToDo?>?> GetAllAsync(string sessionId)
    {
        var session = await _sessionService.GetSessionWithUserBySessionIdAsync(sessionId);
        if (session != null)
        {
            var todos = await _toDoRepository.Where(t=> t!.UserId == session.UserId && !t.IsDeleted).AsNoTracking().ToListAsync();
            return todos;
        }
        
        throw new Exception(ResponseMessages.SessionNotFound);
    }
    public async Task<List<ToDo>?> GetBylistNameAsync(string listName, string sessionId)
    {
        var session = await _sessionService.GetSessionWithUserBySessionIdAsync(sessionId);
        if (session == null)
        {
            throw new Exception(ResponseMessages.SessionNotFound);
        }
        
        var todos =  await _toDoRepository.GetBylistNameAsync(listName,session.UserId);
        return todos;
    }
    
    public async Task<List<ToDo?>?> GetCompletedToDos(string sessionId)
    {
        var session = await _sessionService.GetSessionWithUserBySessionIdAsync(sessionId);
        if (session == null)
        {
            throw new Exception(ResponseMessages.SessionNotFound);
        }

        var completedToDos = await _toDoRepository.Where(t => t!.UserId == session.UserId && t.IsDone &&!t.IsDeleted).ToListAsync();
        
        
        return completedToDos;
    }
    public async Task<ToDo?> Update(TodoDto todoDto, int id, string sessionId)
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
            return toDo;

        }
        throw new Exception(ResponseMessages.SessionInvalidAccesToDo);
    }
    public async Task<ToDo?> UpdateToDoStatusById(string sessionId,int id)
    {
        // get the todoo by id and update its status
        var session = await _sessionService.GetSessionWithUserBySessionIdAsync(sessionId);
        if (session == null)
        {
            throw new Exception(ResponseMessages.SessionNotFound);
        }

        var toDo = await _toDoRepository.Where(t => t!.UserId == session.UserId && t.Id == id && !t.IsDeleted)
            .FirstOrDefaultAsync();
        
        if (toDo != null)
        {
            toDo.IsDone = !toDo.IsDone;
            await Update(toDo);
            return toDo;
        }
        throw new Exception(ResponseMessages.SessionInvalidAccesToDo);
    }
    public async Task<ToDo?> CreateToDo(TodoDto todoDto, string sessionId)
    {
        var session = await _sessionService.GetSessionWithUserBySessionIdAsync(sessionId);
        if (session == null)
        {
            throw new Exception(ResponseMessages.SessionNotFound);
        }
        
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
        toDo.User = session.User;
        session.User.ToDos.Add(toDo);

        await _userService.Update(session.User);
        await  _unitOfWork.CommitAsync();
        return toDo;
    }
    
    public   async Task<ToDo?> Remove(string sessionId,int id)
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
            return todo;
        }

        throw new Exception(ResponseMessages.SessionInvalidAccesToDo);
    }

 
}