using System.Collections.Generic;

namespace GEDCOMLibrary
{
    public class Family
    {
        public string Id { get; set; }
        public Person Husband { get; set; }
        public Person Wife { get; set; }
        public List<Person> Children { get; set; }

        public Family()
        {
            Children = new List<Person>();
        }
        public void AddChild(Person person)
        {
            Children.Add(person);
        }

        public void RemoveChild(Person person)
        {
            Children.Remove(person);
        }

    }
}