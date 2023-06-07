using TodoAPP.Core.DTOs;
using TodoAPP.Core.Models;
using TodoAPP.Core.Repositories;
using TodoAPP.Core.Services;
using TodoAPP.Core.UnitOfWorks;

namespace TodoAPP.Service.Services;

public class ToDoService:Service<ToDo>,IToDoService
{
    private readonly IToDoRepository _toDoRepository;
    private readonly IUnitOfWork _unitOfWork;
    public ToDoService(IGenericRepository<ToDo?> repository, IUnitOfWork unitOfWork, IToDoRepository toDoRepository) : base(repository, unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _toDoRepository = toDoRepository;
    }

    public async Task<List<ToDo>> GetAllAsync(string sessionId)
    {
        return await _toDoRepository.GetWithFullUserInfo(sessionId);
    }

    public Task<ToDo> Create(TodoDto todoDto)
    {
        var entity  = new ToDo()
        {
            ListName = todoDto.ListName,
            CreatedAt = DateTime.Now,
            DueTime = todoDto.DueTime,
            Title = todoDto.ListName,
            TodoContext = todoDto.TodoContext,
            IsDone = false,
            IsDeleted = false
        };
        return Task.FromResult(entity);
    }

    public async Task<List<ToDo>> GetBylistNameAsync(string listName, string sessionId)
    {
        return await _toDoRepository.GetBylistNameAsync(listName,sessionId);
    }

    public async Task<ToDo?> UpdateToDoStatusById(string sessionId,int id)
    {
        var toDo = await _toDoRepository.GetByIdAsync(sessionId,id);
        if (toDo != null)
        {
            toDo.IsDone = !toDo.IsDone;
            await Update(toDo);
        }
        return toDo;
    }

    public async Task<List<ToDo>> GetCompletedToDos(string sessionId)
    {
        var relatedUserToDos = await _toDoRepository.GetWithFullUserInfo(sessionId);
        var completedToDos = relatedUserToDos.Where(x=>x.UserInfo.UserInfoDetailed.SessionGUID == sessionId && x.IsDone && !x.IsDeleted );

        return completedToDos.ToList();
    }

    public async Task<ToDo?> Update(TodoDto todoDto, int id, string sessionId)
    {
        var toDo = await _toDoRepository.GetByIdAsync(sessionId, id);

        if (toDo != null)
        {
            toDo.TodoContext = todoDto.TodoContext == string.Empty ? toDo.TodoContext : todoDto.TodoContext;
            toDo.Title = todoDto.Title == string.Empty ? toDo.Title : todoDto.Title;
            toDo.ListName = todoDto.ListName == string.Empty ? toDo.ListName : todoDto.ListName;
            toDo.DueTime = todoDto.DueTime == toDo.DueTime ? toDo.DueTime : todoDto.DueTime;
        }

        return toDo;

    }

    public   async Task<ToDo> Remove(string sessionId,int id)
    {
        var todo = await _toDoRepository.GetWithFullUserInfoById(sessionId,id);
        if (todo != null)
        {
            todo.IsDeleted = true;
            await Update(todo);
        }

        return todo;
    }
}