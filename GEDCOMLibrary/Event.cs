using System;
using System.Text.RegularExpressions;

namespace GEDCOMLibrary
{
    public class Event : GEDCOMLibrary.ISerializable
    {
        /*
        An event 

        */

        public Event(EventType eventType, DateTime date, string location)
        {
            this.EventType = eventType;
            this.Date = date;
            this.Location = location;

        }
        public Event() {}
        public EventType EventType { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }

        public string Serialize()
        {
            throw new NotImplementedException();
        }

        public void SetDateFromString(string dateString)
        {
            int day;
            int month;
            int year;

            var rx = new Regex(@"^((About|about|Abt|abt|Before|before|Bef|bef|After|after|Aft|aft|bp|baptism|Baptism)[\.\- ,]*)?((?<year>\d{1,4})|(?<month>[A-Za-z]+)[\.\- \,](?<year>\d{1,4})|(?<month>[A-Za-z]+)[\.\- \,](?<day>\d{1,2})[\.\- \,]*(?<year>\d{1,4})|(?<day>\d{1,2})[\.\- \,](?<month>\w+)[\.\- \,]*(?<year>\d{1,4}))$");

            var match = rx.Match(dateString);
            if (match.Success)
            {
                if (match.Groups["day"].Length > 0)
                {
                    day = Convert.ToInt16(match.Groups["day"].Value);
                }
                else
                {
                    day = 1;
                }
                if (match.Groups["month"].Length > 0)
                {
                    string monthString = match.Groups["month"].Value;
                    month = GetMonthAsInt(monthString);
                }
                else
                {
                    month = 1;
                }
                year = Convert.ToInt16(match.Groups["year"].Value);

                Date = new DateTime(year, month, day);
            }
            else
            {
                Date = new DateTime();
            }

        }

        private int GetMonthAsInt(string monthString)
        {
            string threeLetterMonth = @"^[A-Za-z]{3}$";
            string monthName = @"^[A-Za-z]{3,}$";
            string monthAsInt = @"^\d{1,2}$";

            int result = 0;
            string lowerMonthString = monthString.ToLower();

            if (Regex.IsMatch(monthString, threeLetterMonth))
            {
                switch (lowerMonthString)
                {
                    case "jan":
                        result = 1;
                        break;
                    case "feb":
                        result = 2;
                        break;
                    case "mar":
                        result = 3;
                        break;
                    case "apr":
                        result = 4;
                        break;
                    case "may":
                        result = 5;
                        break;
                    case "jun":
                        result = 6;
                        break;
                    case "jul":
                        result = 7;
                        break;
                    case "aug":
                        result = 8;
                        break;
                    case "sep":
                        result = 9;
                        break;
                    case "oct":
                        result = 10;
                        break;
                    case "nov":
                        result = 11;
                        break;
                    case "dec":
                        result = 12;
                        break;
                    default:
                        result = 1;
                        break;
                }
            }


            if (Regex.IsMatch(monthString, monthName))
            {
                // result = monthString.ToLower() switch
                // {
                //     "january" => 1,
                //     "february" => 2,
                //     "march" => 3,
                //     "april" => 4,
                //     "may" => 5,
                //     "june" => 6,
                //     "july" => 7,
                //     "august" => 8,
                //     "september" => 9,
                //     "october" => 10,
                //     "november" => 11,
                //     "december" => 12,
                //     _ => 1
                // };

                switch (monthString.ToLower())
                {
                    case "january":
                        return 1;
                    case "february":
                        result = 2;
                        break;
                    case "march":
                        result = 3;
                        break;
                    case "april":
                        result = 4;
                        break;
                    case "may":
                        result = 5;
                        break;
                    case "june":
                        result = 6;
                        break;
                    case "july":
                        result = 7;
                        break;
                    case "august":
                        result = 8;
                        break;
                    case "september":
                        result = 9;
                        break;
                    case "october":
                        result = 10;
                        break;
                    case "november":
                        result = 11;
                        break;
                    case "december":
                        result = 12;
                        break;
                    default:
                        result = 1;
                        break;
                }
            }

            if (Regex.IsMatch(monthString, monthAsInt))
            {
                result = Convert.ToInt16(monthString);
            }

            return result;
        }
    }
}