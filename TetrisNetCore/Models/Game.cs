using Reactive.Bindings;
using System;

namespace TetrisNetCore.Models
{
    /// <summary>
    /// Reprezentuje samą grę.
    /// </summary>
    public class Game
    {
        #region Właściwości
        /// <summary>
        /// Wynik gry
        /// </summary>
        public GameResult Result { get; } = new GameResult();

        /// <summary>
        /// Pole
        /// </summary>
        public Field Field { get; } = new Field();

        /// <summary>
        /// Czy gra w toku
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsPlaying => this.Field.IsActivated.ToReadOnlyReactiveProperty();

        /// <summary>
        /// Pobiera informację, czy gra się skończyła.
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsOver => this.Field.IsUpperLimitOvered.ToReadOnlyReactiveProperty();

        /// <summary>
        /// Pobiera tetrimino, które pojawi się jako następne.
        /// </summary>
        public IReadOnlyReactiveProperty<TetriminoKind> NextTetrimino => this.nextTetrimino;
        private readonly ReactiveProperty<TetriminoKind> nextTetrimino = new ReactiveProperty<TetriminoKind>();

        /// <summary>
        /// Pobiera lub ustawia liczbę poprzednich przyspieszeń.
        /// </summary>
        private int PreviousCount { get; set; }
        #endregion

        #region Konstruktor
        /// <summary>
        /// Tworzy instancję gry
        /// </summary>
        public Game()
        {
            this.Field.PlacedBlocks.Subscribe(_ =>
            {
                //--- Przyspiesz za każdym razem, gdy kasujesz 10 linii
                int count = this.Result.TotalRowCount.Value / 10;
                if (count > this.PreviousCount)
                {
                    this.PreviousCount = count;
                    this.Field.SpeedUp();
                }

                //--- Skonfiguruj nowe tetrimino
                TetriminoKind kind = this.nextTetrimino.Value;
                this.nextTetrimino.Value = Tetrimino.RandomKind();
                this.Field.Tetrimino.Value = Tetrimino.Create(kind);
            });
            this.Field.LastRemovedRowCount.Subscribe(this.Result.AddRowCount);
        }
        #endregion

        #region Metody
        /// <summary>
        /// Rozpocznij grę
        /// </summary>
        public void Play()
        {
            if (this.IsPlaying.Value)
                return;

            this.PreviousCount = 0;
            this.nextTetrimino.Value = Tetrimino.RandomKind();
            this.Field.Activate(Tetrimino.RandomKind());
            this.Result.Clear();
        }
        #endregion
    }

}
