using strange.extensions.context.api;
using strange.extensions.context.impl;

namespace KPI.GalconClone.ClientC
{
    public class Bootstrapper : ContextView
    {
        public static Bootstrapper Instance => _instance;
        private static Bootstrapper _instance;

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
            this.context = new SignalContext(this, ContextStartupFlags.MANUAL_LAUNCH);
            context.Launch();
        }
    }
}