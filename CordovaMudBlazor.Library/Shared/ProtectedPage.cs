using System.Threading.Tasks;

namespace CordovaMudBlazor.Library
{
    public class ProtectedPage : BasePage
    {
        public bool SubView = false;
        public ProtectedPage() : base()
        {
            
        }
        

        protected override async Task OnInitializedAsync()
        {
            BrowserService.Refresh = new System.Action(Refresh);
            BrowserService.StateHasChanged = new System.Action(StateHasChanged);

            string token = BrowserService.GetToken();
            if (string.IsNullOrEmpty(token)) navMan.NavigateTo("login");
        }
    }
}
