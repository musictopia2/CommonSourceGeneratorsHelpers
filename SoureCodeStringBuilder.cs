using System.Text;

namespace CommonSourceGeneratorsHelpers;

internal class SoureCodeStringBuilder
{
    private const string DoubleQuote = "\""; //hopefully this simple.


    private const string _oneSpace = "    ";
    private const string _twoSpaces = "        ";
    private const string _threeSpaces = "            ";
    private const string _fourSpaces = "                ";


    int _indentLevel = 0;

    //readonly StringBuilder _currentLine = new();

    readonly StringBuilder _builds = new();
    private bool _needsNewLine;

    /// <summary>
    /// Ensures that current cursor position is not dirty (cursor position is zero). If dirty, writes line break
    /// </summary>
    /// <returns></returns>
    public SoureCodeStringBuilder EnsureEmptyLine()
    {
        if (_needsNewLine)
        {
            _builds.AppendLine();
            _needsNewLine = false;
        }
        return this;
    }


    /// <summary>
    /// Increases indentation level, so that the next text lines are all indented with an increased level. 
    /// </summary>
    private void InnerIncreaseIndent()
    {
        _indentLevel++;
    }
    /// <summary>
    /// Manually Increases indentation level, <br />
    /// so that the next text lines are all indented with an increased level. <br />
    /// If you're using helpers like WithIndent, WithCurlyBraces or WithPythonBlock you don't need to manually control indent level.
    /// </summary>
    public SoureCodeStringBuilder IncreaseIndent()
    {
        InnerIncreaseIndent();
        return this;
    }
    /// <summary>
    /// Decreases indentation level, so that the next text lines are all indented with a decreased level. 
    /// </summary>
    private void InnerDecreaseIndent()
    {
        _indentLevel--;
    }

    /// <summary>
    /// Manually Decreases indentation level, <br />
    /// so that the next text lines are all indented with an decreased level. <br />
    /// If you're using helpers like WithIndent, WithCurlyBraces or WithPythonBlock you don't need to manually control indent level.
    /// </summary>
    public SoureCodeStringBuilder DecreaseIndent()
    {
        InnerDecreaseIndent();
        return this;
    }
    public SoureCodeStringBuilder WriteLine(Action action)
    {
        Indent();
        action.Invoke(); //so i can break apart what is being written.
        _builds.AppendLine();
        _needsNewLine = false;
        return this;
    }
    public SoureCodeStringBuilder WriteLine(string text)
    {
        //string indent = string.Join("", _levelIndent.Reverse().ToList());
        //string content = Indent();
        //_builds.Append(content);
        Indent();
        _builds.AppendLine(text);
        _needsNewLine = false;

        return this;
    }

    private void Indent()
    {
        if (_indentLevel == 0)
        {
            return;
        }
        if (_indentLevel == 1)
        {
            _builds.Append(_oneSpace);
            return;
        }
        if (_indentLevel == 2)
        {
            _builds.Append(_twoSpaces);
            return;
        }
        if (_indentLevel == 3)
        {
            _builds.Append(_threeSpaces);
            return;
        }
        if (_indentLevel == 4)
        {
            _builds.Append(_fourSpaces);
            return;
        }
        throw new Exception("Only 4 spaces supported at the most");
        //StringBuilder build = new();
        //for (int i = 0; i < _indentLevel; i++)
        //{
        //    build.Append(IndentString);
        //}
        //return build.ToString();
    }

    public SoureCodeStringBuilder Write(object obj)
    {
        _needsNewLine = true; //i think.  otherwise, would do somewhere else.
        _builds.Append(obj);
        return this;
    }
    public override string ToString()
    {
        return _builds.ToString();
    }
    #region Custom Stuff
    public SoureCodeStringBuilder StartBlock()
    {
        Indent();
        Write("{")
            .IncreaseIndent().EnsureEmptyLine();
        return this;
    }
    public SoureCodeStringBuilder EndBlock()
    {
        DecreaseIndent().EnsureEmptyLine();
        Indent();
        Write("}");
        return this;
    }
    public SoureCodeStringBuilder AppendDoubleQuote(Action action)
    {
        Write(DoubleQuote);
        action.Invoke();
        Write(DoubleQuote);
        return this;
    }
    #endregion
}