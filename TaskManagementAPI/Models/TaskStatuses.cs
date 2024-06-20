using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.Models
{
    public class TaskStatuses
    {
        [Key]
        public int StatusId { get; set; }
        [StringLength(Helper.Constants.MaxLength50, ErrorMessage = Helper.Messages.TitleTooLong)]
        public required string StatusName { get; set; }
    }
}
