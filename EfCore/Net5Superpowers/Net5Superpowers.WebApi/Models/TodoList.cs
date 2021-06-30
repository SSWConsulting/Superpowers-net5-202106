using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Net5Superpowers.WebApi.Models
{
    public class TodoList
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Colour { get; set; }

        public ICollection<TodoItem> Items { get; set; } = new List<TodoItem>();
    }
}
