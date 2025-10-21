namespace Asteroids.Logic.Analytics.Core
{
    public struct AnalyticParameter
    {
        public string Name;
        public object Value;

        public AnalyticParameter(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Name}: {Value}";
        }
    }
}