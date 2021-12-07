using CommonBasicLibraries.BasicDataSettingsAndProcesses;
using CommonBasicLibraries.CollectionClasses;
using System.Text.RegularExpressions; //not common enough
namespace CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.HtmlParserClasses;
public class HtmlParser
{
    private static readonly string _dEFAULT_WORD_SEPARATORS = " " + Constants.VBLF + Constants.VBCR + Constants.VBTab + "<>;,!";
    private string _error = "";
    public string ErrorPath = "";
    public string StartTag { get; set; } = "";
    public string EndTag { get; set; } = "";
    public HtmlParser() { }
    public HtmlParser(string body) { Body = body; }
    public BasicList<string> GetList(string strFirst, bool showErrors = true)
    {
        string tempStr;
        tempStr = Body;
        if (tempStr == "")
            throw new CustomBasicException("Blank list");
        var tGetList = new BasicList<string>();
        do
        {
            if (DoesExist(strFirst) == false)
            {
                if (tGetList.Count == 0)
                    if (showErrors == true)
                        throw new ParserException("There are no items on the list", EnumMethod.GetList) { OriginalBody = tempStr, RemainingHtml = Body, FirstTag = strFirst };
                tGetList.Add(Body);
                Body = tempStr;
                return tGetList;
            }
            tGetList.Add(GetTopInfo(strFirst));
            Body = GetBottomInfo(strFirst, true);
        }
        while (true);
    }
    public BasicList<string> GetList(string strFirst, string strSecond, bool showErrors = true)
    {
        string tempStr;
        tempStr = Body;
        if (tempStr == "")
            throw new CustomBasicException("Blank list");
        var tGetList = new BasicList<string>();
        string thisItem;
        do
        {
            if (DoesExist(strFirst, strSecond) == false)
            {
                if (tGetList.Count == 0)
                    if (showErrors == true)
                        throw new ParserException("There are no items on the list", EnumMethod.GetList) { OriginalBody = tempStr, RemainingHtml = Body, FirstTag = strFirst, SecondTag = strSecond };
                Body = tempStr;
                return tGetList;
            }
            thisItem = GetSomeInfo(strFirst, strSecond, true);
            tGetList.Add(thisItem);
        }
        while (true);
    }
    public string Body { get; set; } = "";
    public bool DoesExist()
    {
        CheckStartTag(); // ending is not required
        return DoesExist(StartTag, EndTag); // i think if nothing is put in there; will use the search terms provided.  to make it a little easier to maintain
    }
    public bool DoesExist(string strFirst, string strSecond = "")
    {
        if (Body.Length == 0)
        {
            return false;
        }
        string strTempBody = Body.ToLower();
        string strTmpFirst = strFirst.ToLower();
        string strTmpSecond = strSecond.ToLower();
        int first_pos = strTempBody.IndexOf(strTmpFirst);
        if (first_pos < 0)
        {
            return false;
        }
        if (strTmpSecond.Length > 0)
        {
            int second_pos = strTempBody.IndexOf(strTmpSecond, first_pos + strTmpFirst.Length);
            if (second_pos < 0)
            {
                return false;
            }
        }
        bool to_ret = true;
        return to_ret;
    }
    public async Task EliminateSeveralTopItemsAsync(BasicList<string> thisList)
    {
        await Task.Run(() =>
        {
            thisList.ForEach(Items =>
            {
                if (DoesExist(Items) == true)
                {
                    Body = GetTopInfo(Items);
                }
            });
        });
    }

    public string GetTopInfo(bool bIncludeThis = false)
    {
        if (EndTag == "")
        {
            throw new MissingTags(EnumLocation.Ending);
        }
        return GetTopInfo(EndTag, bIncludeThis);
    }
    public string GetTopInfo(string strTagEnd, bool bIncludeThis = false)
    {
        if (Body.Length == 0)
        {
            throw new Exception("GetTopInfo is blank");
        }
        string strTmpEnd = strTagEnd.ToLower();
        string strTmpBody = Body.ToLower();
        int n_find = strTmpBody.IndexOf(strTmpEnd);
        if (n_find < 0)
        {
            _error = "HtmlParser::GetTopInfo - string " + strTagEnd + " was not found.";
            throw new ParserException(_error, EnumMethod.GetTopInfo) { FirstTag = strTagEnd, OriginalBody = Body };
        }
        strTmpBody = Body;
        string to_ret;
        if (bIncludeThis)
        {
            to_ret = strTmpBody.Substring(0, n_find + strTagEnd.Length);
        }
        else
        {
            to_ret = strTmpBody.Substring(0, n_find);
        }
        return to_ret;
    }
    private void CheckStartTag()
    {
        if (StartTag == "")
        {
            throw new MissingTags(EnumLocation.Beginning);
        }
    }
    public string GetBottomInfo(bool bTakeOutBody = false, bool bIncludeThis = false)
    {
        CheckStartTag();
        return GetBottomInfo(StartTag, bTakeOutBody, bIncludeThis);
    }
    public string GetBottomInfo(string strStartTag, bool bTakeOutBody = false, bool bIncludeThis = false)
    {
        string to_ret = "";
        if (Body.Length == 0)
        {
            _error = "HtmlParser::GetBottomInfo - nothing in body";
            return to_ret;
        }
        string strTmpStartTag = strStartTag.ToLower();
        string strTmpBody = Body.ToLower();
        int n_find = strTmpBody.IndexOf(strTmpStartTag);
        if (n_find < 0)
        {
            _error = "HtmlParser::GetBottomInfo - string " + strStartTag + " was not found.";
            throw new ParserException(_error, EnumMethod.GetBottomInfo) { FirstTag = strStartTag, OriginalBody = Body };
        }
        strTmpBody = Body;
        if (bIncludeThis)
        {
            to_ret = strTmpBody.Substring(n_find, strTmpBody.Length - n_find);
        }
        else
        {
            to_ret = strTmpBody.Substring(n_find + strStartTag.Length, strTmpBody.Length - n_find - strStartTag.Length);
        }
        if (bTakeOutBody)
        {
            Body = to_ret;
        }
        return to_ret;
    }
    public string GetSomeInfo(bool bTakeOutBody = false, bool bIncludeFirst = false, bool bIncludeLast = false)
    {
        CheckStartTag();
        if (EndTag == "")
        {
            throw new MissingTags(EnumLocation.Ending);
        }
        return GetSomeInfo(StartTag, EndTag, bTakeOutBody, bIncludeFirst, bIncludeLast);
    }
    public string GetSomeInfo(string bstrStartTag, string bstrEndTag, bool bTakeOutBody = false, bool bIncludeFirst = false, bool bIncludeLast = false)
    {
        if (Body.Length == 0)
        {
            _error = "HtmlParser::GetSomeInfo - nothing in body";
            throw new Exception("GetSomeInfo body is blank");
        }
        string strStartTag = bstrStartTag.ToLower();
        string strTempBody = Body.ToLower();
        int n_find = strTempBody.IndexOf(strStartTag);
        if (n_find < 0)
        {
            _error = "HtmlParser::GetSomeInfo - string number 1" + bstrStartTag + " was not found.";
            throw new ParserException(_error, EnumMethod.GetSomeInfo) { FirstTag = bstrStartTag, SecondTag = bstrEndTag, OriginalBody = Body };
        }
        strTempBody = Body;
        int nDeletePos;
        if (bIncludeFirst)
        {
            nDeletePos = n_find;
        }
        else
        {
            nDeletePos = n_find + bstrStartTag.Length;
        }
        strTempBody = strTempBody.Substring(nDeletePos, strTempBody.Length - nDeletePos);
        string strEndTag = bstrEndTag.ToLower();
        string strTempBody2 = strTempBody.ToLower();
        n_find = strTempBody2.IndexOf(strEndTag);
        if (n_find < 0)
        {
            _error = "HtmlParser::GetSomeInfo - string number 2" + bstrEndTag + " was not found.";
            throw new ParserException(_error, EnumMethod.GetSomeInfo) { FirstTag = bstrStartTag, SecondTag = bstrEndTag, OriginalBody = Body, RemainingHtml = strTempBody2 };
        }
        if (bTakeOutBody)
        {
            if (bIncludeLast)
            {
                Body = strTempBody.Substring(n_find + strEndTag.Length, strTempBody.Length - n_find - strEndTag.Length);
            }
            else
            {
                Body = strTempBody.Substring(n_find, strTempBody.Length - n_find);
            }
        }
        string to_ret;
        if (bIncludeLast)
        {
            to_ret = strTempBody.Substring(0, n_find + strEndTag.Length);
        }
        else
        {
            to_ret = strTempBody.Substring(0, n_find);
        }
        return to_ret;
    }
    public string TextWithLinks()
    {
        string to_ret = "";
        if (Body.Length == 0)
        {
            return to_ret;
        }
        try
        {
            string str_tmp_body = "";
            string str_lower_body = Body.ToLower();
            string str_body = Body;
            int body_start = str_lower_body.IndexOf("<body");
            if (body_start >= 0)
            {
                int body_end = str_lower_body.IndexOf("</body", body_start);
                if (body_end < 0)
                {
                    str_body = Body.Substring(body_start, Body.Length - body_start);
                }
                else
                {
                    str_body = Body.Substring(body_start, body_end - body_start);
                }
                str_lower_body = str_body.ToLower();
            }
            RemoveAllTags(ref str_body, ref str_lower_body, "script");
            RemoveAllTags(ref str_body, ref str_lower_body, "style");
            str_tmp_body = StripTags(ref str_body);
            str_tmp_body = str_tmp_body.Replace(Constants.VBCrLf, "<br>");
            str_tmp_body = str_tmp_body.Replace(Constants.VBLF, "<br>");
            str_tmp_body = str_tmp_body.Replace(Constants.VBCR, "<br>");
            str_tmp_body = str_tmp_body.Replace("  ", "&nbsp;&nbsp;");
            str_tmp_body = str_tmp_body.Replace(" &nbsp;", "&nbsp;&nbsp;");
            str_tmp_body = str_tmp_body.Replace("&nbsp; ", "&nbsp;&nbsp;");
            int curPos = 0;
            int start_pos = -1;
            do
            {
                while (curPos < str_tmp_body.Length && (str_tmp_body[curPos] == ' ' || str_tmp_body[curPos].ToString() == Constants.VBLF || str_tmp_body[curPos].ToString() == Constants.VBCR || str_tmp_body[curPos].ToString() == Constants.VBTab))
                {
                    curPos += 1;
                }
                start_pos = curPos;
                string strToken = PickWord(ref curPos, ref str_tmp_body, _dEFAULT_WORD_SEPARATORS);
                if (strToken.Length == 0)
                {
                    curPos += 1;
                    continue;
                }
                string to_replace_with = "";
                string strLowerToken = strToken.ToLower();
                if (Substr(ref strLowerToken, 0, 7) == "http://" || Substr(ref strLowerToken, 0, 8) == "https://" || Substr(ref strLowerToken, 0, 6) == "ftp://")
                {
                    to_replace_with = string.Format("<a href=\"{0}\">{1}</a>", strToken, strToken);
                }
                else if (Substr(ref strLowerToken, 0, 4) == "www.")
                {
                    to_replace_with = string.Format("<a href=\"http://{0}\">{1}</a>", strToken, strToken);
                }
                else if (ValidEmailAddressFormat(strLowerToken))
                {
                    to_replace_with = string.Format("<a href=\"mailto:{0}\">{1}</a>", strToken, strToken);
                }
                if (to_replace_with.Length > 0)
                {
                    int flag_pos;
                    if (curPos > 0)
                    {
                        flag_pos = curPos - strToken.Length;
                    }
                    else
                    {
                        flag_pos = str_tmp_body.Length - strToken.Length;
                    }
                    str_tmp_body = str_tmp_body.Remove(flag_pos, strToken.Length);
                    str_tmp_body = str_tmp_body.Insert(flag_pos, to_replace_with);
                    curPos = flag_pos + to_replace_with.Length;
                }
                curPos += 1;
            }
            while (curPos > start_pos && curPos < str_tmp_body.Length - 1);
            return str_tmp_body;
        }
        catch (Exception ex)
        {
            _error = "HtmlParser::TextWithLinks - got exception (" + ex.Source + " - " + ex.Message + ")";
            throw ex;
        }
    }
    public string HtmlToString()
    {
        string buff = Body;
        buff = buff.Replace("<br>", Constants.VBLF);
        buff = buff.Replace("&nbsp;", " ");
        return StripTags(ref buff);
    }
    public static bool ValidEmailAddressFormat(string strEmailAddress)
    {
        bool to_ret = false;
        if (strEmailAddress.Length == 0)
        {
            return to_ret;
        }
        try
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new(strRegex);
            if (re.IsMatch(strEmailAddress))
            {
                return true;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return to_ret;
    }
    private static int RemoveAllTags(ref string body, ref string lower_body, string tag)
    {
        int start = 0;
        int end = -1;
        int crt_pos = 0;
        while (ExtractScript(ref lower_body, tag, ref start, ref end, ref crt_pos))
        {
            body = body.Remove(start, end - start);
            lower_body = lower_body.Remove(start, end - start);
            end -= end - start;
            crt_pos = end;
        }
        return 1;
    }
    private static bool ExtractScript(ref string strSrc, string tag, ref int start, ref int end, ref int crt_pos)
    {
        string start_tag = string.Format("<{0}", tag);
        string end_tag = string.Format("</{0}>", tag);
        start = strSrc.IndexOf(start_tag, crt_pos);
        if (start < 0)
        {
            return false;
        }
        end = strSrc.IndexOf(end_tag, start);
        if (end < 0)
        {
            return false;
        }
        end += end_tag.Length;
        return true;
    }
    private static string StripTags(ref string strToStrip)
    {
        string to_ret = "";
        int sz_dest_len = strToStrip.Length;
        bool in_tag = false;
        int k = 0;
        while (k < sz_dest_len)
        {
            if (strToStrip[k] == '<')
            {
                in_tag = true;
            }
            if (!in_tag)
            {
                to_ret += strToStrip[k];
            }
            if (strToStrip[k] == '>' && in_tag)
            {
                in_tag = false;
            }
            k += 1;
        }
        return to_ret;
    }
    private static string PickWord(ref int start, ref string strSrc, string strDelim)
    {
        string to_ret = "";
        if (start >= strSrc.Length)
        {
            return to_ret;
        }
        int next_pos = strSrc.IndexOfAny(strDelim.ToCharArray(), start);
        if (next_pos < 0)
        {
            to_ret = strSrc.Substring(start, strSrc.Length - start);
        }
        else
        {
            to_ret = strSrc.Substring(start, next_pos - start);
        }
        start = next_pos;
        return to_ret;
    }
    private static string Substr(ref string str, int index, int count)
    {
        if (index + count > str.Length)
        {
            return str;
        }
        return str.Substring(index, count);
    }
}