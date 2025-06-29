using System.Runtime.InteropServices;
using System.Text;

public class IniFile
{
    private readonly string path;

    // Constructor to set the INI file path
    public IniFile(string iniPath)
    {
        path = iniPath;
    }

    // Windows API function to write data to the INI file
    [DllImport("kernel32", CharSet = CharSet.Unicode)]
    private static extern long WritePrivateProfileString(string section, string key, string value, string filePath);

    // Windows API function to read data from the INI file
    [DllImport("kernel32", CharSet = CharSet.Unicode)]
    private static extern int GetPrivateProfileString(
        string section,
        string key,
        string defaultValue,
        StringBuilder retVal,
        int size,
        string filePath);

    // Write data to a specific section and key
    public void Write(string section, string key, string value)
    {
        WritePrivateProfileString(section, key, value, path);
    }

    // Read data from a specific section and key
    public string Read(string section, string key, string defaultValue = "")
    {
        var retVal = new StringBuilder(255);
        GetPrivateProfileString(section, key, defaultValue, retVal, retVal.Capacity, path);
        return retVal.ToString();
    }
}
