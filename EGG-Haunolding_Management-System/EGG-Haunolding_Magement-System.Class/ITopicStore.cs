namespace EGG_Haunolding_Management_System.Class
{
    public interface ITopicStore
    {
        List<TopicItem> GetAllTopics();
        void AddTopic(TopicItem topicItem);
        List<int> GetTopicsByUser(string username);
        TopicItem GetTopicItemById(int id);
        void DeleteTopic(int id);
        void AddTopicToUser(string username, int id);
        void RemoveAllTopicsFromUser(string username);
    }
}