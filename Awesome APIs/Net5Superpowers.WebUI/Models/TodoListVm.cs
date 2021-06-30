using System.Collections.Generic;

namespace Net5Superpowers.WebUI.Models
{
    public class TodoListVm
    {
        public IList<LookupDto> PriorityLevels { get; set; }
        public IList<TodoListDto> Lists { get; set; }
    }

}
