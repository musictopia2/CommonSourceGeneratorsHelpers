namespace CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.HtmlParserClasses;
internal static class Utils
{
    public static bool WriteContentToFile(string path, string content)
    {
        bool to_ret = true;
        FileStream? fs = null;
        BinaryWriter? w = null;
        try
        {
            fs = new FileStream(path, FileMode.CreateNew);
            w = new BinaryWriter(fs);
            int i = 0;
            while (i < content.Length)
            {
                w.Write(Convert.ToInt32(content[i])); //maybe this will be okay.  i think could be the c# way of doing this part.
                i += 1;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("ParserUtils::WriteContentToFile(" + path + ") - got exception: [" + ex.Source + " - " + ex.Message + "]");
        }
        finally
        {
            if (!(w == null))
            {
                w.Close();
                w = null;
            }

            if (!(fs == null))
                fs.Close();
        }
        return to_ret;
    }
    public static string ReadContentFromFile(string path)
    {
        string to_ret = "";
        StreamReader sr;
        try
        {
            sr = new StreamReader(path);
            to_ret = sr.ReadToEnd();
        }
        catch (Exception ex)
        {
            Console.WriteLine("ParserUtils::ReadContentFromFile(" + path + ") - got exception: [" + ex.Source + " - " + ex.Message + "]");
        }
        finally
        {
        }
        return to_ret;
    }
    public static bool DeleteFile(string file_name)
    {
        try
        {
            if (FileExists(file_name))
            {
                File.Delete(file_name);
                return !FileExists(file_name);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("ParserUtils::DeleteFile(" + file_name + ") - got exception [" + ex.Source + ", " + ex.Message + "]");
        }

        return false;
    }
    public static bool FileExists(string file_name)
    {
        try
        {
            return File.Exists(file_name);
        }
        catch (Exception ex)
        {
            Console.WriteLine("ParserUtils::FileExists(" + file_name + ") - got exception [" + ex.Source + ", " + ex.Message + "]");
        }
        return false;
    }
}