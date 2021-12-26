using System.Text;
namespace CommonSourceGeneratorsHelpers;
public interface IWriter
{
    IWriter Write(object obj);
    IWriter AppendDoubleQuote(Action<IWriter> action);
}
public interface ICodeBlock
{
    ICodeBlock WriteLine(string text);
    ICodeBlock WriteLine(Action<IWriter> action);
    ICodeBlock WriteCodeBlock(Action<ICodeBlock> action, bool endSemi = false, bool alsoBlankLine = false);
    ICodeBlock AppendEmptyLine();
}
public class SourceCodeStringBuilder : IWriter, ICodeBlock
{
    private const string DoubleQuote = "\"";
    private const string _oneSpace = "    ";
    private const string _twoSpaces = "        ";
    private const string _threeSpaces = "            ";
    private const string _fourSpaces = "                ";
    private const string _fiveSpaces = "                    ";
    private const string _sixSpaces = "                        ";
    private const string _sevenSpaces = "                            ";
    private const string _eightSpaces = "                                ";
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
    public SourceCodeStringBuilder WriteLine(Action<IWriter> action)
    {
        Indent();
        action.Invoke(this); //so i can break apart what is being written.
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
            return;
        }
        if (_indentLevel == 7)
        {
            _builds.Append(_sevenSpaces);
            return;
        }
        if (_indentLevel == 8)
        {
            _builds.Append(_eightSpaces);
            return;
        }
        throw new Exception("Only 8 spaces supported at the most");
    }
    private SourceCodeStringBuilder Write(object obj)
    {
        _needsNewLine = true;
        _builds.Append(obj);
        return this;
    }
    IWriter IWriter.Write(object obj)
    {
        return Write(obj);
    }
    public override string ToString()
    {
        return _builds.ToString();
    }
    #region Custom Stuff
    public SourceCodeStringBuilder WriteCodeBlock(Action<ICodeBlock> action, bool endSemi = false, bool alsoBlankLine = false)
    {
        StartBlock();
        action.Invoke(this);
        if (endSemi == false)
        {
            EndBlock(alsoBlankLine);
            return this;
        }
        DecreaseIndent().EnsureEmptyLine();
        Indent();
        Write("};"); //end semi means to also have a blank line.
        _builds.AppendLine();
        return this;
    }
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
    public SourceCodeStringBuilder AppendDoubleQuote(Action<SourceCodeStringBuilder> action)
    {
        Write(DoubleQuote);
        action.Invoke(this);
        Write(DoubleQuote);
        return this;
    }
    ICodeBlock ICodeBlock.WriteLine(string text)
    {
        return WriteLine(text);
    }

    ICodeBlock ICodeBlock.WriteLine(Action<IWriter> action)
    {
        return WriteLine(action);
    }
    ICodeBlock ICodeBlock.WriteCodeBlock(Action<ICodeBlock> action, bool endSemi, bool alsoBlankLine)
    {
        return WriteCodeBlock(action, endSemi, alsoBlankLine);
    }
    public SourceCodeStringBuilder AppendEmptyLine()
    {
        return WriteLine("");
    }
    ICodeBlock ICodeBlock.AppendEmptyLine()
    {
        return AppendEmptyLine();
    }
    IWriter IWriter.AppendDoubleQuote(Action<IWriter> action)
    {
        return AppendDoubleQuote(action);
    }
    #endregion
}