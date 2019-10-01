using System.Collections;
using System.Threading;
using Assets.Scripts.Client;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests
{
    public class ServerTest
    {
        // Run only when server is started
        [Test]
        public void InitsMapWhenReady()
        {
            using (var client1 = InitClient())
            using (var client2 = InitClient())
            {
                MapContent map = null;
                var gameStartedCount = 0;
                var mapGeneratedCount = 0;

                client1.MapGeneratedSignal.AddListener((received) => { mapGeneratedCount++; map = received; }) ;
                client1.GameStartedSignal.AddListener(() => gameStartedCount++);
                client1.StartDispatchingEvents();

                client2.MapGeneratedSignal.AddListener((_) => mapGeneratedCount++);
                client2.GameStartedSignal.AddListener(() => gameStartedCount++);
                client2.StartDispatchingEvents();

                client1.SendReady();
                client2.SendReady();

                Thread.Sleep(500);
                Assert.AreEqual(2, mapGeneratedCount);
                Assert.NotNull(map?.Map);

                client1.SendRendered();
                client2.SendRendered();
                Thread.Sleep(500);
                Assert.AreEqual(2, gameStartedCount);
            }
        }

        private ServerClient InitClient()
        {
            var client = ServerClient.Init("127.0.0.1", 10800);
            client.MapGeneratedSignal = new MapGenerated();
            client.GameStartedSignal = new GameStarted();
            client.PlayerConnectedSignal = new PlayerConnected();
            client.PlayerInitializedSignal = new PlayerInitialized();
            client.HpAddedSignal = new HpAdded();
            client.GameOverSignal = new GameOver();
            client.DamageDoneSignal = new DamageDone();
            client.PlayerReadySignal = new PlayerReady();
            return client;
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator ServerTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
