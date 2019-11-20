using System.Diagnostics;
using System.Runtime.InteropServices;
namespace testFoco.models
{
    class LinkAttachment : Attachment
    {
        public LinkAttachment(string title, string link)
        {
            Title = title;
            Content = link;
            Type = AttachmentType.link;
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