using strange.extensions.context.impl;
using strange.extensions.dispatcher.eventdispatcher.api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainContextView : ContextView
{

    public static MainContextView Instance { get { return instance; } }

    private static MainContextView instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        //Instantiate the context, passing it this instance.


        //This is the most basic of startup choices, and probably the most common.
        //You can also opt to pass in ContextStartFlag options, such as:
        //
        //context = new MyFirstContext(this, ContextStartupFlags.MANUAL_MAPPING);
        //context = new MyFirstContext(this, ContextStartupFlags.MANUAL_MAPPING | ContextStartupFlags.MANUAL_LAUNCH);
        //
        //These flags allow you, when necessary, to interrupt the startup sequence.
    }
    private bool isRunContext = false;
    static public bool isPauseDisable = false;

    public static IEventDispatcher strangeDispatcher = null;


    void Start()
    {
        GameObject.DontDestroyOnLoad(this);
        isRunContext = false;
    }

    public static void DispatchStrangeEvent(object eventType)
    {
        if (strangeDispatcher == null && instance != null && instance.context != null)
        {
            if ((instance.context as MainContextRoot).dispatcher != null)
            {
                strangeDispatcher = (instance.context as MainContextRoot).dispatcher;
            }
        }

        if (strangeDispatcher != null)
        {
            strangeDispatcher.Dispatch(eventType);
        }
        else
        {
            Debug.LogWarning("strangeDispatcher Not Ready");
        }
    }

    public static void DispatchStrangeEvent(object eventType, object data)
    {
        if (strangeDispatcher == null && instance != null && instance.context != null)
        {
            if ((instance.context as MainContextRoot).dispatcher != null)
            {
                strangeDispatcher = (instance.context as MainContextRoot).dispatcher;
            }
        }

        if (strangeDispatcher != null)
        {
            strangeDispatcher.Dispatch(eventType, data);
        }
        else
        {
            Debug.LogWarning("strangeDispatcher Not Ready");
        }
    }

    /// Remove a previously registered observer with exactly one argument from this Dispatcher
    public static void AddListenerStrangeEvent(object evt, EventCallback callback)
    {
        if (strangeDispatcher == null && instance != null && instance.context != null)
        {
            if ((instance.context as MainContextRoot).dispatcher != null)
            {
                strangeDispatcher = (instance.context as MainContextRoot).dispatcher;
            }
        }

        if (strangeDispatcher != null)
        {
            strangeDispatcher.AddListener(evt, callback);
        }
        else
        {
            Debug.LogWarning("strangeDispatcher Not Ready");
        }
    }

    public static void AddListenerStrangeEvent(object evt, EmptyCallback callback)
    {
        if (strangeDispatcher == null && instance != null && instance.context != null)
        {
            if ((instance.context as MainContextRoot).dispatcher != null)
            {
                strangeDispatcher = (instance.context as MainContextRoot).dispatcher;
            }
        }

        if (strangeDispatcher != null)
        {
            strangeDispatcher.AddListener(evt, callback);
        }
        else
        {
            Debug.LogWarning("strangeDispatcher Not Ready");
        }
    }

    /// Remove a previously registered observer with exactly no arguments from this Dispatcher
    public static void RemoveListenerStrangeEvent(object evt, EmptyCallback callback)
    {
        if (strangeDispatcher == null && instance != null && instance.context != null)
        {
            if ((instance.context as MainContextRoot).dispatcher != null)
            {
                strangeDispatcher = (instance.context as MainContextRoot).dispatcher;
            }
        }

        if (strangeDispatcher != null)
        {
            strangeDispatcher.RemoveListener(evt, callback);
        }
        else
        {
            Debug.LogWarning("strangeDispatcher Not Ready");
        }
    }

    /// Remove a previously registered observer with exactly no arguments from this Dispatcher
    public static void RemoveListenerStrangeEvent(object evt, EventCallback callback)
    {
        if (strangeDispatcher == null && instance != null && instance.context != null)
        {
            if ((instance.context as MainContextRoot).dispatcher != null)
            {
                strangeDispatcher = (instance.context as MainContextRoot).dispatcher;
            }
        }

        if (strangeDispatcher != null)
        {
            strangeDispatcher.RemoveListener(evt, callback);
        }
        else
        {
            Debug.LogWarning("strangeDispatcher Not Ready");
        }
    }


    /// Returns true if the provided observer is already registered
    public static bool HasListenerStrangeEvent(object evt, EventCallback callback)
    {
        if (strangeDispatcher == null && instance != null && instance.context != null)
        {
            if ((instance.context as MainContextRoot).dispatcher != null)
            {
                strangeDispatcher = (instance.context as MainContextRoot).dispatcher;
            }
        }

        if (strangeDispatcher != null)
        {
            return strangeDispatcher.HasListener(evt, callback);
        }
        else
        {
            Debug.Log("strangeDispatcher Not Ready");
        }
        return false;
    }

    public static bool HasListenerStrangeEvent(object evt, EmptyCallback callback)
    {
        if (strangeDispatcher == null && instance != null && instance.context != null)
        {
            if ((instance.context as MainContextRoot).dispatcher != null)
            {
                strangeDispatcher = (instance.context as MainContextRoot).dispatcher;
            }
        }

        if (strangeDispatcher != null)
        {
            return strangeDispatcher.HasListener(evt, callback);
        }
        else
        {
            Debug.Log("strangeDispatcher Not Ready");
        }
        return false;
    }

    void Update()
    {
        if (!isRunContext)
        {
            isRunContext = true;
            MonoBehaviour view = this;
            if (view != null)
            {
                context = new MainContextRoot(view);
                //context.Start();
            }
            else
            {
                Debug.LogError("MonoBehaviour == NULL & MainContextInput == NULL! ERROR context Not Started");
            }
        }
        else
        {
            if (context != null)
            {
                if (strangeDispatcher == null)
                {
                    if ((context as MainContextRoot).dispatcher != null)
                    {
                        strangeDispatcher = (context as MainContextRoot).dispatcher;
                    }
                }
            }
        }
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (!isRunContext || instance == null || strangeDispatcher == null)
        {
            return;
        }
        if (pauseStatus && !isPauseDisable)
        {
        }
    }

}