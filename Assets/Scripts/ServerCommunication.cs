using Assets.Scripts.Client;
using KPI.GalconClone.ClientC;
using Assets.Scripts.Config;

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

        clientTest = ServerClient.Init(Server.Address, Server.Port);
        clientTest.SendReady();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
