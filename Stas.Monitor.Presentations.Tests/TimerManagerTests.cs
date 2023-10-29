using NUnit.Framework;
using Stas.Monitor.Presentations;
using System.Timers;
using System.Threading;

namespace Stas.Monitor.Presentations.Tests
{
    [TestFixture]
    public class TimerManagerTests
    {
        private TimerManager _timerManager;
        private int _elapsedCount;

        [SetUp]
        public void Setup()
        {
            _timerManager = new TimerManager();
            _elapsedCount = 0;
        }

        [Test]
        public void StartTimer_ShouldTriggerElapsedEvent()
        {
            _timerManager.StartTimer(OnElapsed);

            // Attendre un peu plus que l'intervalle du timer pour s'assurer qu'il a été déclenché
            Thread.Sleep(1100);

            Assert.AreEqual(1, _elapsedCount);
        }

        [Test]
        public void StopTimer_ShouldStopTriggeringElapsedEvent()
        {
            _timerManager.StartTimer(OnElapsed);
            Thread.Sleep(900); // Augmenter le temps d'attente pour s'assurer que l'événement Elapsed est déclenché au moins une fois
            _timerManager.StopTimer(OnElapsed);
            Thread.Sleep(900); // Attendre pour s'assurer que l'événement Elapsed n'est pas déclenché après l'arrêt du timer

            // Le timer a été démarré puis arrêté, donc l'événement Elapsed devrait être déclenché au moins une fois
            Assert.AreEqual(1, _elapsedCount);
        }


        private void OnElapsed(object sender, ElapsedEventArgs e)
        {
            _elapsedCount++;
        }
    }
}
