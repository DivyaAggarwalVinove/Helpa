using Xamarin.Forms;

namespace Helpa
{
    public class ServiceButton : Button
    {
        public string serviceName;
        public bool isSelected;

        public void OnOffServices(bool isSelected)
        {
            MessagingCenter.Send<ServiceButton, bool>(this, "Select Or Unselect Service", isSelected);
        }
    }
}