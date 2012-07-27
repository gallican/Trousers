using System;

namespace Trousers.Core
{
    public class WorkItemHistory
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Status {get;set;}
        public int StoryPoints { get; set; }
    }
}