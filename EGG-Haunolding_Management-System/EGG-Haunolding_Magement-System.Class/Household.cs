namespace EGG_Haunolding_Management_System.Class
{
    public class Household
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public Household(string name, int value)
        {
            Name = name;
            Value = value;
        }
    }
}