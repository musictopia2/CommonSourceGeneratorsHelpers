using CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.Misc;
using CommonBasicLibraries.BasicDataSettingsAndProcesses;
using CommonBasicLibraries.CollectionClasses;
using System.Globalization;
using System.Text.RegularExpressions; //not common enough to put into globals.
namespace CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
public static class Strings
{
    public static string BackSpaceRemoveEnding0s(this string payLoad)
    {
        string output = payLoad.TrimEnd(['0']);
        return output;
    }

    //private static string _monthReplace = "";
    public static BasicList<string> CommaDelimitedList(string payLoad)
    {
        return payLoad.Split(",").ToBasicList(); //comma alone.
    }
    public static bool IsNumeric(this string thisStr)
    {
        return int.TryParse(thisStr, out _); //you are forced to assign variable no matter what now.
    }
    public static int FindMonthInStringLine(this string thisStr) // will return a number  this will assume that there is never a case where 2 months appear
    {
        BasicList<string> possList = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
        int possNum;
        possNum = 0;
        string monthReplace;
        int currentNum;
        foreach (var thisPoss in possList)
        {
            if (thisStr.Contains(thisPoss) == true)
            {
                currentNum = thisPoss.GetMonthID();
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
    public static int GetMonthID(this string monthString)
    {

        return monthString switch
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
    public static BasicList<string> GetSentences(this string sTextToParse)
    {
        BasicList<string> al = [];
        string sTemp = sTextToParse;
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
    public static string StripHtml(this string htmlText) //unfortunately not perfect.
    {
        var thisText = Regex.Replace(htmlText, "<.*?>", "");
        if (thisText.Contains("<sup") == true)
        {
            var index = thisText.IndexOf("<sup");
            thisText = thisText.Substring(0, index);
        }
        if (thisText.Contains("<div class=" + "\"") == true)
        {
            thisText = thisText.Replace("<div class=" + "\"", "");
        }
        if (thisText.Contains("<a") == true)
        {
            var index = thisText.IndexOf("<a");
            thisText = thisText.Substring(0, index);
        }
        thisText = thisText.Replace("[a]", "");
        thisText = thisText.Replace("[b]", ""); // because even if its b, needs to go away as well.
        thisText = thisText.Replace("[c]", "");
        thisText = thisText.Replace("[d]", "");
        thisText = thisText.Replace("[e]", "");
        thisText = thisText.Replace("[f]", "");
        thisText = thisText.Replace("[g]", "");
        thisText = thisText.Replace("[h]", "");
        thisText = thisText.Replace("[i]", "");
        thisText = thisText.Replace("[j]", "");
        thisText = thisText.Replace("[k]", "");
        thisText = thisText.Replace("[l]", "");
        thisText = thisText.Replace("[m]", "");
        thisText = thisText.Replace("[n]", "");
        thisText = thisText.Replace("[o]", "");
        thisText = thisText.Replace("[p]", "");
        thisText = thisText.Replace("[q]", "");
        thisText = thisText.Replace("[r]", "");
        thisText = thisText.Replace("[s]", "");
        thisText = thisText.Replace("[t]", "");
        thisText = thisText.Replace("[u]", "");
        thisText = thisText.Replace("[v]", "");
        thisText = thisText.Replace("[w]", "");
        thisText = thisText.Replace("[x]", "");
        thisText = thisText.Replace("[y]", "");
        thisText = thisText.Replace("[z]", "");
        var nextText = System.Net.WebUtility.HtmlDecode(thisText);
        return nextText.Trim();
    }
    public static string TextWithSpaces(this string thisText)
    {
        string newText = thisText;
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
    public static int GetSeconds(this string timeString)
    {
        var tempList = timeString.Split(":").ToBasicList();
        if (tempList.Count > 3)
        {
            throw new CustomBasicException("Can't handle more than 3 :");
        }
        if (tempList.Count == 3)
        {
            int firstNum;
            int secondNum;
            int thirdNum;
            firstNum = int.Parse(tempList.First().ToString());
            secondNum = int.Parse(tempList[1]);
            thirdNum = int.Parse(tempList.Last());
            int firstSecs;
            firstSecs = firstNum * 60 * 60;
            var secondSecs = secondNum * 60;
            var thirdSecs = thirdNum;
            return firstSecs + secondSecs + thirdSecs;
        }
        if (tempList.Count == 2)
        {
            int firstSecs = int.Parse(tempList.First()) * 60;
            return firstSecs + int.Parse(tempList.Last());
        }
        if (tempList.Count == 0)
        {
            throw new CustomBasicException("I think its wrong");
        }
        if (tempList.Count == 1)
        {
            throw new CustomBasicException("Should just return as is");
        }
        throw new CustomBasicException("Not sure");
    }
    public static BasicList<string> GetRange(this BasicList<string> thisList, string startWithString, string endWithString)
    {
        int firstIndex = thisList.IndexOf(startWithString);
        int secondIndex = thisList.IndexOf(endWithString);
        if (firstIndex == -1)
        {
            throw new CustomBasicException(startWithString + " is not found for the start string");
        }
        if (secondIndex == -1)
        {
            throw new CustomBasicException(endWithString + " is not found for the end string");
        }
        if (firstIndex > secondIndex)
        {
            throw new CustomBasicException("The first string appears later in the last than the second string");
        }
        return thisList.Skip(firstIndex).Take(secondIndex - firstIndex + 1).ToBasicList();
    }
    public static BasicList<string> Split(this string thisStr, string words)
    {
        int oldCount = thisStr.Length;
        BasicList<string> tempList = [];
        do
        {
            if (thisStr.Contains(words) == false)
            {
                if (thisStr != "")
                {
                    tempList.Add(thisStr);
                }
                return tempList;
            }
            tempList.Add(thisStr.Substring(0, thisStr.IndexOf(words)));
            if (tempList.Count > oldCount)
            {
                throw new CustomBasicException("Can't be more than " + oldCount);
            }
            thisStr = thisStr.Substring(thisStr.IndexOf(words) + words.Length);
        }
        while (true);
    }
    public static string Join(this BasicList<string> thisList, string words)
    {
        string newWord = "";
        thisList.ForEach(temps =>
        {
            if (newWord == "")
            {
                newWord = temps;
            }
            else
            {
                newWord = newWord + words + temps;
            }
        });
        return newWord;
    }
    public static string Join(this BasicList<string> thisList, string words, int skip, int take)
    {
        var newList = thisList.Skip(skip).ToBasicList();
        if (take > 0)
        {
            newList = thisList.Take(take).ToBasicList();
        }
        return Join(newList, words);
    }
    public static bool ContainNumbers(this string thisStr)
    {
        return thisStr.Where(Items => char.IsNumber(Items) == true).Any();
    }
    public static string PartialString(this string fullString, string searchFor, bool beginning)
    {
        if (fullString.Contains(searchFor) == false)
        {
            throw new CustomBasicException(searchFor + " is not contained in " + fullString);
        }
        if (beginning == true)
        {
            return fullString.Substring(0, fullString.IndexOf(searchFor)).Trim();
        }
        return fullString.Substring(fullString.IndexOf(searchFor) + searchFor.Length).Trim();
    }
    public static bool ContainsFromList(this string thisStr, BasicList<string> thisList)
    {
        string temps;
        temps = thisStr.ToLower();
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
    public static bool PostalCodeValidForUS(this string postalCode)
    {
        if (postalCode.Length < 5)
        {
            return false;
        }
        if (postalCode.Length == 5)
        {
            return int.TryParse(postalCode, out _);
        }
        if (postalCode.Length == 9)
        {
            return int.TryParse(postalCode, out _);
        }
        if (postalCode.Length == 10)
        {
            int index;
            index = postalCode.IndexOf("-");
            if (index != 5)
            {
                return false;
            }
            postalCode = postalCode.Replace("-", "");
            return int.TryParse(postalCode, out _);
        }
        return false;
    }
    public static string GetWords(this string thisWord) // each upper case will represent a word.  for now; will not publish to bob's server.  if i need this function or needed for bob; then rethink that process
    {
        var tempList = thisWord.ToBasicList();
        int x = 0;
        string newText = "";
        if (thisWord.Contains(" ") == true)
        {
            throw new CustomBasicException(thisWord + " cannot contain spaces already");
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
    public static string ToTitleCase(this string info, bool replaceUnderstores = true)
    {
        if (replaceUnderstores)
        {
            info = info.Replace("_", " ");
        }
        TextInfo currentTextInfo = CultureInfo.InvariantCulture.TextInfo;
        string output = currentTextInfo.ToTitleCase(info);
        return output;
    }
    public static string ConvertCase(this string info, bool doAll = true)
    {
        string tempStr = "";
        bool isSpaceOrDot = false;
        if (doAll)
        {
            var loopTo = info.Length - 1;
            for (int i = 0; i <= loopTo; i++)
            {
                if (info[i].ToString() != " " & info[i].ToString() != ".")
                {
                    if (i == 0 | isSpaceOrDot)
                    {
                        tempStr += char.ToUpper(info[i]);
                        isSpaceOrDot = false;
                    }
                    else
                    {
                        tempStr += char.ToLower(info[i]);
                    }
                }
                else
                {
                    isSpaceOrDot = true;
                    tempStr += info[i];
                }
            }
        }
        else
        {
            var loopTo1 = info.Length - 1;
            for (int i = 0; i <= loopTo1; i++)
            {
                if (info[i].ToString() != " " & info[i].ToString() != ".")
                {
                    if (isSpaceOrDot)
                    {
                        tempStr += char.ToUpper(info[i]);
                        isSpaceOrDot = false;
                    }
                    else if (i == 0)
                    {
                        tempStr += char.ToUpper(info[0]);
                    }
                    else
                    {
                        tempStr += char.ToLower(info[i]);
                    }
                }
                else
                {
                    if (info[i].ToString() != " ")
                    {
                        isSpaceOrDot = !isSpaceOrDot;
                    }
                    tempStr += info[i];
                }
            }
        }
        return tempStr;
    }
    public static BasicList<string> GenerateSentenceList(this string entireText)
    {
        return entireText.Split(Constants.VBCrLf).ToBasicList();
    }
    public static string BodyFromStringList(this BasicList<string> thisList)
    {
        if (thisList.Count == 0)
        {
            throw new CustomBasicException("Must have at least one item in order to get the body from the string list");
        }
        StrCat cats = new();
        thisList.ForEach(ThisItem =>
        {
            cats.AddToString(ThisItem, Constants.VBCrLf);
        });
        return cats.GetInfo();
    }
    public static string GetDoubleQuoteString(this string value) => $"{Constants.DoubleQuote}{value}{Constants.DoubleQuote}";
}