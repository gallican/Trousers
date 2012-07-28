using System;

namespace Trousers.Core.Infrastructure
{
    public class WorkItemHistory
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Status {get;set;}
        public int StoryPoints { get; set; }
    }
}