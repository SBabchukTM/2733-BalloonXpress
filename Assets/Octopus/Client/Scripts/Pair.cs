namespace Octopus.Client
{
    [System.Serializable]
    public class Pair
    {
        public string Key;

        public string Value;

        public Pair(string key, string value)
        {
            Key = key;

            Value = value;
        }
    }
}