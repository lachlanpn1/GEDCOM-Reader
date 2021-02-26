using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace GEDCOMLibrary
{
    public class GEDCOMReader
    {
        /*
            Accept a file with the .ged extension, read it and return an object of type FamilyTree.
        */
        Regex individualRx = new Regex(@"@P(?<id>\d+)@");
        Regex familyRx = new Regex(@"@F(?<id>\d+)@");

        Regex GEDCOMLineRx = new Regex(@"^(?<quantifier>\d{1}) (?<header>[\@A-Z-a-z0-9]+) ?(?<content>.*)$");

        public FamilyTree ProcessFile(StreamReader file)
        {
            var familyTree = new FamilyTree();
            GEDCOMLibrary.Event lifeEvent = null;
            Person person = null;
            Family family = null;

            string header = null;
            string content = null;
            string line = "";
            try
            {
                while ((line = file.ReadLine()) != null)
                {
                    // Debug.WriteLine($"Now processing {line}");
                    Match match = GEDCOMLineRx.Match(line);

                    if (match.Success)
                    {
                        header = match.Groups["header"].Value;
                        content = match.Groups["content"].Value;
                    }
                    else
                    {
                        // Debug.WriteLine("Loop continuing.");
                        continue;
                    }

                    match = individualRx.Match(header);
                    if (match.Success)
                    {
                        if (person != null)
                        {
                            if (lifeEvent != null)
                            {
                                // Debug.WriteLine("Added last event");
                                person.Events.Add(lifeEvent);
                                lifeEvent = null;
                            }
                            // Debug.WriteLine($"Saving person named {person.GivenName}");
                            familyTree.Members.Add(person);
                        }
                        // Debug.WriteLine("Creating new person");
                        person = new Person();
                        person.Id = match.Groups["id"].Value;
                        continue;
                    }

                    match = familyRx.Match(header);
                    if (match.Success)
                    {
                        if (family != null)
                        {
                            // Debug.WriteLine("Saving family");
                            if (family.Husband != null)
                            {
                                // Debug.WriteLine($"Added {family.Husband.GivenName} as husband of family {family.Id}");
                                family.Husband.SpouseFamily = family;
                            }
                            if (family.Wife != null)
                            {
                                // Debug.WriteLine($"Added {family.Wife.GivenName} as wife of family {family.Id}");
                                family.Wife.SpouseFamily = family;
                            }
                            if (family.Children != null)
                            {
                                foreach (Person child in family.Children)
                                {
                                    // Debug.WriteLine($"Added {child.GivenName} as a child of family {family.Id}");
                                    child.ChildFamily = family;
                                }
                            }
                        }
                        // Debug.WriteLine("Creating new family");
                        family = new Family();
                        family.Id = match.Groups["id"].Value;
                        continue;
                    }


                    if (header == "BIRT")
                    {
                        if (lifeEvent != null && person != null)
                        {
                            person.Events.Add(lifeEvent);
                        }
                        lifeEvent = new GEDCOMLibrary.Event();
                        // Debug.WriteLine("BIRTH: " + " made new event");
                        lifeEvent.EventType = EventType.Birth;
                    }
                    else if (header == "DATE")
                    {
                        // Debug.WriteLine($"Setting event date to {content}");
                        lifeEvent.SetDateFromString(content);
                        // Debug.WriteLine($"Event date was set to {lifeEvent.Date}");
                    }
                    else if (header == "PLAC")
                    {
                        // Debug.WriteLine($"Set event location to {content}");
                        lifeEvent.Location = content;
                    }
                    else if (header == "DEAT")
                    {
                        if (lifeEvent != null && person != null)
                        {
                            person.Events.Add(lifeEvent);
                        }
                        lifeEvent = new GEDCOMLibrary.Event();
                        // Debug.WriteLine("DEATH: " + " made new event");
                        lifeEvent.EventType = EventType.Death;
                    }
                    else if (header == "SEX")
                    {
                        // Debug.WriteLine($"Set individual's gender to {content}");
                        person.SetGenderFromString(content);
                    }
                    else if (header == "NAME")
                    {
                        if (person != null)
                        {
                            person.SetNamesFromString(content);
                            // Debug.WriteLine($"Setting individual's name to {content}");
                            // Debug.WriteLine($"Given name set to {person.GivenName}");
                            // Debug.WriteLine($"Middle name set to {person.MiddleNames}");
                            // Debug.WriteLine($"Family name set to {person.FamilyName}");
                        }
                    }
                    else if (header == "RESI")
                    {
                        if (lifeEvent != null && person != null)
                        {
                            person.Events.Add(lifeEvent);
                        }
                        lifeEvent = new GEDCOMLibrary.Event();
                        // Debug.WriteLine("RESIDENCE: " + " made new event");
                        lifeEvent.EventType = EventType.Residence;
                    }
                    else if (header == "HUSB")
                    {
                        match = individualRx.Match(content);
                        if (match.Success)
                        {
                            var husband = familyTree.FindPersonById(match.Groups["id"].Value);
                            family.Husband = husband;
                        }
                    }
                    else if (header == "WIFE")
                    {
                        match = individualRx.Match(content);
                        if (match.Success)
                        {
                            var wife = familyTree.FindPersonById(match.Groups["id"].Value);
                            family.Wife = wife;
                        }
                    }
                    else if (header == "CHIL")
                    {
                        match = individualRx.Match(content);
                        if (match.Success)
                        {
                            var child = familyTree.FindPersonById(match.Groups["id"].Value);
                            family.Children.Add(child);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(line + "\n");
                Console.WriteLine(ex.Message);
            }


            return familyTree;
        }

    }
}
