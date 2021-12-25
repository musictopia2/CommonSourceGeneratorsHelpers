using System.Text;
namespace CommonSourceGeneratorsHelpers;
internal class SourceCodeStringBuilder
{
    private const string DoubleQuote = "\"";
    private const string _oneSpace = "    ";
    private const string _twoSpaces = "        ";
    private const string _threeSpaces = "            ";
    private const string _fourSpaces = "                ";
    private const string _fiveSpaces = "                    ";
    private const string _sixSpaces = "                        ";
    private const string _sevenSpaces = "                            ";
    int _indentLevel = 0;

    //readonly StringBuilder _currentLine = new();

    readonly StringBuilder _builds = new();
    private bool _needsNewLine;

    /// <summary>
    /// Ensures that current cursor position is not dirty (cursor position is zero). If dirty, writes line break
    /// </summary>
    /// <returns></returns>
    public SourceCodeStringBuilder EnsureEmptyLine()
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
    public SourceCodeStringBuilder IncreaseIndent()
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
    public SourceCodeStringBuilder DecreaseIndent()
    {
        InnerDecreaseIndent();
        return this;
    }
    public SourceCodeStringBuilder WriteLine(Action action)
    {
        Indent();
        action.Invoke(); //so i can break apart what is being written.
        _builds.AppendLine();
        _needsNewLine = false;
        return this;
    }
    public SourceCodeStringBuilder WriteLine(string text)
    {
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
        if (_indentLevel == 5)
        {
            _builds.Append(_fiveSpaces);
            return;
        }
        if (_indentLevel == 6)
        {
            _builds.Append(_sixSpaces);
        }
        if (_indentLevel == 7)
        {
            _builds.Append(_sevenSpaces);
        }
        throw new Exception("Only 7 spaces supported at the most");
    }
    public SourceCodeStringBuilder Write(object obj)
    {
        _needsNewLine = true;
        _builds.Append(obj);
        return this;
    }
    public override string ToString()
    {
        return _builds.ToString();
    }
    #region Custom Stuff
    public SourceCodeStringBuilder StartBlock()
    {
        Indent();
        Write("{")
            .IncreaseIndent().EnsureEmptyLine();
        return this;
    }
    public SourceCodeStringBuilder EndBlock(bool alsoBlankLine = false)
    {
        DecreaseIndent().EnsureEmptyLine();
        Indent();
        Write("}");
        if (alsoBlankLine)
        {
            _builds.AppendLine();
        }
        return this;
    }
    public SourceCodeStringBuilder AppendDoubleQuote(Action action)
    {
        Write(DoubleQuote);
        action.Invoke();
        Write(DoubleQuote);
        return this;
    }
    
    #endregion
}