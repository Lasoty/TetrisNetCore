using Reactive.Bindings;
using TetrisNetCore.Models;

namespace TetrisNetCore.ViewModels
{
    /// <summary>
    /// Reprezentuje wynik gry.
    /// </summary>
    public class GameResultViewModel
    {
        #region Właściwości
        /// <summary>
        /// Wynik gry
        /// </summary>
        private GameResult Result { get; }


        /// <summary>
        /// Pobiera całkowitą liczbę usuniętych wierszy.
        /// </summary>
        public IReadOnlyReactiveProperty<int> TotalRowCount => this.Result.TotalRowCount;


        /// <summary>
        ///Pobiera liczbę wymazań w jednym wierszu.
        /// </summary>
        public IReadOnlyReactiveProperty<int> RowCount1 => this.Result.RowCount1;


        /// <summary>
        /// Pobiera liczbę wymazań w dwóch wierszach.
        /// </summary>
        public IReadOnlyReactiveProperty<int> RowCount2 => this.Result.RowCount2;


        /// <summary>
        /// Pobiera liczbę wymazań w 3 wierszach.
        /// </summary>
        public IReadOnlyReactiveProperty<int> RowCount3 => this.Result.RowCount3;


        /// <summary>
        /// Pobiera liczbę wymazań w 4 wierszach.
        /// </summary>
        public IReadOnlyReactiveProperty<int> RowCount4 => this.Result.RowCount4;
        #endregion


        #region Konstruktor
        /// <summary>
        /// Tworzy instancję wyników gry
        /// </summary>
        /// <param name="result">Wyniki gry</param>
        public GameResultViewModel(GameResult result)
        {
            this.Result = result;
        }
        #endregion
    }
}