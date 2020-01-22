using Assets.Scripts.Client;
using KPI.GalconClone.ClientC;

namespace Assets.Scripts.Views
{
    public class MenuView : BaseView
    {
        [Inject]
        public ServerClient Client { get; set; }

        public void OnReady()
        {
            Client.SendReady();
        }
    }
}
