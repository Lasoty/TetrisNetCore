using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Timers;
using TetrisNetCore.Extensions;

namespace TetrisNetCore.Models
{
    /// <summary>
    /// Pełni funkcję miejsca dla Tetrisa.
    /// </summary>
    public class Field
    {
        #region Stałe
        /// <summary>
        /// Reprezentuje liczbę wierszy. Ta wartość jest stałą.
        /// </summary>
        public const byte RowCount = 24;


        /// <summary>
        /// Reprezentuje liczbę kolumn. Ta wartość jest stałą.
        /// </summary>
        public const byte ColumnCount = 10;
        #endregion

        #region Właściwości
        /// <summary>
        /// Pobiera kolekcję umieszczonych bloków.
        /// </summary>
        public IReadOnlyReactiveProperty<IReadOnlyList<Block>> PlacedBlocks => this.placedBlocks;
        private readonly ReactiveProperty<IReadOnlyList<Block>> placedBlocks = new ReactiveProperty<IReadOnlyList<Block>>(Array.Empty<Block>(), ReactivePropertyMode.RaiseLatestValueOnSubscribe);

        /// <summary>
        /// Pobiera lub ustawia aktualnie działające tetrimino.
        /// </summary>
        public ReactiveProperty<Tetrimino> Tetrimino { get; } = new ReactiveProperty<Tetrimino>();

        /// <summary>
        /// Pobiera, czy jest w stanie aktywnym.
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsActivated => this.isActivated;
        private readonly ReactiveProperty<bool> isActivated = new ReactiveProperty<bool>(mode: ReactivePropertyMode.DistinctUntilChanged);

        /// <summary>
        /// Pobiera, czy przekroczono górną linię limitu.
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsUpperLimitOvered => this.isUpperLimitOvered;
        private readonly ReactiveProperty<bool> isUpperLimitOvered = new ReactiveProperty<bool>(mode: ReactivePropertyMode.DistinctUntilChanged);

        /// <summary>
        /// Pobiera liczbę usuniętych wierszy.
        /// </summary>
        public IReadOnlyReactiveProperty<int> LastRemovedRowCount => this.lastRemovedRowCount;
        private readonly ReactiveProperty<int> lastRemovedRowCount = new ReactiveProperty<int>(mode: ReactivePropertyMode.None);

        /// <summary>
        /// Pobierz zegar.
        /// </summary>
        private Timer Timer { get; } = new Timer();
        #endregion

        #region Konstruktor
        /// <summary>
        /// Tworzy instancję pola gry
        /// </summary>
        public Field()
        {
            this.Timer.ElapsedAsObservable()
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(x => this.MoveTetrimino(MoveDirection.Down));
        }
        #endregion

        #region Metody
        /// <summary>
        /// Aktywuj.
        /// </summary>
        /// <param name="kind">Pierwszy rodzaj tetrimino</param>
        public void Activate(TetriminoKind kind)
        {
            this.isActivated.Value = true;
            this.isUpperLimitOvered.Value = false;
            this.Tetrimino.Value = Models.Tetrimino.Create(kind);
            this.placedBlocks.Value = Array.Empty<Block>();
            this.Timer.Interval = 1000;
            this.Timer.Start();
        }

        /// <summary>
        /// Przesuwa tetrimino w określonym kierunku.
        /// </summary>
        /// <param name="direction">Kierunek ruchu</param>
        public void MoveTetrimino(MoveDirection direction)
        {
            if (!this.isActivated.Value)
                return;

            //--- Ruch w dół to specjalne traktowanie
            if (direction == MoveDirection.Down)
            {
                this.Timer.Stop();
                if (this.Tetrimino.Value.Move(direction, this.CheckCollision)) this.Tetrimino.ForceNotify();
                else this.FixTetrimino();
                this.Timer.Start();
                return;
            }

            //--- W przypadku ruchu lewo/prawo zmień powiadomienie, jeśli ruch się powiedzie
            if (this.Tetrimino.Value.Move(direction, this.CheckCollision))
                this.Tetrimino.ForceNotify();
        }

        /// <summary>
        /// Obraca tetrimino w określonym kierunku。
        /// </summary>
        /// <param name="direction">Kierunek rotacji</param>
        public void RotationTetrimino(RotationDirection direction)
        {
            if (!this.isActivated.Value)
                return;

            if (this.Tetrimino.Value.Rotation(direction, this.CheckCollision))
                this.Tetrimino.ForceNotify();
        }

        /// <summary>
        /// Wymuś potwierdzenie tetrimino。
        /// </summary>
        public void ForceFixTetrimino()
        {
            if (!this.isActivated.Value)
                return;

            this.Timer.Stop();
            while (this.Tetrimino.Value.Move(MoveDirection.Down, this.CheckCollision)) ;  //--- Poruszaj się, aż do kolizji
            this.FixTetrimino();
            this.Timer.Start();
        }

        /// <summary>
        /// Ustaw tetrimin.
        /// </summary>
        private void FixTetrimino()
        {
            //--- Potwierdź, że tetrimino zostało umieszczone i usuń je, gdy bloki są kompletne
            Tuple<int, Block[]> result = this.RemoveAndFixBlock();

            //--- Powiadom liczbę wyrównanych linii
            int removedRowCount = result.Item1;
            if (removedRowCount > 0)
                this.lastRemovedRowCount.Value = removedRowCount;

            //--- Jeśli blok przekroczy górną granicę, gra się kończy
            if (result.Item2.Any(x => x.Position.Row < 0))
            {
                this.isActivated.Value = false;
                this.isUpperLimitOvered.Value = true;
                return;
            }

            //--- aktualizacja
            this.Tetrimino.Value = null;  //--- 一Wyczyść raz
            this.placedBlocks.Value = result.Item2;
        }

        /// <summary>
        /// Zwiększa prędkość spadania tetrimino。
        /// </summary>
        public void SpeedUp()
        {
            const int min = 15;  //---Najszybszy 15ms
            double interval = this.Timer.Interval / 2;  //--- Podwójna prędkość
            this.Timer.Interval = Math.Max(interval, min);
        }
        #endregion

        #region Wyniki i inne
        /// <summary>
        /// Wykrywanie kolizji
        /// </summary>
        /// <param name="block">Blok do sprawdzenia</param>
        /// <returns>Prawda, jeśli jest konflikt</returns>
        private bool CheckCollision(Block block)
        {
            if (block == null)
                throw new ArgumentNullException(nameof(block));

            //--- Utknąłem w ścianie po lewej
            if (block.Position.Column < 0)
                return true;

            //--- Utknąłem w ścianie po prawej
            if (ColumnCount <= block.Position.Column)
                return true;

            //--- utknąłem w podłodze
            if (RowCount <= block.Position.Row)
                return true;

            //--- Są już umieszczone klocki
            return this.placedBlocks.Value.Any(x => x.Position == block.Position);
        }

        /// <summary>
        /// Jeśli wszystkie klocki są wyrównane, usuń je i ustaw umieszczone klocki.
        /// </summary>
        /// <returns>Potwierdzony umieszczony blok</returns>
        private Tuple<int, Block[]> RemoveAndFixBlock()
        {
            //--- Grupuj bloki według linii
            var rows = this.placedBlocks.Value
                        .Concat(this.Tetrimino.Value.Blocks)  //--- Zsyntetyzuj umieszczone bloki i tetrimino
                        .GroupBy(x => x.Position.Row)  //--- Grupuj według linii
                        .Select(x => new
                        {
                            Row = x.Key,
                            IsFilled = ColumnCount <= x.Count(),  //--- Czy jest pełny?
                            Blocks = x,
                        })
                        .ToArray();

            //--- Usuń wyrównane bloki i potwierdź
            Block[] blocks = rows
                        .OrderByDescending(x => x.Row)    //--- Sortuj od najgłębszego
                        .WithIndex(x => x.IsFilled)       //--- Zwiększaj za każdym razem, gdy zostanie znaleziony pasujący wiersz
                        .Where(x => !x.Element.IsFilled)  //--- Usuń wyrównane linie
                        .SelectMany(x =>
                        {
                            //--- Linie technologiczne, których nie trzeba przesuwać tak jak są
                            //--- Specjalne przetwarzanie w celu poprawy wydajności przetwarzania
                            if (x.Index == 0)
                                return x.Element.Blocks;

                            //--- Przesuń kroki poniżej znikniętej linii
                            return x.Element.Blocks.Select(y =>
                            {
                                Position position = new Position(y.Position.Row + x.Index, y.Position.Column);
                                return new Block(y.Color, position);
                            });
                        })
                        .ToArray();

            //--- Liczba usuniętych linii
            int removedRowCount = rows.Count(x => x.IsFilled);
            return Tuple.Create(removedRowCount, blocks);
        }
        #endregion
    }
}