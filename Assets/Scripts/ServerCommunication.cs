using Assets.Scripts.Client;
using KPI.GalconClone.ClientC;
using UnityEngine;

public class ServerCommunication : BaseView
{
    private ServerClient clientTest;

    [Inject]
    public ServerClient client { get; set; }

    protected override void Start()
    {
        base.Start();
        client.StartDispatchingEvents();
        client.SendReady();

        clientTest = ServerClient.Init("127.0.0.1", 10800);
        clientTest.SendReady();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
