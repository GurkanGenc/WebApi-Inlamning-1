using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeskHelpAPI.Data
{
    public class Ticket
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public DateTime Changed { get; set; }

        [Required]
        public TicketStatus Status { get; set; }
    }

    public enum TicketStatus
    {
        //[Display(Name = "Not started")]
        notStarted,
        ongoing,
        closed
    }

}