using Prism.Mvvm;

namespace TableAlgorithmicMethod.Models
{
    public class DataFormatItem : BindableBase
    {
        public string Name { get; }

        public int Identifier { get; }

        public DataFormatItem(string name, int identifier)
        {
            Name = name;
            Identifier = identifier;
        }
    }
}
