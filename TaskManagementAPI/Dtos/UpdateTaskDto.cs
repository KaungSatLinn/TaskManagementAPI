using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.Dtos
{
    public class UpdateTaskDto
    {
        public int TaskId { get; set; }
        [Required]
        [StringLength(Helper.Constants.MaxTitleLength, ErrorMessage = Helper.Messages.TitleTooLong)]
        public string Title { get; set; }
        public string Description { get; set; }
        public int PriorityId { get; set; }
        public DateTime? DueDate { get; set; }
        public int StatusId { get; set; }
    }
}
