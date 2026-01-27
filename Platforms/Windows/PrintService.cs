using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MyJournal.Services;
using Windows.Graphics.Printing;


namespace MyJournal.Services
{
    public class PrintService : IPrintService
    {
        public async Task Print(string jobName, string htmlContent)
        {
            var printWindow = new Microsoft.UI.Xaml.Window();
            printWindow.Title = jobName;

            var webView = new WebView2();
            printWindow.Content = webView;
            printWindow.Activate();

            await webView.EnsureCoreWebView2Async();

            webView.NavigationCompleted += (sender, args) =>
            {
                if (args.IsSuccess)
                {
                    webView.CoreWebView2.ShowPrintUI(Microsoft.Web.WebView2.Core.CoreWebView2PrintDialogKind.Browser);
                }
            };
            webView.NavigateToString(htmlContent);

        }
    }
}
