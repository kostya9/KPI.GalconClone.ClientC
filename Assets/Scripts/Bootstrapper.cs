using strange.extensions.context.impl;

namespace KPI.GalconClone.ClientC
{
    public class Bootstrapper : ContextView 
    {
        // Start is called before the first frame update
        void Awake() {
            this.context = new SignalContext(this);
        }
    }
}