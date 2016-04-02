using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Holdem.Models
{
    public class AddPlayerViewModel
    {
        public Guid TableId { get; set; }
        public Guid RoundId { get; set; }
        public Guid PlayerId { get; set; }
    }

    public class SubmitCardScreenViewModel : AddPlayerViewModel
    {
        public string Command { get; set; }
        public decimal RaiseAmount { get; set; } = (decimal) 0.00;
    }
}