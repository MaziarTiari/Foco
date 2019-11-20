using System;
using System.IO;

namespace testFoco.models
{
    class FileAttachment : Attachment
    {
        private string path;
        private string comment;
        public FileAttachment(string title, string path) : base(path)
        {
            base.title = title;
            if (string.IsNullOrWhiteSpace(path)) throw new System.ArgumentException("message", nameof(path));
            this.Path = path;
        }
        public string Comment
        {
            get => comment;
            set
            {
                if (string.IsNullOrWhiteSpace(comment)) return;
                comment = value;
            }
        }

        public string Path { get => path; set => path = value; }

        public void OpenFile(string filePath, string programPath)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = programPath;
            p.StartInfo.Arguments = filePath;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            //start the exe
            p.Start();
        }
    }
}
