namespace TetrisNetCore.Models
{
    /// <summary>
    /// Reprezentuje kierunek。
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// W górę
        /// </summary>
        Up = 0,

        /// <summary>
        /// W prawo
        /// </summary>
        Right,

        /// <summary>
        /// W dół
        /// </summary>
        Down,

        /// <summary>
        /// W lewo
        /// </summary>
        Left,
    }

    /// <summary>
    /// Wskazuje kierunek obrotu.
    /// </summary>
    public enum RotationDirection
    {
        /// <summary>
        /// Obrót w prawo
        /// </summary>
        Right = 0,

        /// <summary>
        /// Obrót w lewo
        /// </summary>
        Left,
    }

    /// <summary>
    /// Wskazuje kierunek ruchu。
    /// </summary>
    public enum MoveDirection
    {
        /// <summary>
        /// W prawo
        /// </summary>
        Right = 0,

        /// <summary>
        /// W dół 
        /// </summary>
        Down,

        /// <summary>
        /// W lewo
        /// </summary>
        Left,
    }
}