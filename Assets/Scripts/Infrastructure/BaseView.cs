using strange.extensions.context.api;
using strange.extensions.mediation.api;
using UnityEngine;

namespace KPI.GalconClone.ClientC
{
    public class BaseView : MonoBehaviour, IView
    {
        private bool _requiresContext = true;
        public bool requiresContext
        {
            get
            {
                return _requiresContext;
            }
            set
            {
                _requiresContext = value;
            }
        }

        /// Determines the type of event the View is bubbling to the Context
        protected enum BubbleType
        {
            Add,
            Remove,
            Enable,
            Disable
        }

        /// A flag for allowing the View to register with the Context
        /// In general you can ignore this. But some developers have asked for a way of disabling
        ///  View registration with a checkbox from Unity, so here it is.
        /// If you want to expose this capability either
        /// (1) uncomment the commented-out line immediately below, or
        /// (2) subclass View and override the autoRegisterWithContext method using your own custom (public) field.
        //[SerializeField]
        protected bool registerWithContext = true;
        virtual public bool autoRegisterWithContext
        {
            get { return registerWithContext; }
            set { registerWithContext = value; }
        }

        public bool registeredWithContext { get; set; }

        /// A MonoBehaviour Awake handler.
        /// The View will attempt to connect to the Context at this moment.
        protected virtual void Awake()
        {
            if (autoRegisterWithContext && !registeredWithContext && shouldRegister)
                bubbleToContext(this, BubbleType.Add, false);
        }

        /// A MonoBehaviour Start handler
        /// If the View is not yet registered with the Context, it will 
        /// attempt to connect again at this moment.
        protected virtual void Start()
        {
            if (autoRegisterWithContext && !registeredWithContext && shouldRegister)
                bubbleToContext(this, BubbleType.Add, true);

        }



        /// A MonoBehaviour OnDisable handler
        /// The View will inform the Context that it was disabled
        protected virtual void OnDestroy()
        {
            bubbleToContext(this, BubbleType.Remove, false);
        }

        /// Recurses through Transform.parent to find the GameObject to which ContextView is attached
        /// Has a loop limit of 100 levels.
        /// By default, raises an Exception if no Context is found.
        virtual protected void bubbleToContext(MonoBehaviour view, BubbleType type, bool finalTry)
        {
            IContext context = Bootstrapper.Instance?.context;

            if(context == null)
            {
                if(finalTry)
                {
                    Debug.LogError("Context is null");
                }

                return;
            }

            switch (type)
            {
                case BubbleType.Add:
                    context.AddView(view);
                    registeredWithContext = true;
                    break;
                case BubbleType.Remove:
                    context.RemoveView(view);
                    break;
                default:
                    break;
            }    
        }

        public bool shouldRegister { get { return enabled && gameObject.activeInHierarchy; } }

    }
}