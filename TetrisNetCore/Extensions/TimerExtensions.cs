using System;
using System.Reactive.Linq;
using System.Timers;

namespace TetrisNetCore.Extensions
{
    /// <summary>
    /// Zapewnia rozszerzenia do System.Timers.Timer.
    /// </summary>
    public static class TimerExtensions
    {
        /// <summary>
        /// Zwraca zdarzenie Elapsed jako IObservable<T>.
        /// </summary>
        /// <param name="self">regulator czasowy</param>
        /// <returns>Sekwencja zdarzeń</returns>
        public static IObservable<ElapsedEventArgs> ElapsedAsObservable(this Timer self)
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self));

            return Observable.FromEvent<ElapsedEventHandler, ElapsedEventArgs>
            (
                h => (s, e) => h(e),
                h => self.Elapsed += h,
                h => self.Elapsed -= h
            );
        }
    }
}
