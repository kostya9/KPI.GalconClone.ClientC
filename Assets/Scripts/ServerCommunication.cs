using Assets.Scripts.Client;
using KPI.GalconClone.ClientC;
using Assets.Scripts.Config;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Timers;
using Assets.Scripts;

public class ServerCommunication : BaseView
{
    private ServerClient clientTest;

    [Inject]
    public ServerClient client { get; set; }

    [Inject]
    public AppHpInitiated AppHpInitiated { get; set; }

    [Inject]
    public MoveUnitInitiated MoveUnitInitiated { get; set; }

    [Inject]
    public PlanetLayoutStore Store { get; set; }

    private static Timer aTimer;
    private static Timer mTimer;
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
        startAddHpTimer();
        startMoveTimer();
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
                //client.SendAddHp(planetIds.ToArray()[0]); // to test 'addHp' work
            }
            pl.UnselectAll();
            pl.startAttackFlag = false;
        }
        
    }

    private void startAddHpTimer()
    {
        aTimer = new System.Timers.Timer();
        aTimer.Interval = 3000;
        aTimer.Elapsed += AddHpOnTimedEvent;
        aTimer.AutoReset = true; // Have the timer fire repeated events (true is the default)
        aTimer.Enabled = true; // start the timer
    }

    private void startMoveTimer()
    {
        mTimer = new System.Timers.Timer();
        mTimer.Interval = 1000;
        mTimer.Elapsed += MoveOnTimedEvent;
        mTimer.AutoReset = true; // Have the timer fire repeated events (true is the default)
        mTimer.Enabled = true; // start the timer
    }

    private void AddHpOnTimedEvent(object source, ElapsedEventArgs e)
    {
        AppHpInitiated.Dispatch();
        Debug.Log("AddHp timer ticks");
    }

    private void MoveOnTimedEvent(System.Object source, System.Timers.ElapsedEventArgs e)
    {
        Debug.Log("Move timer ticks");
        MoveUnitInitiated.Dispatch();
    }

    /*
     * better realization of OnTimedEvent, needs to be tested
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
