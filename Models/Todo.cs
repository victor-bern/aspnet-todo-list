using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace todo.Models
{
    public class Todo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo é obrigátorio")]
        public string Title { get; set; }

        [DefaultValue(false)]
        public bool IsDone { get; set; }

        [Required(ErrorMessage = "Este campo é obrigátorio")]
        public int UserId { get; set; }
    }
}