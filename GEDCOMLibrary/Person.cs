using System.Text.RegularExpressions;
using System;

namespace GEDCOMLibrary
{
    using System.Collections.Generic;

    public class Person : GEDCOMLibrary.ISerializable, IEquatable<Person>
    {
        /*
            A member of a family tree.
        */

        Regex nameRx = new Regex(@"^((?<given>\w+) ?)?(?<middle>([a-zA-z-]+ ?)*)? *(\((?<nickname>[a-zA-Z]+)\))? ?(\/(?<family>\w+)\/)?$");

        public string Id { get; set; }
        public string GivenName { get; set; }
        public string MiddleNames { get; set; }
        public string FamilyName { get; set; }
        public string MaidenName { get; set; }
        public string NickName { get; set; }
        public Gender Gender { get; set; }
        public Family ChildFamily { get; set; } // family in which this person is a child.
        public Family SpouseFamily { get; set; } // family in which this person is a spouse.
        public List<Event> Events { get; set; }

        public Person()
        {
            Events = new List<Event>();
        }


        public string Serialize()
        {
            throw new System.NotImplementedException();
        }

        public void SetGenderFromString(string genderString)
        {
            if (genderString == "M")
            {
                Gender = Gender.Male;
            }
            else if (genderString == "F")
            {
                Gender = Gender.Female;
            }
            else
            {
                Gender = Gender.Unspecified;
            }
        }

        public void SetNamesFromString(string fullName)
        {
            Match match = nameRx.Match(fullName);
            if (match.Success)
            {
                GivenName = CapitaliseFirstLetter(match.Groups["given"].Value);
                MiddleNames = CapitaliseFirstLetter(match.Groups["middle"].Value);
                FamilyName = CapitaliseFirstLetter(match.Groups["family"].Value);
                MaidenName = CapitaliseFirstLetter(match.Groups["family"].Value);
                NickName = CapitaliseFirstLetter(match.Groups["nickname"].Value);
            }
            else
            {
                GivenName = fullName; // error figuring out name
                FamilyName = "Unknown.";
            }
        }

        public string CapitaliseFirstLetter(string word)
        {
            if (word.Length > 0)
            {
                if (word.Length > 1)
                {
                    return word[0].ToString().ToUpper() + word.Substring(1).ToLower();
                }
                return word.ToUpper();
            }
            return "";
        }

        public bool HasSpouse()
        {
            if(SpouseFamily == null)
            {
                return false;
            }
        
            if(Gender == Gender.Male) 
            {
                return !(SpouseFamily.Wife == null);
            } 
            else
            {
                return !(SpouseFamily.Husband == null);
            }
        }

        public bool HasFather()
        {
            if(ChildFamily == null)
            {
                return false;
            }
            else
            {
                return !(ChildFamily.Husband == null);
            }
        }

        public bool HasMother()
        {
            if (ChildFamily == null)
            {
                return false;
            }
            else
            {
                return !(ChildFamily.Wife == null);
            }
        }

        public bool HasParents()
        {
            if (ChildFamily == null)
            {
                return false;
            }
            else
            {
                return (HasMother() || HasFather());
            }
        }

        public bool HasChildren()
        {
            return (SpouseFamily != null) && (SpouseFamily.Children.Count > 0);
        }

        public string GetDisplayName(bool displayMaidenName)
        {
            if (GivenName == null)
            {
                GivenName = "";
            }
            if (MiddleNames == null)
            {
                MiddleNames = "";
            }
            if (FamilyName == null)
            {
                FamilyName = "";
            }
            if (MaidenName == null)
            {
                MaidenName = "";
            }
            string fullName;
            if (displayMaidenName)
            {
                if (Gender == Gender.Female)
                {
                    fullName = GivenName + ' ' + (MiddleNames != "" ? MiddleNames + ' ':"") + MaidenName;
                }
                else
                {
                    fullName = GivenName + ' ' + (MiddleNames != "" ? MiddleNames + ' ':"") + FamilyName;
                }
            }
            else
            {
                fullName = GivenName + ' ' + (MiddleNames != "" ? MiddleNames + ' ':"") + FamilyName;
            }
            if (fullName == "  ")
            {
                return "Unknown.";
            }
            else
            {
                return fullName;
            }
        }

        // return the period of life of the individual
        // Possible results:
        // 1901 - 1960
        // 1901 - Living
        // Unknown - 1960
        // 1905 - Unknown
        // Unknown
        public string GetLifeDatesAsString()
        {
            string birth;
            string death;
            int BirthIndex = Events.FindIndex(e => e.EventType == EventType.Birth);
            int DeathIndex = Events.FindIndex(e => e.EventType == EventType.Death);
            if (BirthIndex == -1)
            {
                birth = "Unknown";
            }
            else
            {
                birth = Events[BirthIndex].Date.ToString("yyyy");
            }
            if (DeathIndex == -1)
            {
                death = "Unknown";
            }
            else
            {
                death = Events[DeathIndex].Date.ToString("yyyy");
            }

            if (birth == "Unknown" && death == "Unknown")
            {
                return "Unknown";
            }
            else
            {
                return birth + " - " + death;
            }
        }

        public bool Equals(Person other)
        {
            Event birthEvent = Events.Find(e => e.EventType == EventType.Birth);
            Event otherBirthEvent = other.Events.Find(e => e.EventType == EventType.Birth);
            if(other == null)
            {
                return false;
            }

            return (this.FamilyName == other.FamilyName &&
                this.GivenName == other.GivenName && 
                birthEvent.Date == otherBirthEvent.Date
            );
        }
    }
}