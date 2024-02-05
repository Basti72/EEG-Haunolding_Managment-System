namespace EGG_Haunolding_Management_System.Class
{
    public class TopicItem
    {
        public int Id { get; set; }
        public string Topic { get; set; }

        public TopicItem(int id, string topic)
        {
            Id = id;
            Topic = topic;
        }
    }
}