using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using TetrisNetCore.Extensions;
using TetrisNetCore.Models;
using TetrisNetCore.ViewModels;

namespace TetrisNetCore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml - Logika interakcji
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Właściwości
        /// <summary>
        /// Pobierz lub skonfiguruj grę.
        /// </summary>
        private GameViewModel Game
        {
            get { return this.DataContext as GameViewModel; }
            set { this.DataContext = value; }
        }
        #endregion


        #region konstruktor
        /// <summary>
        /// Utwórz instancję.
        /// </summary>
        public MainWindow()
        {
            this.Game = new GameViewModel();
            this.InitializeComponent();
            SetupField(this.field, this.Game.Field.Cells, 30);
            SetupField(this.nextField, this.Game.NextField.Cells, 18);
            this.AttachEvents();
            this.Game.Play();
        }
        #endregion


        #region Inicjacja
        /// <summary>
        /// Przygotuj pole.
        /// </summary>
        /// <param name="field">Pole dla Grid</param>
        /// <param name="cells">Model do rysowania komórek</param>
        /// <param name="blockSize">Rozmiar bloku</param>
        private static void SetupField(Grid field, CellViewModel[,] cells, byte blockSize)
        {
            //--- Definicja wiersza/kolumny
            for (int r = 0; r < cells.GetLength(0); r++)
                field.RowDefinitions.Add(new RowDefinition { Height = new GridLength(blockSize, GridUnitType.Pixel) });

            for (int c = 0; c < cells.GetLength(1); c++)
                field.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(blockSize, GridUnitType.Pixel) });

            //--- Ustawienia komórki
            foreach (IndexedItem2<CellViewModel> item in cells.WithIndex())
            {
                //--- Powiąż kolor tła
                SolidColorBrush brush = new SolidColorBrush();
                TextBlock control = new TextBlock
                {
                    DataContext = item.Element,
                    Background = brush,
                    Margin = new Thickness(1),
                };
                BindingOperations.SetBinding(brush, SolidColorBrush.ColorProperty, new Binding("Color.Value"));

                //--- Ustaw i dodaj jako element podrzędny
                Grid.SetRow(control, item.X);
                Grid.SetColumn(control, item.Y);
                field.Children.Add(control);
            }
        }


        /// <summary>
        /// Powiązane wydarzenia.
        /// </summary>
        private void AttachEvents()
        {
            this.KeyDown += (s, e) =>
            {
                switch (e.Key)
                {
                    case Key.Z: this.Game.Field.RotationTetrimino(RotationDirection.Left); break;
                    case Key.X: this.Game.Field.RotationTetrimino(RotationDirection.Right); break;
                    case Key.Up: this.Game.Field.RotationTetrimino(RotationDirection.Right); break;
                    case Key.Right: this.Game.Field.MoveTetrimino(MoveDirection.Right); break;
                    case Key.Down: this.Game.Field.MoveTetrimino(MoveDirection.Down); break;
                    case Key.Left: this.Game.Field.MoveTetrimino(MoveDirection.Left); break;
                    case Key.Escape: this.Game.Play(); break;
                    case Key.Space: this.Game.Field.ForceFixTetrimino(); break;
                }
            };
        }
        #endregion

    }
}
