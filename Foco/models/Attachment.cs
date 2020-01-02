using System;
using System.Diagnostics;
namespace Foco.models
{
    public class Attachment
    {

        private string title;
        private string link;

        public string Title { get => title; set { if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(); title = value; } }
        public string Link { get => link; set { if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(); link = value; } }

        public Attachment(string title, string link)
        {
            Title = title;
            Link = link;
        }

        public static bool IsWebUrl(string link)
        {
            // from https://stackoverflow.com/questions/7578857/how-to-check-whether-a-string-is-a-valid-http-url
            return Uri.TryCreate(link, UriKind.Absolute, out Uri uriResult) && (uriResult.Scheme == Uri.UriSchemeHttps || uriResult.Scheme == Uri.UriSchemeHttp);
        }

        public bool IsWebUrl()
        {
            return Attachment.IsWebUrl(link);
        }

        public void OpenUrl()
        {
            try
            {
                Process.Start(link);
            }
            catch
            {
                Link = link.Replace("&", "^&");
                Process.Start(new ProcessStartInfo("cmd", $"/c start {link}") { CreateNoWindow = true });
            }
        }

    }
}