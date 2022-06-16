using System;
using System.ComponentModel.DataAnnotations;

namespace _3D_XO_Server.Models
{
    public class GamesState
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string UserId { get; set; }
        public bool IsReaded { get; set; }
        public string State { get; set; }
        public DateTime Time { get; set; }
    }
}
