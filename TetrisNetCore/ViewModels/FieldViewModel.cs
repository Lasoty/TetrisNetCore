using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Media;
using Reactive.Bindings;
using TetrisNetCore.Extensions;
using TetrisNetCore.Models;

namespace TetrisNetCore.ViewModels
{
    /// <summary>
    /// Zapewnia model do rysowania w obszarze gry。
    /// </summary>
    public class FieldViewModel
    {
        #region Właściwości
        /// <summary>
        /// Miejsce do gry
        /// </summary>
        private Field Field { get; }


        /// <summary>
        /// Pobiera kolekcję komórek.
        /// </summary>
        public CellViewModel[,] Cells { get; }


        /// <summary>
        /// Czy jest aktywny
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsActivated => this.Field.IsActivated;


        /// <summary>
        /// Kolor tła
        /// </summary>
        private Color BackgroundColor => Colors.WhiteSmoke;
        #endregion

        #region Konstruktor
        /// <summary>
        /// Tworzy instancję pola gry
        /// </summary>
        public FieldViewModel(Field field)
        {
            this.Field = field;

            //--- Przygotuj komórkę do rysowania
            this.Cells = new CellViewModel[Field.RowCount, Field.ColumnCount];
            foreach (IndexedItem2<CellViewModel> item in this.Cells.WithIndex())
                this.Cells[item.X, item.Y] = new CellViewModel();

            //--- Obsługa zmian związanych z blokami
            this.Field.Tetrimino
                .CombineLatest
                (
                    this.Field.PlacedBlocks,
                    (t, p) => (t == null ? p : p.Concat(t.Blocks))
                            .ToDictionary2(x => x.Position.Row, x => x.Position.Column)
                )
                .Subscribe(x =>
                {
                    foreach (IndexedItem2<CellViewModel> item in this.Cells.WithIndex())
                    {
                        Color color = x.GetValueOrDefault(item.X)
                                    ?.GetValueOrDefault(item.Y)
                                    ?.Color
                                    ?? this.BackgroundColor;
                        item.Element.Color.Value = color;
                    }
                });
        }
        #endregion

        #region Metody
        /// <summary>
        /// Przesuwa tetrimino w określonym kierunku.
        /// </summary>
        /// <param name="direction">Kierunek ruchu</param>
        public void MoveTetrimino(MoveDirection direction) => this.Field.MoveTetrimino(direction);


        /// <summary>
        /// Obraca tetrimino w określonym kierunku
        /// </summary>
        /// <param name="direction">Kierunek rotacji</param>
        public void RotationTetrimino(RotationDirection direction) => this.Field.RotationTetrimino(direction);


        /// <summary>
        /// Zmusza tetrimino do zafiksowania.
        /// </summary>
        public void ForceFixTetrimino() => this.Field.ForceFixTetrimino();
        #endregion
    }
}