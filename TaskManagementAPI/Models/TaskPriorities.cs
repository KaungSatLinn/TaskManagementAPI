using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.Models
{
    public class TaskPriorities
    {
        [Key]
        public int PriorityId { get; set; }
        [StringLength(Helper.Constants.MaxLength50, ErrorMessage = Helper.Messages.TitleTooLong)]
        public required string PriorityName { get; set; }
    }
}
