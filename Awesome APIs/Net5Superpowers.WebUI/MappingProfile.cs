using AutoMapper;
using Net5Superpowers.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Net5Superpowers.WebUI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TodoList, TodoListDto>();
            CreateMap<TodoItem, TodoItemDto>();
        }
    }
}
