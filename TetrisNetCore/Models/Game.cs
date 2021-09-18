using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisNetCore.Models
{
    public class Game
    {
        public GameResult Result { get; internal set; }
        public Field Field { get; internal set; }

        internal void Play()
        {
            
        }
    }
}
