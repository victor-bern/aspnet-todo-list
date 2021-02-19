using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace todo.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo Obrigatorio")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Campo Obrigatorio")]
        public string Password { get; set; }
        public List<Todo> Todos { get; set; }

    }
}