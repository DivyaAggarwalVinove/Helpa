using Xamarin.Forms;

namespace Helpa
{
    public class ScopeButton : Button
    {
        public string scopeName;
        public bool isSelected;

        public void OnOffScope(bool isSelected)
        {
            MessagingCenter.Send<ScopeButton, bool>(this, "Select Or Unselect Service", isSelected);
        }
    }
}