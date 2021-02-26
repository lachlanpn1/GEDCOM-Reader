using System.Collections.Generic;

using System.Text;

namespace GEDCOMLibrary
{
    public class FamilyTree
    {
        /*
            Contains a list of people who are members of a family.
        */
        public FamilyTree() {
            Members = new List<Person>();
        }
        public List<Person> Members {get; set;}

        public override string ToString()
        {
            string result = "FAMILY TREE\n";
            foreach(Person member in Members)
            {
                result += member.Id + " : " + member.GivenName + " " + member.FamilyName + "\n";
            }
            result += "-- END --\n";
            return result;
        }

        public Person FindPersonById(string id) 
        {
            return Members.Find(person => person.Id == id);
        }
    }
}