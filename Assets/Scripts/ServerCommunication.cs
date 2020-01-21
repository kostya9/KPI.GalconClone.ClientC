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

    [Inject]
    public UnitLayoutStore UnitsStore { get; set; }

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

    private static void AddHpOnTimedEvent(System.Object source, System.Timers.ElapsedEventArgs e)
    {
        Debug.Log("AddHp timer ticks");
        hpUpEventFlag = true;
    }

    private void MoveOnTimedEvent(System.Object source, System.Timers.ElapsedEventArgs e)
    {
        Debug.Log("Move timer ticks");
        var unitsLayout = UnitsStore.GetUnitLayout();
        foreach (KeyValuePair<int, Unit> unitKeyValue in unitsLayout._units)
        {
            if (unitKeyValue.Value.Owner != null && unitKeyValue.Value.Owner.Id == Constants.CURRENT_PLAYER_ID)
            {
                Debug.Log("Here1");
                GameObject obj = GameObject.Find("Unit" + unitKeyValue.Key);
                Debug.Log("Here2");
                if (obj != null)
                {
                    Debug.Log("Not null");
                    UnitView uv = obj.GetComponent<UnitView>();
                    uv.Move();
                    Unit movingUnit = uv.GetUnit();
                    client.SendMove(movingUnit.Id, movingUnit.Position.x, movingUnit.Position.y);
                }
                else
                {
                    Debug.LogError("Error: obj is null");
                }
                //unit.Value.Move();
                Debug.Log("Unit: " + unitKeyValue.Value.Id + ", new Position: " + unitKeyValue.Value.Position);
            }
        }
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
