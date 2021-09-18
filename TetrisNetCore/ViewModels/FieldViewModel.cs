using System.Windows.Media;
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
        }

        #endregion

    }
}