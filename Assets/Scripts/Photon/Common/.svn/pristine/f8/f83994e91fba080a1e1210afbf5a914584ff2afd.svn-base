using Excel;
using RememberPos;
using System.Data;
using System.IO;

public class ExcelAccess
{
    public static string FilePath(string name)
    {
#if SERVER
        return "Resources/Excels/" + name;
#else
        return "Assets/Resources/Excels/" + name;
#endif
    }

    public static DataRowCollection GetCollection(string excelName)
    {
#if SERVER
        Singleton._log.Info("ExcelAccess file path=" + ExcelAccess.FilePath(excelName)); 
#endif
        FileStream stream = File.Open(ExcelAccess.FilePath(excelName), FileMode.Open, FileAccess.Read, FileShare.Read);
        IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream); 
        DataSet result = excelReader.AsDataSet();
        return result.Tables[0].Rows;
    }
}

