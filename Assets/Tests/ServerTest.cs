using System.Collections;
using UnityEngine.TestTools;

namespace Tests
{
    public class ServerTest
    {
        // A Test behaves as an ordinary method
        /*[Test]
        public async Task ServerTestSimplePasses()
        {
            var client = await ServerClient.Init("127.0.0.1", 8080);
            await Task.Delay(500);
            client.Dispose();
        }*/

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
