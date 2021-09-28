using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace TetrisNetCore.Models
{
    /// <summary>
    /// Reprezentuje rodzaj tetrimino
    /// </summary>
    public enum TetriminoKind
    {
        /// <summary>
        /// <para>□■□□</para>
        /// <para>□■□□</para>
        /// <para>□■□□</para>
        /// <para>□■□□</para>
        /// </summary>
        I = 0,

        /// <summary>
        /// <para>□□□□</para>
        /// <para>□■■□</para>
        /// <para>□■■□</para>
        /// <para>□□□□</para>
        /// </summary>
        O,

        /// <summary>
        /// <para>□□□□</para>
        /// <para>□■■□</para>
        /// <para>■■□□</para>
        /// <para>□□□□</para>
        /// </summary>
        S,

        /// <summary>
        /// <para>□□□□</para>
        /// <para>□■■□</para>
        /// <para>□□■■</para>
        /// <para>□□□□</para>
        /// </summary>
        Z,

        /// <summary>
        /// <para>□□□□</para>
        /// <para>□■□□</para>
        /// <para>□■■■</para>
        /// <para>□□□□</para>
        /// </summary>
        J,

        /// <summary>
        /// <para>□□□□</para>
        /// <para>□□■□</para>
        /// <para>■■■□</para>
        /// <para>□□□□</para>
        /// </summary>
        L,

        /// <summary>
        /// <para>□□□□</para>
        /// <para>□■□□</para>
        /// <para>■■■□</para>
        /// <para>□□□□</para>
        /// </summary>
        T,
    }

    /// <summary>
    /// Zapewnia rozszerzenia typu tetromino。
    /// </summary>
    public static class TetriminoExtensions
    {
        /// <summary>
        /// Uzyskaj kolor bloku。
        /// </summary>
        /// <param name="self">Rodzaje tetrimino</param>
        /// <returns>kolor</returns>
        public static Color BlockColor(this TetriminoKind self)
        {
            switch (self)
            {
                case TetriminoKind.I: return Colors.LightBlue;
                case TetriminoKind.O: return Colors.Yellow;
                case TetriminoKind.S: return Colors.YellowGreen;
                case TetriminoKind.Z: return Colors.Red;
                case TetriminoKind.J: return Colors.Blue;
                case TetriminoKind.L: return Colors.Orange;
                case TetriminoKind.T: return Colors.Purple;
            }
            throw new InvalidOperationException("Unknown Tetrimino");
        }


        /// <summary>
        /// Uzyskaj początkową pozycję bloku
        /// </summary>
        /// <param name="self">Rodzaje tetrimino</param>
        /// <returns>Pozycja początkowa</returns>
        public static Position InitialPosition(this TetriminoKind self)
        {
            int length = 0;
            switch (self)
            {
                case TetriminoKind.I: length = 4; break;
                case TetriminoKind.O: length = 2; break;
                case TetriminoKind.S:
                case TetriminoKind.Z:
                case TetriminoKind.J:
                case TetriminoKind.L:
                case TetriminoKind.T: length = 3; break;
                default: throw new InvalidOperationException("Unknown Tetrimino");
            }

            int row = -length;
            int column = (Field.ColumnCount - length) / 2;
            return new Position(row, column);
        }


        /// <summary>
        /// Generuje bloki pasujące do określonego typu i orientacji tetrimino.
        /// </summary>
        /// <param name="self">Rodzaj tetrimino</param>
        /// <param name="offset">Ruch do współrzędnych absolutnych</param>
        /// <param name="direction">kierunek</param>
        /// <returns>Kolekcja bloków</returns>
        public static IReadOnlyList<Block> CreateBlock(this TetriminoKind self, Position offset, Direction direction = Direction.Up)
        {
            //--- Reprezentowanie kształtu bloku za pomocą bitów
            //--- Jest stały, ale tak jest najłatwiej i najszybciej
            int[,] pattern = null;
            switch (self)
            {
                #region I
                case TetriminoKind.I:
                    switch (direction)
                    {
                        case Direction.Up:
                            pattern = new int[,]
                            {
                                { 0, 1, 0, 0 },
                                { 0, 1, 0, 0 },
                                { 0, 1, 0, 0 },
                                { 0, 1, 0, 0 },
                            };
                            break;

                        case Direction.Right:
                            pattern = new int[,]
                            {
                                { 0, 0, 0, 0 },
                                { 1, 1, 1, 1 },
                                { 0, 0, 0, 0 },
                                { 0, 0, 0, 0 },
                            };
                            break;

                        case Direction.Down:
                            pattern = new int[,]
                            {
                                { 0, 0, 1, 0 },
                                { 0, 0, 1, 0 },
                                { 0, 0, 1, 0 },
                                { 0, 0, 1, 0 },
                            };
                            break;

                        case Direction.Left:
                            pattern = new int[,]
                            {
                                { 0, 0, 0, 0 },
                                { 0, 0, 0, 0 },
                                { 1, 1, 1, 1 },
                                { 0, 0, 0, 0 },
                            };
                            break;
                    }
                    break;
                #endregion

                #region O
                case TetriminoKind.O:
                    pattern = new int[,]
                    {
                        { 1, 1 },
                        { 1, 1 },
                    };
                    break;
                #endregion

                #region S
                case TetriminoKind.S:
                    switch (direction)
                    {
                        case Direction.Up:
                            pattern = new int[,]
                            {
                                { 0, 1, 1 },
                                { 1, 1, 0 },
                                { 0, 0, 0 },
                            };
                            break;

                        case Direction.Right:
                            pattern = new int[,]
                            {
                                { 0, 1, 0 },
                                { 0, 1, 1 },
                                { 0, 0, 1 },
                            };
                            break;

                        case Direction.Down:
                            pattern = new int[,]
                            {
                                { 0, 0, 0 },
                                { 0, 1, 1 },
                                { 1, 1, 0 },
                            };
                            break;

                        case Direction.Left:
                            pattern = new int[,]
                            {
                                { 1, 0, 0 },
                                { 1, 1, 0 },
                                { 0, 1, 0 },
                            };
                            break;
                    }
                    break;
                #endregion

                #region Z
                case TetriminoKind.Z:
                    switch (direction)
                    {
                        case Direction.Up:
                            pattern = new int[,]
                            {
                                { 1, 1, 0 },
                                { 0, 1, 1 },
                                { 0, 0, 0 },
                            };
                            break;

                        case Direction.Right:
                            pattern = new int[,]
                            {
                                { 0, 0, 1 },
                                { 0, 1, 1 },
                                { 0, 1, 0 },
                            };
                            break;

                        case Direction.Down:
                            pattern = new int[,]
                            {
                                { 0, 0, 0 },
                                { 1, 1, 0 },
                                { 0, 1, 1 },
                            };
                            break;

                        case Direction.Left:
                            pattern = new int[,]
                            {
                                { 0, 1, 0 },
                                { 1, 1, 0 },
                                { 1, 0, 0 },
                            };
                            break;
                    }
                    break;
                #endregion

                #region J
                case TetriminoKind.J:
                    switch (direction)
                    {
                        case Direction.Up:
                            pattern = new int[,]
                            {
                                { 1, 0, 0 },
                                { 1, 1, 1 },
                                { 0, 0, 0 },
                            };
                            break;

                        case Direction.Right:
                            pattern = new int[,]
                            {
                                { 0, 1, 1 },
                                { 0, 1, 0 },
                                { 0, 1, 0 },
                            };
                            break;

                        case Direction.Down:
                            pattern = new int[,]
                            {
                                { 0, 0, 0 },
                                { 1, 1, 1 },
                                { 0, 0, 1 },
                            };
                            break;

                        case Direction.Left:
                            pattern = new int[,]
                            {
                                { 0, 1, 0 },
                                { 0, 1, 0 },
                                { 1, 1, 0 },
                            };
                            break;
                    }
                    break;
                #endregion

                #region L
                case TetriminoKind.L:
                    switch (direction)
                    {
                        case Direction.Up:
                            pattern = new int[,]
                            {
                                { 0, 0, 1 },
                                { 1, 1, 1 },
                                { 0, 0, 0 },
                            };
                            break;

                        case Direction.Right:
                            pattern = new int[,]
                            {
                                { 0, 1, 0 },
                                { 0, 1, 0 },
                                { 0, 1, 1 },
                            };
                            break;

                        case Direction.Down:
                            pattern = new int[,]
                            {
                                { 0, 0, 0 },
                                { 1, 1, 1 },
                                { 1, 0, 0 },
                            };
                            break;

                        case Direction.Left:
                            pattern = new int[,]
                            {
                                { 1, 1, 0 },
                                { 0, 1, 0 },
                                { 0, 1, 0 },
                            };
                            break;
                    }
                    break;
                #endregion

                #region T
                case TetriminoKind.T:
                    switch (direction)
                    {
                        case Direction.Up:
                            pattern = new int[,]
                            {
                                { 0, 1, 0 },
                                { 1, 1, 1 },
                                { 0, 0, 0 },
                            };
                            break;

                        case Direction.Right:
                            pattern = new int[,]
                            {
                                { 0, 1, 0 },
                                { 0, 1, 1 },
                                { 0, 1, 0 },
                            };
                            break;

                        case Direction.Down:
                            pattern = new int[,]
                            {
                                { 0, 0, 0 },
                                { 1, 1, 1 },
                                { 0, 1, 0 },
                            };
                            break;

                        case Direction.Left:
                            pattern = new int[,]
                            {
                                { 0, 1, 0 },
                                { 1, 1, 0 },
                                { 0, 1, 0 },
                            };
                            break;
                    }
                    break;
                    #endregion
            }

            //--- Nie dotyczyło żadnego
            if (pattern == null)
                throw new InvalidOperationException("Unknown Tetrimino");

            //--- Utwórz blok, w którym stoi bit
            Color color = self.BlockColor();
            return Enumerable.Range(0, pattern.GetLength(0))
                    .SelectMany(r => Enumerable.Range(0, pattern.GetLength(1)).Select(c => new Position(r, c)))
                    .Where(x => pattern[x.Row, x.Column] != 0)  //--- Gdzie stoi bit
                    .Select(x => new Position(x.Row + offset.Row, x.Column + offset.Column))  //--- Transformacja współrzędnych bezwzględnych
                    .Select(x => new Block(color, x))
                    .ToArray();
        }
    }

}