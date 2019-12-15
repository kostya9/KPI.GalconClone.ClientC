using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Assets.Scripts.Client;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ServerTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void ServerTestSimplePasses()
        {
            var client1 = ServerClient.Init("127.0.0.1", 10800);
            var client2 = ServerClient.Init("127.0.0.1", 10800);
            client1.SendReady();
            client2.SendReady();
            Thread.Sleep(5_000);
            client1.Dispose();
            client2.Dispose();
        }

        [Test]
        public void A()
        {
            Assert.True(true);
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
