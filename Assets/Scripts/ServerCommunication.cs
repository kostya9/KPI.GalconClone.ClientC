using Assets.Scripts.Client;
using KPI.GalconClone.ClientC;
using Assets.Scripts.Config;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Timers;

public class ServerCommunication : BaseView
{
    private ServerClient clientTest;

    [Inject]
    public ServerClient client { get; set; }
    
    [Inject]
    public PlanetLayoutStore Store { get; set; }

    private static Timer aTimer;
    public static bool hpUpEventFlag;

    protected override void Start()
    {
        base.Start();
        client.StartDispatchingEvents();
        client.SendReady();

        clientTest = ServerClient.Init(Server.Address, Server.Port);
        clientTest.SendReady();

        client.SendRendered();
        clientTest.SendRendered();
        
        hpUpEventFlag = false;
        addHpTimerStart();
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
                //client.SendAddHp(planetIds.ToArray()[0]);
            }
            pl.UnselectAll();
            pl.startAttackFlag = false;
        }
        
        if (hpUpEventFlag)
        {
            foreach (Planet planet in pl)
            {
                if (planet.Owner != null)
                {
                    if (planet.Owner.Id == Constants.CURRENT_PLAYER_ID)
                    {
                        //client.SendAddHp(planet.Id);
                    }
                }
            }
            hpUpEventFlag = false;
        }
    }

    private void addHpTimerStart()
    {
        aTimer = new System.Timers.Timer();
        aTimer.Interval = 2000;
        aTimer.Elapsed += OnTimedEvent;
        aTimer.AutoReset = true; // Have the timer fire repeated events (true is the default)
        aTimer.Enabled = true; // start the timer
    }

    private static void OnTimedEvent(System.Object source, System.Timers.ElapsedEventArgs e)
    {
        Debug.Log("Timer is here");
        hpUpEventFlag = true;
    }

    /*
    private void OnTimedEvent(System.Object source, System.Timers.ElapsedEventArgs e)
    {
        Debug.Log("Timer is here");
        PlanetLayout pl = Store.GetPlanetLayout();
        foreach (Planet planet in pl)
        {
            if (planet.Owner != null)
            {
                if (planet.Owner.Id == Constants.CURRENT_PLAYER_ID)
                {
                    client.SendAddHp(planet.Id);
                }
            }
        }
    }
    */
}
