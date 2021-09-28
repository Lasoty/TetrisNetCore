using Reactive.Bindings;
using TetrisNetCore.Models;

namespace TetrisNetCore.ViewModels
{
    /// <summary>
    /// Zapewnia model do rysowania całej gry.
    /// </summary>
    public class GameViewModel
    {
        #region Właściwości
        /// <summary>
        /// Obiekt gry
        /// </summary>
        private Game Game { get; } = new Game();


        /// <summary>
        /// Wyniki gry
        /// </summary>
        public GameResultViewModel Result { get; }


        /// <summary>
        /// Pobiera model do rysowania pola.
        /// </summary>
        public FieldViewModel Field { get; }


        /// <summary>
        /// Pobiera model do rysowania następujących pól tetrimino.
        /// </summary>
        public NextFieldViewModel NextField { get; }


        /// <summary>
        /// Czy w trakcie gry
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsPlaying => this.Game.IsPlaying;


        /// <summary>
        /// Czy gra zakończona
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsOver => this.Game.IsOver;
        #endregion


        #region Konstruktor
        /// <summary>
        /// Utwórz instancję.
        /// </summary>
        public GameViewModel()
        {
            this.Result = new GameResultViewModel(this.Game.Result);
            this.Field = new FieldViewModel(this.Game.Field);
            this.NextField = new NextFieldViewModel(this.Game.NextTetrimino);
        }
        #endregion


        #region Metody
        /// <summary>
        /// Rozpocznij Grę
        /// </summary>
        public void Play() => this.Game.Play();
        #endregion
    }

}
