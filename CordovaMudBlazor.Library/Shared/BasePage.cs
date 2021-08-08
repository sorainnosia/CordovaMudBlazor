using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace CordovaMudBlazor.Library
{
    public class BasePage : LayoutComponentBase, IDisposable
    {
        [Inject]
        protected NavigationManager navMan { get; set; }

        [Inject]
        public IJSRuntime jrt { get; set; }

        [Inject]
        public MyHttp http { get; set; }
        public Debouncer _searchDebouncer;

        public BasePage()
        {
            _searchDebouncer = new Debouncer(TimeSpan.FromSeconds(.75), ExportAllReal);
        }

        public void ExportAll()
        {
            _searchDebouncer.Action = ExportAllReal;
            _searchDebouncer.Invoke();
        }

        public virtual void ExportAllReal()
        {

        }

        void IDisposable.Dispose()
        {
            if (_searchDebouncer != null) _searchDebouncer.Dispose();
        }

        public async void Refresh()
        {
            
            _searchDebouncer.Action = RefreshReal;
            _searchDebouncer.Invoke();
        }

        public virtual void RefreshReal()
        {

        }

        protected override async Task OnInitializedAsync()
        {
            
        }
    }
}
