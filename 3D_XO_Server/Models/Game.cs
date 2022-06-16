using System;
using System.Collections.Generic;

namespace _3D_XO_Server.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string User1Id { get; set; }
        public string User2Id { get; set; }
        public GameResult Result { get; set; }
        public DateTime Date { get; set; }
        public List<GamesState> States { get; set; }

        public Game()
        {
            States = new List<GamesState>();
        }
    }
}
