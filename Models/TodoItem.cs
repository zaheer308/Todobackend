using System;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;


namespace TodoBackend.Models
{

    [Table("todolist")]
    public class TodoItem
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public int Userid { get; set; }

        public DateTime Datetime { get; set; }

    }
}