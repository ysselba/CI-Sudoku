namespace sudoku;

public class WriterCSV
{
    public List<String> Lines {get; set; }
    private string _ColNames;

    public WriterCSV()
    {
        Lines = new List<String>();
        _ColNames = "\"S\";\"Plateau\";\"TestFrequency\";\"AverageTime\";\"AverageCount\";\"AveragePlateauCount\";\"Sudoku\"";;
    }
    
    //location of csv file
    public string GetFilePath(String fileName)
    {
        string workingDirectory = Environment.CurrentDirectory;
        string root = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
        return Path.Combine(root, fileName);
    }

    public void addLine(int S, int plateau, int testFreq, TimeSpan gemTime, double gemCount, double gemPlat, string inputString)
    {
        Lines.Add($"{S};{plateau};{testFreq};\"{gemTime}\";{gemCount};{gemPlat};\"{inputString}\"");
    }

    //clear current csv and write new result
    public void UpdateCSV()
    {
        string path = GetFilePath("BenchmarkResults.csv");
        File.WriteAllText(path,string.Empty);
        TextWriter tw = new StreamWriter(path);
        tw.WriteLine(_ColNames);
        foreach (var line in Lines)
        {
            tw.WriteLine(line);
        }
        tw.Close();
    }
}