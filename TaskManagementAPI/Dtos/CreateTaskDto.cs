using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.Dtos
{
    public class CreateTaskDto
    {
        public int TaskId { get; set; }
        [Required]
        [StringLength(Helper.Constants.MaxTitleLength, ErrorMessage = Helper.Messages.TitleTooLong)]
        public string Title { get; set; }
        public string Description { get; set; }
        public int PriorityId { get; set; }
        public DateOnly? DueDate { get; set; }
        public int StatusId { get; set; }
    }
}
