using CommonBasicLibraries.BasicDataSettingsAndProcesses;
using CommonBasicLibraries.CollectionClasses;
using System.Globalization;
namespace CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
public static class Strings
{
    extension(string payLoad)
    {
        public string BackSpaceRemoveEnding0s()
        {
            string output = payLoad.TrimEnd(['0']);
            return output;
        }

        //private static string _monthReplace = "";
        public BasicList<string> CommaDelimitedList()
        {
            return payLoad.Split(',').ToBasicList(); //comma alone.
        }
        public bool INumeric => int.TryParse(payLoad, out _);
        public int FindMonthInStringLine() // will return a number  this will assume that there is never a case where 2 months appear
        {
            BasicList<string> possList = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
            int possNum;
            possNum = 0;
            string monthReplace;
            int currentNum;
            foreach (var thisPoss in possList)
            {
                if (payLoad.Contains(thisPoss) == true)
                {
                    currentNum = thisPoss.MonthID;
                    if (currentNum > 0)
                    {
                        monthReplace = thisPoss;
                        if (possNum > 0)
                        {
                            throw new CustomBasicException("There should not have been 2 months in the same line.  Rethink");
                        }
                        possNum = currentNum;
                    }
                }
            }
            return possNum;
        }
        public int MonthID
        {
            get
            {
                return payLoad switch
                {
                    null => 0,
                    "January" => 1,
                    "February" => 2,
                    "March" => 3,
                    "April" => 4,
                    "May" => 5,
                    "June" => 6,
                    "July" => 7,
                    "August" => 8,
                    "September" => 9,
                    "October" => 10,
                    "November" => 11,
                    "December" => 12,
                    _ => 0
                };
            }
        }
        public BasicList<string> GetSentences()
        {
            BasicList<string> al = [];
            string sTemp = payLoad;
            sTemp = sTemp.Replace("\r\n", " ");
            string[] customSplit = [".", "?", "!", ":"];
            var splits = sTemp.Split(customSplit, StringSplitOptions.RemoveEmptyEntries).ToList();
            int pos;
            foreach (var thisSplit in splits)
            {
                pos = sTemp.IndexOf(thisSplit);
                var thisChar = sTemp.Trim().ToCharArray();
                if (pos + thisSplit.Length <= thisChar.Length - 1)
                {
                    char c = thisChar[pos + thisSplit.Length];
                    al.Add(thisSplit.Trim() + c.ToString());
                    sTemp = sTemp.Replace(thisSplit, ""); // because already accounted for.
                }
                else
                {
                    al.Add(thisSplit);
                }
            }
            if (al.First().StartsWith("\"") == true)
            {
                throw new CustomBasicException("I don't think the first one can start with quotes");
            }
            int x;
            var loopTo = al.Count - 1; // this is used so the quotes go into the proper places.
            for (x = 1; x <= loopTo; x++)
            {
                string firstItem;
                string secondItem;
                firstItem = al[x - 1];
                secondItem = al[x];
                if (secondItem.StartsWith("\"") == true)
                {
                    al[x] = al[x].Substring(1); // i think
                    al[x] = al[x].Trim();
                    al[x - 1] = al[x - 1] + "\"";
                    al[x - 1] = al[x - 1].Trim();
                }
                else if (secondItem.StartsWith(")") == true)
                {
                    al[x] = al[x].Substring(1); // i think
                    al[x] = al[x].Trim();
                    al[x - 1] = al[x - 1] + ")";
                    al[x - 1] = al[x - 1].Trim();
                }
                else if (secondItem.Length == 1)
                {
                    var ThisStr = secondItem.ToString();
                    al[x] = al[x].Substring(1); // i think
                    al[x] = al[x].Trim();
                    al[x - 1] = al[x - 1] + ThisStr;
                    al[x - 1] = al[x - 1].Trim();
                }
            }
            int numbers = al.Where(Items => Items == "").Count();
            int opening = al.Where(Items => Items == "(").Count();
            int closing = al.Where(Items => Items == ")").Count();
            foreach (var thisItem in al)
            {
                if (numbers == 1 || numbers == 3)
                {
                    throw new CustomBasicException("Quotes are not correct.  Has " + numbers + " Quotes");
                }
                if (opening != closing)
                {
                    throw new CustomBasicException("Opening and closing much match for ( and ) characters");
                }
            }
            al = (from xx in al
                  where xx != ""
                  select xx).ToBasicList();
            return al;
        }

        public string TextWithSpaces()
        {
            string newText = payLoad;
            int x = 0;
            string finals = "";
            foreach (var thisChar in newText)
            {
                bool rets = int.TryParse(thisChar.ToString(), out _);
                if (char.IsLower(thisChar) == false && x > 0 && rets == false)
                {
                    finals += " " + thisChar;
                }
                else
                {
                    finals += thisChar;
                }
                x++;
            }
            return finals;
        }
        public BasicList<string> Split(string words)
        {
            int oldCount = payLoad.Length;
            BasicList<string> tempList = [];
            do
            {
                if (payLoad.Contains(words) == false)
                {
                    if (payLoad != "")
                    {
                        tempList.Add(payLoad);
                    }
                    return tempList;
                }
                tempList.Add(payLoad.Substring(0, payLoad.IndexOf(words)));
                if (tempList.Count > oldCount)
                {
                    throw new CustomBasicException("Can't be more than " + oldCount);
                }
                payLoad = payLoad.Substring(payLoad.IndexOf(words) + words.Length);
            }
            while (true);
        }
        public bool ContainNumber => payLoad.Where(xx => char.IsNumber(xx) == true).Any();
        public string PartialString(string searchFor, bool beginning)
        {
            if (payLoad.Contains(searchFor) == false)
            {
                throw new CustomBasicException(searchFor + " is not contained in " + payLoad);
            }
            if (beginning == true)
            {
                return payLoad.Substring(0, payLoad.IndexOf(searchFor)).Trim();
            }
            return payLoad.Substring(payLoad.IndexOf(searchFor) + searchFor.Length).Trim();
        }
        public bool ContainsFromList(BasicList<string> thisList)
        {
            string temps;
            temps = payLoad.ToLower();
            foreach (var thisItem in thisList)
            {
                var news = thisItem.ToLower();
                if (temps.Contains(news) == true)
                {
                    return true;
                }
            }
            return false;
        }

        public string GetWords() // each upper case will represent a word.  for now; will not publish to bob's server.  if i need this function or needed for bob; then rethink that process
        {
            var tempList = payLoad.ToBasicList();
            int x = 0;
            string newText = "";
            if (payLoad.Contains(" ") == true)
            {
                throw new CustomBasicException(payLoad + " cannot contain spaces already");
            }
            tempList.ForEach(thisItem =>
            {
                if (char.IsUpper(thisItem) == true && x > 0)
                {
                    newText = newText + " " + thisItem;
                }
                else
                {
                    newText += thisItem;
                }
                x += 1;
            });
            newText = newText.Replace("I P ", " IP ");
            return newText;
        }
        public string ToTitleCase(bool replaceUnderstores = true)
        {
            if (replaceUnderstores)
            {
                payLoad = payLoad.Replace("_", " ");
            }
            TextInfo currentTextInfo = CultureInfo.InvariantCulture.TextInfo;
            string output = currentTextInfo.ToTitleCase(payLoad);
            return output;
        }
        public string ConvertCase(bool doAll = true)
        {
            string tempStr = "";
            bool isSpaceOrDot = false;
            if (doAll)
            {
                var loopTo = payLoad.Length - 1;
                for (int i = 0; i <= loopTo; i++)
                {
                    if (payLoad[i].ToString() != " " & payLoad[i].ToString() != ".")
                    {
                        if (i == 0 | isSpaceOrDot)
                        {
                            tempStr += char.ToUpper(payLoad[i]);
                            isSpaceOrDot = false;
                        }
                        else
                        {
                            tempStr += char.ToLower(payLoad[i]);
                        }
                    }
                    else
                    {
                        isSpaceOrDot = true;
                        tempStr += payLoad[i];
                    }
                }
            }
            else
            {
                var loopTo1 = payLoad.Length - 1;
                for (int i = 0; i <= loopTo1; i++)
                {
                    if (payLoad[i].ToString() != " " & payLoad[i].ToString() != ".")
                    {
                        if (isSpaceOrDot)
                        {
                            tempStr += char.ToUpper(payLoad[i]);
                            isSpaceOrDot = false;
                        }
                        else if (i == 0)
                        {
                            tempStr += char.ToUpper(payLoad[0]);
                        }
                        else
                        {
                            tempStr += char.ToLower(payLoad[i]);
                        }
                    }
                    else
                    {
                        if (payLoad[i].ToString() != " ")
                        {
                            isSpaceOrDot = !isSpaceOrDot;
                        }
                        tempStr += payLoad[i];
                    }
                }
            }
            return tempStr;
        }
        public BasicList<string> GenerateSentenceList()
        {
            return payLoad.Split(Constants.VBCrLf).ToBasicList();
        }
        public string DoubleQuoteString => $"{Constants.DoubleQuote}{payLoad}{Constants.DoubleQuote}";
    }
}