using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models
{
    public class TicketModel
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string CustomerName { get; set; }

        public DateTime Created { get; set; }

        public DateTime Changed { get; set; }

        public TicketStatus Status { get; set; }
    }
    public enum TicketStatus
    {
        [Display(Name = "Not Started")]
        Not_Started,
        Ongoing,
        Closed
    }

}
