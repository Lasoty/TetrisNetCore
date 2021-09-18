using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisNetCore.Models
{
    public class Game
    {
        public GameResult Result { get; internal set; } = new GameResult();
        public Field Field { get; internal set; } = new Field();
        public IReadOnlyReactiveProperty<object> NextTetrimino { get; internal set; }

        internal void Play()
        {
            
        }
    }
}
