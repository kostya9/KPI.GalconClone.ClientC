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

    protected override void Start()
    {
        base.Start();
        client.StartDispatchingEvents();

        //clientTest = ServerClient.Init(Server.Address, Server.Port);
        //clientTest.SendReady();


        //clientTest.SendRendered();
        
        startAddHpTimer();
        startMoveTimer();
    }

    public void ready()
    {

        client.SendReady();
    }

    // Update is called once per frame
    void Update()
    {
        PlanetLayout pl = Store?.GetPlanetLayout();
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

    private void startAddHpTimer()
    {
        aTimer = new System.Timers.Timer();
        aTimer.Interval = Constants.ADD_HP_INTERVAL;
        aTimer.Elapsed += AddHpOnTimedEvent;
        aTimer.AutoReset = true; // Have the timer fire repeated events (true is the default)
        aTimer.Enabled = true; // start the timer
    }

    private void startMoveTimer()
    {
        mTimer = new System.Timers.Timer();
        mTimer.Interval = Constants.MOVE_INTERVAL;
        mTimer.Elapsed += MoveOnTimedEvent;
        mTimer.AutoReset = true;
        mTimer.Enabled = true;
    }

    private void AddHpOnTimedEvent(object source, ElapsedEventArgs e)
    {
        //Debug.Log("AddHp timer ticks");
        AppHpInitiated.Dispatch();
    }

    private void MoveOnTimedEvent(System.Object source, System.Timers.ElapsedEventArgs e)
    {
        //Debug.Log("Move timer ticks");
        MoveUnitInitiated.Dispatch();
    }
}
