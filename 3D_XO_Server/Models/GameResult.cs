using System.Collections.Generic;

namespace _3D_XO_Server.Models
{
    public class GameResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Game> Games { get; set; }
        public GameResult() 
        {
            Games = new List<Game>();
        }
    }
}
