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
        var completedToDos = relatedUserToDos.Where(x=>x.UserInfo.UserInfoDetailed.SessionGUID == sessionId && x.IsDone == true && x.IsDeleted == false);

        return completedToDos.ToList();
    }
}