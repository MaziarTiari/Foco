using System;
using System.Diagnostics;
namespace Foco.models
{
    public class LinkAttachment : Attachment
    {

        public LinkAttachment(string title, string link) : base(title, link)
        {
            Type = AttachmentType.Link;
        }

        public bool IsWebUrl()
        {
            // from https://stackoverflow.com/questions/7578857/how-to-check-whether-a-string-is-a-valid-http-url
            return Uri.TryCreate(content, UriKind.Absolute, out Uri uriResult) && (uriResult.Scheme == Uri.UriSchemeHttps || uriResult.Scheme == Uri.UriSchemeHttp);
        }

        public void OpenUrl()
        {
            try
            {
                Process.Start(Content);
            }
            catch
            {
                Content = Content.Replace("&", "^&");
                Process.Start(new ProcessStartInfo("cmd", $"/c start {Content}") { CreateNoWindow = true });
            }
        }

    }
}