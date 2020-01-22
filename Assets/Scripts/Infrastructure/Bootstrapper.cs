using Assets.Scripts.Infrastructure;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using UnityEngine;

namespace KPI.GalconClone.ClientC
{
    public class Bootstrapper : ContextView
    {
        public static Bootstrapper Instance => _instance;
        private static Bootstrapper _instance;

        [Inject]
        public MainThreadCommandExecutor executor { get; set; }

        public Bootstrapper()
        {
            if (_instance == null)
            {
                _instance = this;
            }
        }
        
        // Start is called before the first frame update
        void Awake() 
        {
            Application.runInBackground = true;
            this.context = new SignalContext(this, ContextStartupFlags.MANUAL_LAUNCH);
            context.Launch();
            context.AddView(this);
        }

        private void Update()
        {
            while (executor.ExecuteNextTask()) ;
        }
    }
}