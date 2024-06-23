using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementAPI.Models
{
    public class Tasks
    {
        [Key]
        public int TaskId { get; set; }
        [StringLength(Helper.Constants.MaxTitleLength, ErrorMessage = Helper.Messages.TitleTooLong)]
        public required string Title { get; set; }
        public string? Description { get; set; }
        public int? PriorityId { get; set; }
        [ForeignKey("PriorityId")]
        public virtual TaskPriorities TaskPriorities { get; set; }
        public DateOnly? DueDate { get; set; }
        public int? StatusId { get; set; }
        [ForeignKey("StatusId")]
        public virtual TaskStatuses TaskStatuses { get; set; }
        public int IsDelete { get; set; }
    }
}
