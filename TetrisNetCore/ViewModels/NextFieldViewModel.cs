using System;
using System.Reactive.Linq;
using Reactive.Bindings;
using System.Windows.Media;
using TetrisNetCore.Extensions;
using TetrisNetCore.Models;

namespace TetrisNetCore.ViewModels
{
    /// <summary>
    /// Zapewnia model pola do wyświetlenia następnego tetrimino.
    /// </summary>
    public class NextFieldViewModel
    {
        #region Stałe
        /// <summary>
        /// Reprezentuje liczbę wierszy. Ta wartość jest stałą.
        /// </summary>
        private const byte RowCount = 5;


        /// <summary>
        /// Reprezentuje liczbę kolumn. Ta wartość jest stałą.
        /// </summary>
        private const byte ColumnCount = 5;
        #endregion

        #region Właściwości
        /// <summary>
        /// Kolekcja komórek.
        /// </summary>
        public CellViewModel[,] Cells { get; }


        /// <summary>
        /// Kolor tła.
        /// </summary>
        private Color BackgroundColor => Colors.WhiteSmoke;
        #endregion

        #region Konstruktor
        /// <summary>
        /// Tworzy instancję pola następnego tetrimino
        /// </summary>
        /// <param name="nextTetrimino">Następna tetrimino</param>
        public NextFieldViewModel(IReadOnlyReactiveProperty<TetriminoKind> nextTetrimino)
        {
            //--- Przygotuj komórkę do rysowania
            this.Cells = new CellViewModel[RowCount, ColumnCount];
            foreach (IndexedItem2<CellViewModel> item in this.Cells.WithIndex())
                this.Cells[item.X, item.Y] = new CellViewModel();

            //--- Obsługa zmian związanych z blokami
            nextTetrimino
                .Select(x => Tetrimino.Create(x).Blocks.ToDictionary2(y => y.Position.Row, y => y.Position.Column))
                .Subscribe(x =>
                {
                    //--- Regulacja przesunięcia ViewPort
                    //--- Dużo kłopotów z poprawnym pisaniem, więc musiałem wymusić
                    Position offset = new Position((-6 - x.Count) / 2, 2);

                    //--- Zastosować
                    foreach (IndexedItem2<CellViewModel> item in this.Cells.WithIndex())
                    {
                        Color color = x.GetValueOrDefault(item.X + offset.Row)
                                    ?.GetValueOrDefault(item.Y + offset.Column)
                                    ?.Color
                                    ?? this.BackgroundColor;
                        item.Element.Color.Value = color;
                    }
                });
        }
        #endregion
    }
}