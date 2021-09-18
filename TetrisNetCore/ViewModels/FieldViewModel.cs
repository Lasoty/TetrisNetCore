using System.Windows.Media;
using TetrisNetCore.Extensions;
using TetrisNetCore.Models;

namespace TetrisNetCore.ViewModels
{
    public class FieldViewModel
    {
        #region Właściwości

        public CellViewModel[,] Cells { get; set; }

        private Color BackgroundColor => Colors.WhiteSmoke;

        public Field Field { get; }

        #endregion

        #region Konstruktor

        public FieldViewModel(Field field)
        {
            Field = field;

            this.Cells = new CellViewModel[Field.RowCount, Field.ColumnCount];

            foreach (var item in this.Cells.WithIndex())
            {
                this.Cells[item.X, item.Y] = new CellViewModel();
            }

            //TODO: Obsługa zmian związanych z blokami
        }

        #endregion

        #region Metody

        //TODO: Obsługa ruchu

        //TODO: Obsługa rotacji



        #endregion

    }
}