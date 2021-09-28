using Reactive.Bindings;
using System;
using System.Reactive.Linq;

namespace TetrisNetCore.Models
{
    /// <summary>
    /// Reprezentuję wynik gry
    /// </summary>
    public class GameResult
    {
        #region Właściwości
        /// <summary>
        /// Całkowita liczba usuniętych linii
        /// </summary>
        public IReadOnlyReactiveProperty<int> TotalRowCount { get; }


        /// <summary>
        /// Pobiera liczbę wymazań w jednego wiersza.
        /// </summary>
        public IReadOnlyReactiveProperty<int> RowCount1 => this.rowCount1;
        private readonly ReactiveProperty<int> rowCount1 = new ReactiveProperty<int>();


        /// <summary>
        /// Pobiera liczbę wymazań w dwóch wierszy.
        /// </summary>
        public IReadOnlyReactiveProperty<int> RowCount2 => this.rowCount2;
        private readonly ReactiveProperty<int> rowCount2 = new ReactiveProperty<int>();


        /// <summary>
        /// Pobiera liczbę wymazań w 3 wierszach.
        /// </summary>
        public IReadOnlyReactiveProperty<int> RowCount3 => this.rowCount3;
        private readonly ReactiveProperty<int> rowCount3 = new ReactiveProperty<int>();


        /// <summary>
        /// Pobiera liczbę wymazań w 4 wierszach.
        /// </summary>
        public IReadOnlyReactiveProperty<int> RowCount4 => this.rowCount4;
        private readonly ReactiveProperty<int> rowCount4 = new ReactiveProperty<int>();
        #endregion


        #region Konstruktor
        /// <summary>
        /// Tworzy instancję wyniku gry
        /// </summary>
        public GameResult()
        {
            this.TotalRowCount
                = this.RowCount1.CombineLatest
                (
                    this.RowCount2,
                    this.RowCount3,
                    this.RowCount4,
                    (x1, x2, x3, x4) => x1 * 1
                                    + x2 * 2
                                    + x3 * 3
                                    + x4 * 4
                )
                .ToReadOnlyReactiveProperty();
        }
        #endregion


        #region Metody
        /// <summary>
        /// Dodaj liczbę usuniętych wierszy.
        /// </summary>
        /// <param name="count">Liczba linii</param>
        public void AddRowCount(int count)
        {
            switch (count)
            {
                case 1: this.rowCount1.Value++; break;
                case 2: this.rowCount2.Value++; break;
                case 3: this.rowCount3.Value++; break;
                case 4: this.rowCount4.Value++; break;
                default: throw new ArgumentOutOfRangeException(nameof(count));
            }
        }


        /// <summary>
        /// Wyczyść wynik.
        /// </summary>
        public void Clear()
        {
            this.rowCount1.Value = 0;
            this.rowCount2.Value = 0;
            this.rowCount3.Value = 0;
            this.rowCount4.Value = 0;
        }
        #endregion
    }
}