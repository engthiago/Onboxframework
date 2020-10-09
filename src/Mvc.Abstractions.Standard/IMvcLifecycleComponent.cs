using System.Threading.Tasks;

namespace Onbox.Mvc.Abstractions.V7
{
    public interface IMvcLifecycleComponent
    {
        bool CanRetryOnError { get; set; }
        bool CanRetryOnWarning { get; set; }
        string Error { get; set; }
        bool IsLoading { get; set; }
        string Message { get; set; }
        string Warning { get; set; }

        void OnDestroy();
        void OnErrorRetry();
        void OnInit();
        Task OnInitAsync();
        void OnWarningRetry();
    }
}