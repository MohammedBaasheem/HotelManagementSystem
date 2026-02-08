using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Feedback : BaseModel
    {
        public string FeedBackDetails { get; set; }

        public DateTime FeedBackDate { get; set; }

        public bool IsRecommended { get; set; }

        public int Rate { get; set; }
        public int RoomReservationId { get; set; }   // FK
        public RoomReservation RoomReservation { get; set; }


    }
}