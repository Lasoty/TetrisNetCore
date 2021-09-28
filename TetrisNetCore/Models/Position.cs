namespace TetrisNetCore.Models
{
    /// <summary>
    /// Reprezentuje współrzędne.
    /// </summary>
    public struct Position
    {
        #region Właściwości
        /// <summary>
        /// Wiersz
        /// </summary>
        public int Row { get; }


        /// <summary>
        /// Kolumna
        /// </summary>
        public int Column { get; }
        #endregion


        #region Konstruktor
        /// <summary>
        /// Wypełnia wartości elementu współrzędnych
        /// </summary>
        /// <param name="row">Wiersz</param>
        /// <param name="column">Kolumna</param>
        public Position(int row, int column)
        {
            this.Row = row;
            this.Column = column;
        }
        #endregion


        #region Nadpisanie metod porównania
        /// <summary>
        ///Określa, czy jest równoważny określonej instancji
        /// </summary>
        /// <param name="obj">Instancja do porównania</param>
        /// <returns>true jeśli prawda</returns>
        public override bool Equals(object obj) 
            => this == (Position)obj;


        /// <summary>
        /// Uzyskaj wartość skrótu
        /// </summary>
        /// <returns>Wartość skrótu</returns>
        public override int GetHashCode()
            => this.Row.GetHashCode() ^ this.Column.GetHashCode();


        /// <summary>
        /// Wartość tekstowa
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
            => $"({this.Row}, {this.Column})";
        #endregion


        #region Przeciążenie operatora
        /// <summary>
        /// Zdefiniuj operator znaku równości
        /// </summary>
        /// <param name="left">Lewa strona</param>
        /// <param name="right">Prawa strona</param>
        /// <returns>true jeśli wartości są równe</returns>
        public static bool operator ==(Position left, Position right)
            => left.Row == right.Row
            && left.Column == right.Column;


        /// <summary>
        /// Przeciążenie operatora !=
        /// </summary>
        /// <param name="left">Lewa strona</param>
        /// <param name="right">Prawa strona</param>
        /// <returns>Jeśli wartości są różne wtedy true</returns>
        public static bool operator !=(Position left, Position right)
            => !(left == right);
        #endregion
    }

}