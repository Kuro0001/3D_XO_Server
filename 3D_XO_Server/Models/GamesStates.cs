using System.ComponentModel.DataAnnotations;

namespace _3D_XO_Server.Models
{
    public class GamesStates
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public int UserId { get; set; }
        public bool IsReaded { get; set; }
        public string GameState { get; set; }
    }
}
