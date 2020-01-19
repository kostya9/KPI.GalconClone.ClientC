using Assets.Scripts.Client;
using KPI.GalconClone.ClientC;
using Assets.Scripts.Config;
using UnityEngine;
using System.Collections.Generic;

public class ServerCommunication : BaseView
{
    private ServerClient clientTest;

    [Inject]
    public ServerClient client { get; set; }
    
    [Inject]
    public PlanetLayoutStore Store { get; set; }

    protected override void Start()
    {
        base.Start();
        client.StartDispatchingEvents();
        client.SendReady();

        clientTest = ServerClient.Init(Server.Address, Server.Port);
        clientTest.SendReady();

        client.SendRendered();
        clientTest.SendRendered();
    }

    // Update is called once per frame
    void Update()
    {
        PlanetLayout pl = Store.GetPlanetLayout();
        if (pl != null && pl.startAttackFlag)
        {
            List<int> planetIds = pl.getSelectedIds();
            if (planetIds.Count != 0)
            {
                client.SendSelect(planetIds.ToArray(), Constants.PERCENTAGE);
            }
            pl.UnselectAll();
            pl.startAttackFlag = false;
        }
    }
}
