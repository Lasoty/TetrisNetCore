using System.Windows.Media;

namespace TetrisNetCore.Models
{
    /// <summary>
    /// Zapewnia funkcję jako blok
    /// </summary>
    public class Block
    {
        #region Właściwości
        /// <summary>
        /// Kolor
        /// </summary>
        public Color Color { get; }

        /// <summary>
        /// Pozycja elementu
        /// </summary>
        public Position Position { get; }
        #endregion

        #region Konstruktor
        /// <summary>
        /// Tworzy instancję elementu
        /// </summary>
        /// <param name="color">Kolor bloku</param>
        /// <param name="position">Pozycja bloku</param>
        public Block(Color color, Position position)
        {
            this.Color = color;
            this.Position = position;
        }
        #endregion
    }
}