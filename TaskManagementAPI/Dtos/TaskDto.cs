using System.ComponentModel.DataAnnotations;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Dtos
{
    public class TaskDto
    {
        public int TaskId { get; set; }
        [StringLength(Helper.Constants.MaxTitleLength, ErrorMessage = Helper.Messages.TitleTooLong)]
        public required string Title { get; set; }
        public string Description { get; set; } = string.Empty;
        public string PriorityName { get; set; }
        public DateOnly? DueDate { get; set; }
        public string StatusName { get; set; }
    }
}
