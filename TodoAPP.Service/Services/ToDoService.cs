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

    public async Task<IQueryable<ToDo>> GetAllAsync()
    {
        return await _toDoRepository.GetAllAsync();
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

    public async Task<List<ToDo>> GetWithFullUserInfo(string sessionId)
    {
        return await _toDoRepository.GetWithFullUserInfo(sessionId);
    }

    public async Task<ToDo?> UpdateToDoStatusById(int id)
    {
        var toDo = await _toDoRepository.GetByIdAsync(id);
        if (toDo != null)
        {
            toDo.IsDone = !toDo.IsDone;
            _toDoRepository.Update(toDo);
            await _unitOfWork.CommitAsync();
        }
        return toDo;
    }

    public async Task<List<ToDo>> GetCompletedToDos(string sessionId)
    {
        var relatedUserToDos = await _toDoRepository.GetWithFullUserInfo(sessionId);
        var completedToDos = relatedUserToDos.Where(x=>x.UserInfo.UserInfoDetailed.SessionGUID == sessionId && x.IsDone && !x.IsDeleted );

        return completedToDos.ToList();
    }

    public  Task<ToDo?> Update(TodoDto todoDto, int id, UserInfo user)
    {
        var toDo = _toDoRepository.Where(x => x.Id == id).FirstOrDefault();

        if (toDo != null)
        {
            toDo.TodoContext = todoDto.TodoContext == string.Empty ? toDo.TodoContext : todoDto.TodoContext;
            toDo.Title = todoDto.Title == string.Empty ? toDo.Title : todoDto.Title;
            toDo.ListName = todoDto.ListName == string.Empty ? toDo.ListName : todoDto.ListName;
            toDo.DueTime = todoDto.DueTime == toDo.DueTime ? toDo.DueTime : todoDto.DueTime;
        }
        
        return Task.FromResult(toDo);

    }
    
}