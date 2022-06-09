using System;
using System.Collections.Generic;

namespace _3D_XO_Server.Models
{
    public class Game
    {
        public int Id { get; set; }
        public int User1Id { get; set; }
        public int User2ID { get; set; }
        public string Result { get; set; }
        public DateTime Date { get; set; }
        public List<GamesStates> States { get; set; }

        public Game()
        {
            States = new List<GamesStates>();
        }
    }
}
