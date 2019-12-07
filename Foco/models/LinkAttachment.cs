using System.Diagnostics;
namespace Foco.models
{
    public class LinkAttachment : Attachment
    {
        
        public LinkAttachment(string title, string link) : base(title, link)
        {
            Type = AttachmentType.Link;
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