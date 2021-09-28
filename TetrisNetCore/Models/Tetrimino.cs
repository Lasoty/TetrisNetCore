using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using TetrisNetCore.Utilities;

namespace TetrisNetCore.Models
{
    /// <summary>
    /// Pełni funkcję tetrimino.
    /// </summary>
    public class Tetrimino
    {
        #region Właściwości
        /// <summary>
        /// Rodzaj figury
        /// </summary>
        public TetriminoKind Kind { get; }


        /// <summary>
        /// Kolor figury
        /// </summary>
        public Color Color => this.Kind.BlockColor();


        /// <summary>
        /// Uzyskaj punkt odniesienia (= współrzędna górna lewa).
        /// </summary>
        public Position Position { get; private set; }


        /// <summary>
        /// Uzyskaj orientację.
        /// </summary>
        public Direction Direction { get; private set; }


        /// <summary>
        /// Bloki składowe
        /// </summary>
        public IReadOnlyList<Block> Blocks { get; private set; }
        #endregion

        #region Konstruktor
        /// <summary>
        /// Utwórz instancję
        /// </summary>
        /// <param name="kind">Rodzaje tetrimino</param>
        private Tetrimino(TetriminoKind kind)
        {
            this.Kind = kind;
            this.Position = kind.InitialPosition();
            this.Blocks = kind.CreateBlock(this.Position);
        }
        #endregion

        #region Metody
        /// <summary>
        /// Losowo uzyskaj rodzaj tetrimino。
        /// </summary>
        /// <returns>Rodzaje tetrimino</returns>
        public static TetriminoKind RandomKind()
        {
            int length = Enum.GetValues(typeof(TetriminoKind)).Length;
            return (TetriminoKind)RandomProvider.ThreadRandom.Next(length);
        }


        /// <summary>
        /// Wygeneruj tetrimino。
        /// </summary>
        /// <returns>instancja</returns>
        public static Tetrimino Create(TetriminoKind? kind = null)
        {
            kind = kind ?? RandomKind();
            return new Tetrimino(kind.Value);
        }
        #endregion

        #region Operowanie tetrimino
        /// <summary>
        /// Poruszanie się w określonym kierunku.
        /// </summary>
        /// <param name="direction">Kierunek ruchu</param>
        /// <param name="checkCollision">Deleguj, czy określone bloki są w konflikcie</param>
        /// <returns>Czy to się poruszyło</returns>
        public bool Move(MoveDirection direction, Func<Block, bool> checkCollision)
        {
            //--- Określ pozycję po przeniesieniu
            Position position = this.Position;
            if (direction == MoveDirection.Down)
            {
                int row = position.Row + 1;
                position = new Position(row, position.Column);
            }
            else
            {
                int delta = (direction == MoveDirection.Right) ? 1 : -1;
                int column = position.Column + delta;
                position = new Position(position.Row, column);
            }

            //--- Wygeneruj blok zgodnie z pozycją
            IReadOnlyList<Block> blocks = this.Kind.CreateBlock(position, this.Direction);

            //--- Czy bloki się zderzają?
            if (blocks.Any(checkCollision))
                return false;

            //--- Zapisz stan ruchu, jeśli nie ma kolizji
            this.Position = position;
            this.Blocks = blocks;
            return true;
        }


        /// <summary>
        /// Obracóć się w określonym kierunk.
        /// </summary>
        /// <param name="rotationDirection">Kierunek rotacji</param>
        /// <param name="checkCollision">Określ, czy wskazane bloki są w konflikcie</param>
        /// <returns>Czy się obrócił</returns>
        public bool Rotation(RotationDirection rotationDirection, Func<Block, bool> checkCollision)
        {
            //--- Określ orientację po obrocie
            int count = Enum.GetValues(typeof(Direction)).Length;
            int delta = (rotationDirection == RotationDirection.Right) ? 1 : -1;
            int direction = (int)this.Direction + delta;
            if (direction < 0) direction += count;
            if (direction >= count) direction %= count;

            //--- Korekcja rotacji bocznej (Super Rotation)
            int[] adjustPattern = this.Kind == TetriminoKind.I
                                ? new[] { 0, 1, -1, 2, -2 }  //--- Korekta do 2 komórek, jeśli kształt to I
                                : new[] { 0, 1, -1 };  //--- W przeciwnym razie korekcja 1 komórki
            foreach (int adjust in adjustPattern)
            {
                //--- Generuj zorientowane bloki
                Position position = new Position(this.Position.Row, this.Position.Column + adjust);
                IReadOnlyList<Block> blocks = this.Kind.CreateBlock(position, (Direction)direction);

                //--- Zapisuje obrót, jeśli nie koliduje
                if (!blocks.Any(checkCollision))
                {
                    this.Direction = (Direction)direction;
                    this.Position = position;
                    this.Blocks = blocks;
                    return true;
                }
            }

            //--- Nie da się obrócić
            return false;
        }
        #endregion
    }
}