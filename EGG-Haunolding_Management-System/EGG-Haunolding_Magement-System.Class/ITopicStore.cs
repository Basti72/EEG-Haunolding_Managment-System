namespace EGG_Haunolding_Management_System.Class
{
    public interface ITopicStore
    {
        List<TopicItem> GetAllTopics();
        void AddTopic(TopicItem topicItem);
        List<int> GetTopicsByUser(string username);
        void DeleteTopic(int id);
        void AddTopicsToUser(string  username, List<int> ids);
        void RemoveTopicsFromUser(string username, List<int> ids);
    }
}