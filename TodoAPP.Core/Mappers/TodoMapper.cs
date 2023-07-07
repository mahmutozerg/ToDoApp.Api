using TodoAPP.Core.DTOs;
using TodoAPP.Core.Models;

namespace TodoAPP.Core.Mappers;

public static class TodoMapper
{
       public  static List<ToDoResponseDto> ToResponseDtoList(List<ToDoEntity?> toDos)
        {
            var toDoResponseDtos = new List<ToDoResponseDto>();
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
            return toDoResponseDtos;
        }

       public static ToDoResponseDto ToResponseDto(ToDoEntity? toDo)
       {
           var dto = (new ToDoResponseDto()
           {
               Id = toDo.Id,
               Title = toDo.Title,
               TodoContext = toDo.TodoContext,
               DueTime = toDo.DueTime,
               IsDone = toDo.IsDone
           });
           return dto;
       }

}