using System.Text;
namespace CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.Misc;
public class StrCat
{
    private StringBuilder _atoms = new();
    public void ClearString()
    {
        _atoms = new StringBuilder();
    }
    public void AddToString(string info, string delimiter = "")
    {
        if (_atoms.Length > 0)
        {
            _atoms.Append(delimiter);
        }
        _atoms.Append(info);
    }
    public string GetInfo()
    {
        return _atoms.ToString();
    }
}