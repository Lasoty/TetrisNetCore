using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using TetrisNetCore.Extensions;
using TetrisNetCore.ViewModels;

namespace TetrisNetCore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml - Logika interakcji
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Właściwości

        public GameViewModel Game
        {
            get { return this.DataContext as GameViewModel; }
            set { this.DataContext = value; }
        }

        #endregion

        #region Konstruktor

        /// <summary>
        /// Utwórz instancję.
        /// </summary>
        public MainWindow()
        {
            this.Game = new GameViewModel();
            this.InitializeComponent();

            SetupField(this.field, this.Game.Field.Cells, 30);
            SetupField(this.nextField, this.Game.NextField.Cells, 18);
        }

        #endregion

        #region Inicjalizacja

        private void SetupField(Grid field, CellViewModel[,] cells, byte blockSize)
        {
            for (int r = 0; r < cells.GetLength(0); r++)
            {
                field.RowDefinitions.Add(new RowDefinition()
                {
                    Height = new GridLength(blockSize, GridUnitType.Pixel)
                });
            }

            for (int c = 0; c < cells.GetLength(1); c++)
            {
                field.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(blockSize, GridUnitType.Pixel)
                });
            }

            foreach (IndexedItem2<CellViewModel> item in cells.WithIndex())
            {
                SolidColorBrush brush = new SolidColorBrush();
                TextBlock control = new TextBlock
                {
                    DataContext = item.Element,
                    Background = brush,
                    Margin = new Thickness(1)
                };

                BindingOperations.SetBinding(brush, SolidColorBrush.ColorProperty, new Binding("Color.Value"));

                Grid.SetRow(control, item.X);
                Grid.SetColumn(control, item.Y);
                field.Children.Add(control);
            }
        }

        #endregion
    }
}
