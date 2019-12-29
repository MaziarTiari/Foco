using System;

namespace Foco.models
{
    public class Deadline
    {
        private string title;
        private DateTime date;

        public Deadline(string title, DateTime date)
        {
            this.title = title;
            this.date = date;
        }

        public string Title { get => title; set => title = value; }
        public DateTime Date { get => date; set => date = value; }
    }
}