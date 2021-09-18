using Reactive.Bindings;
using System.Windows.Media;
using TetrisNetCore.Extensions;

namespace TetrisNetCore.ViewModels
{
    public class NextFieldViewModel
    {
        #region Stałe

        private const byte RowCount = 5;
        
        private const byte ColumnCount = 5;

        #endregion


        #region Właściwości

        public CellViewModel[,] Cells { get; set; }

        public Color BackgroundColor => Colors.WhiteSmoke;

        #endregion

        #region Konstruktor

        public NextFieldViewModel(IReadOnlyReactiveProperty<object> nextTetrimino)
        {
            this.Cells = new CellViewModel[RowCount, ColumnCount];

            foreach (var item in this.Cells.WithIndex())
            {
                this.Cells[item.X, item.Y] = new CellViewModel();
            }

            //TODO: Obsługa zmian związanych z blokami
        }

        #endregion
    }
}