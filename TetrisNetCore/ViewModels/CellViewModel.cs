using Reactive.Bindings;
using System.Windows.Media;

namespace TetrisNetCore.ViewModels
{
    /// <summary>
    /// Zapewnia model do rysowania komórek.
    /// </summary>
    public class CellViewModel
    {
        #region Właściwości
        /// <summary>
        /// Kolor
        /// </summary>
        public ReactiveProperty<Color> Color { get; } = new ReactiveProperty<Color>();
        #endregion
    }
}
