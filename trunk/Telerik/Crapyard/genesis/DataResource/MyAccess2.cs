
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using DataResource.DesignPatterns;
using DataResource.LinQToSqlServer;
using ExceptionHandler;
using Maintenance;
using MyDataConsumer;
using DataResource;
using MathNet.Numerics.Distributions;

public class MyAccess2 : IDataConsumer
{
    /// <summary>
    /// NOTE: not threadsafe
    /// </summary>
    private static IDictionary<string, double[]> m_OverschrijdingsKansenPerLocatie = new Dictionary<string, double[]>();

    public static IDictionary<string, double[]> OverschrijdingsKansenPerLocatie
    {
        get { return MyAccess2.m_OverschrijdingsKansenPerLocatie; }
        set { MyAccess2.m_OverschrijdingsKansenPerLocatie = value; }
    }

    #region IDataConsumer Members

    public static int ProjectId { get; private set; }

    public static Project Project { get; private set; }
    /// <summary>
    /// The path that was created by the last GetPathBronBestanden or CreateHierarchicalDatabase call.
    /// </summary>
    public string ProjectPath { get; private set; }

    /// <summary>
    /// The files for input. Collection is made after GetPathBronBestanden or CreateHierarchicalDatabase.
    /// </summary>
    public CollectionBronPaden BronBestanden { get; private set; }
    /// <summary>
    /// Creates the (hierarchical) database for this locatie on the filesystem.
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="root"></param>
    /// <param name="sof"></param>
    /// <param name="saf"></param>
    /// <returns></returns>
    public CollectionBronPaden CreateHierarchicalDatabase(int projectId, string root, bool sof, bool saf)
    {
        return HandleBronPadenEnBestanden(projectId, root, sof, saf, true);
    }
    /// <summary>
    /// Returns a collection of all bronbestanden
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="root"></param>
    /// <param name="sof"></param>
    /// <param name="saf"></param>
    /// <returns></returns>
    public CollectionBronPaden GetPathBronBestanden(int projectId, string root, bool sof, bool saf)
    {
        return HandleBronPadenEnBestanden(projectId, root, sof, saf, false);
    }
    /// <summary>
    /// Returns the path whilst creating files or not. Bronpaden are stored.
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="root"></param>
    /// <param name="sof"></param>
    /// <param name="saf"></param>
    /// <param name="createFiles"></param>
    /// <returns></returns>
    private CollectionBronPaden HandleBronPadenEnBestanden(int projectId, string root,
        bool sof, bool saf,
        bool createFiles)
    {
        ProjectId = projectId;
        ///NOTE: caller must anticipate exception.
        Collection<int> gegevensSetIds;
        var gegevensSoorten = new Dictionary<int, int>();
        Bronpaden = new Collection<string>();

        using (DataClasses1DataContext context = new DataClasses1DataContext())
        {
            gegevensSetIds = GetGegevensSetIds(context, projectId);
            gegevensSoorten = GetGegevensSoort(context, gegevensSetIds);

            if (Directory.Exists(root))
            {
                string bronBestanden = root + "\\BronBestanden";
                string resultaten = root + "\\Resultaten";
                string projectFolder = resultaten + "\\ProjectId_" + projectId;
                ProjectPath = projectFolder;

                if (createFiles)
                {
                    Directory.CreateDirectory(bronBestanden);
                    Directory.CreateDirectory(resultaten);
                    Directory.CreateDirectory(projectFolder);
                }
                foreach (var gegevensSoort in gegevensSoorten)
                {
                    string gegevensSoortDir = bronBestanden + "\\GegevensSoortId_" + gegevensSoort.Key;
                    string gegevensSetDir = gegevensSoortDir + "\\GegevensSetId_" + gegevensSoort.Value;
                    if (createFiles)
                    {
                        Directory.CreateDirectory(gegevensSoortDir);
                        Debug.Assert(Directory.Exists(gegevensSoortDir));
                        Directory.CreateDirectory(gegevensSetDir);
                        Debug.Assert(Directory.Exists(gegevensSetDir));
                    }
                    Bronpaden.Add(gegevensSetDir);
                }
            }

            if (DeploymentDirectory != null) HandleBronBestanden(sof, saf, Bronpaden, createFiles);

        }
        return BronBestanden;
    }

    protected Collection<string> Bronpaden { get; set; }


    private static Dictionary<int, int> GetGegevensSoort(DataClasses1DataContext context, Collection<int> gegevensSetIds)
    {
        var gegevensSoorten = new Dictionary<int, int>();
        var res = context.GegevensSets.Where(
            x => x.GegevensSoortId == 1 && x.GegevensSetId == gegevensSetIds[0]);
        int gegevensSoort1 = context.GegevensSets.Where(
            x => x.GegevensSoortId == 1 && x.GegevensSetId == gegevensSetIds[0]).First().GegevensSoortId;
        int gegevensSoort2 = context.GegevensSets.Where(
            x => x.GegevensSoortId == 2 && x.GegevensSetId == gegevensSetIds[1]).First().GegevensSoortId;
        int gegevensSoort3 = context.GegevensSets.Where(
            x => x.GegevensSoortId == 3 && x.GegevensSetId == gegevensSetIds[2]).First().GegevensSoortId;
        int gegevensSoort4 = context.GegevensSets.Where(
            x => x.GegevensSoortId == 4 && x.GegevensSetId == gegevensSetIds[3]).First().GegevensSoortId;

        gegevensSoorten.Add(gegevensSoort1, gegevensSetIds[0]);
        gegevensSoorten.Add(gegevensSoort2, gegevensSetIds[1]);
        gegevensSoorten.Add(gegevensSoort3, gegevensSetIds[2]);
        gegevensSoorten.Add(gegevensSoort4, gegevensSetIds[3]);

        return gegevensSoorten;
    }

    public static Collection<int> GetGegevensSetIds(DataClasses1DataContext context, int projectId)
    {
        var gegevensSetIds = new Collection<int>();

        var projects =
            from project in context.Projects
            where project.ProjectId == projectId
            select new
            {
                project.ImplicGegevensSetId,
                project.FaalkansGegevensSetId,
                project.TNOGegevensSetId,
                project.ToetspeilGegevensSetId
            };
        var curProject =
            (from project in context.Projects
             where project.ProjectId == projectId
             select project).FirstOrDefault();

        Project = curProject;
        int? implicGegevensSetId = projects.FirstOrDefault().ImplicGegevensSetId;//gegevenssoort 1
        int? faalkansGegevensSetId = projects.FirstOrDefault().FaalkansGegevensSetId;//gegevenssoort 2
        int? tnoGegevensSetId = projects.FirstOrDefault().TNOGegevensSetId;//gegevenssoort 3
        int? toetspeilGegevensSetId = projects.FirstOrDefault().ToetspeilGegevensSetId;//gegevenssoort 4

        gegevensSetIds.Add((int)implicGegevensSetId);
        gegevensSetIds.Add((int)faalkansGegevensSetId);
        gegevensSetIds.Add((int)tnoGegevensSetId);
        gegevensSetIds.Add((int)toetspeilGegevensSetId);


        return gegevensSetIds;
    }

    /// <summary>
    /// Gets bronbestanden via gegegevenssetid(implic) and projectid.
    /// </summary>
    /// <param name="projectId"></param>
    /// <returns></returns>
    public Collection<string> GetBronBestanden(int projectId)
    {
        ///NOTE: caller must anticipate exception.
        var bronbestanden = new Collection<string>();
        using (DataClasses1DataContext context = new DataClasses1DataContext())
        {

            var gegegevenSetId =
                (from project in context.Projects
                 where project.ProjectId == projectId
                 select project).FirstOrDefault().ImplicGegevensSetId;
            var bronbestandenLinQ = (from bronBestand in context.BronBestands
                                     where bronBestand.GegevensSetId == gegegevenSetId
                                     select bronBestand.BestandsNaam);
            ///Note: do not export List<T>.
            foreach (var bestand in bronbestandenLinQ)
            {
                bronbestanden.Add(bestand);
            }
        }
        return bronbestanden;
    }

    private void HandleBronBestanden(bool sof, bool saf, Collection<string> bronPaden, bool createFiles)
    {
        BronBestanden = new CollectionBronPaden();

        Debug.Assert(DeploymentDirectory != null);
        string datadirDirSource = DeploymentDirectory + "\\datadir";
        string dirTarget = "";
        int i = 0;
        string shortName;
        string sourceFileName;
        string destFileName;

        foreach (var path in bronPaden)
        {

            if (Directory.Exists(path))
            {
                switch (i)
                {
                    case 0:
                        #region copy implic files

                        dirTarget = path;//implic-dir
                        ImplicDirectory = path;

                        if (createFiles)
                        {
                            string[] listDatadir = Directory.GetFiles(datadirDirSource);
                            //listDatadir.Dump();

                            int countFiles = listDatadir.Length;

                            for (int j = 0; j < countFiles; j++)
                            {
                                bool toBeCopied = true;

                                shortName = Path.GetFileName(listDatadir[j]);
                                bool isSof = shortName.ToLower(CultureInfo.InvariantCulture).StartsWith("sof");
                                bool isSaf = shortName.ToLower(CultureInfo.InvariantCulture).StartsWith("saf");
                                if (isSof && sof) { /*always true*/} else toBeCopied = false;
                                if (isSaf && saf) toBeCopied = true; ///else copy always.
                                if (!isSaf && !isSof) toBeCopied = true; ///else copy always.
                                ///
                                ///Log.ConsoleWriteline(shortName);
                                sourceFileName = datadirDirSource + @"\" + shortName;
                                destFileName = dirTarget + @"\" + shortName;
                                if (createFiles && File.Exists(destFileName))
                                    File.Delete(destFileName);

                                if (toBeCopied && createFiles)
                                {
                                    File.Copy(sourceFileName, destFileName);
                                }
                                BronBestanden.Add(new DataResource.DesignPatterns.BronBestand
                                {
                                    Naam = shortName,
                                    Pad = destFileName
                                });
                                //TODO: InitData.UniqueInstance.ImplicNamen.Add(shortName);//

                            }
                        }
                        #endregion
                        break;
                    case 1:
                        dirTarget = path;//Faalkansen.
                        FaalkansDirectory = path;

                        if (createFiles)
                        {
                            shortName = "Scenariokansen.txt";
                            CopyBronFile(shortName, dirTarget);
                            BronBestanden.Add(new DataResource.DesignPatterns.BronBestand
                            {
                                Naam = shortName,
                                Pad = path
                            });
                        }
                        break;
                    case 2:
                        dirTarget = path;//tno.
                        TnoDirectory = path;

                        if (createFiles)
                        {
                            shortName = "TNOkansen.txt";
                            CopyBronFile(shortName, dirTarget);
                            BronBestanden.Add(new DataResource.DesignPatterns.BronBestand
                            {
                                Naam = shortName,
                                Pad = path
                            });
                            shortName = "WenWTzV.dat";
                            CopyBronFile(shortName, dirTarget);
                            BronBestanden.Add(new DataResource.DesignPatterns.BronBestand
                            {
                                Naam = shortName,
                                Pad = path
                            });
                        }
                        break;
                    case 3:
                        dirTarget = path;//toetspeil.
                        ToetsPeilenDirectory = path;

                        if (createFiles)
                        {
                            shortName = "Toetspeilen.txt";
                            CopyBronFile(shortName, dirTarget);
                            BronBestanden.Add(new DataResource.DesignPatterns.BronBestand
                            {
                                Naam = shortName,
                                Pad = path
                            });
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }

            }

            i++;
        }

        ///Copy results
        CopyResults();
    }

    private void CopyResults()
    {
        string dirTarget = ProjectPath;
        string shortName = "overschrijding_Roompot Binnen.dat";
        CopyBronFile(shortName, dirTarget);

        dirTarget = ProjectPath;
        shortName = "overschrijding_Roompot Buiten.dat";
        CopyBronFile(shortName, dirTarget);

        dirTarget = ProjectPath;
        shortName = "prestatiepeil.dat";
        CopyBronFile(shortName, dirTarget);

        dirTarget = ProjectPath;
        shortName = "projectnaam.dat";
        CopyBronFile(shortName, dirTarget);
    }

    private void CopyBronFile(string shortName, string dirTarget)
    {
        try
        {
            if (InitData.UniqueInstance.InSyncMode)
            {
                string destFileName;
                string sourceFileName;
                sourceFileName = DeploymentDirectory + @"\" + shortName;

                destFileName = dirTarget + @"\" + shortName;
                if (File.Exists(destFileName))
                    File.Delete(destFileName);
                File.Copy(sourceFileName, destFileName);
                Debug.Assert(File.Exists(sourceFileName));
                Debug.Assert(File.Exists(destFileName));
            }
        }
        catch (Exception)
        {
            ///TODO: maak nog een caller zonder deze aanroep.
            //Niets doen. In productie wordt er niets gekopieerd.
            DataResource.Maintenance.Log.Writeline("Negeer de functie CopyBronFile.");
        }

    }
    public bool SaveBronBestand(int gegevensetId, string naam)
    {
        ///NOTE: caller must anticipate exception.
        var bronBestand = new DataResource.LinQToSqlServer.BronBestand();
        bronBestand.BestandsNaam = naam;
        bronBestand.GegevensSetId = gegevensetId;
        bronBestand.AanleverDatum = DateTime.Now;

        using (DataClasses1DataContext context = new DataClasses1DataContext())
        {
            context.BronBestands.InsertOnSubmit(bronBestand);
            context.SubmitChanges();
        }
        return true;
    }

    int IDataConsumer.SaveToDatFile(string datFile, double[][] matrix, int rows, int colums, string format, int countSpacesPostFix)
    {
        return SaveToDatFile(datFile, matrix, rows, colums, format, countSpacesPostFix);
    }

    public bool Open(string fileNaam)
    {
        throw new System.NotImplementedException();
    }

    public string ReadLine()
    {
        throw new NotImplementedException();
    }


    public void Dispose()
    {
        throw new System.NotImplementedException();
    }

    public bool Close()
    {
        throw new System.NotImplementedException();
    }

    public string FileName
    {
        get;
        set;
    }
    public static Collection<string> ReadFile(string filename, bool headerUsed)
    {
        bool isEof = false;
        int lineCount = 1;
        int aantalGelezenRegels = 0;
        int kopRegels = 0;
        Collection<string> fileReference = new Collection<string>();
        kopRegels = GetKopRegels(filename);
        using (TextReader readFile = new StreamReader(filename))
        {
            string line = "";

            line = readFile.ReadLine();
            while (kopRegels > 3 && lineCount <= kopRegels)
            {
                line = readFile.ReadLine();
                lineCount++;
            }
            ///isEof = false;
            if (!string.IsNullOrEmpty(line) && line.Length > 1)
            {
                //Debug.Assert(lineCount == 57 || lineCount==1);
                //Debug.Assert(line.ToLowerInvariant().StartsWith(implic.ToLowerInvariant()), 
                //    "Regel begint niet met een implic aanduiding.");

                while (!isEof)
                {
                    if (!string.IsNullOrEmpty(line) && line.Length > 0)
                    {
                        fileReference.Add(line);
                    }
                    else
                        isEof = true;
                    line = readFile.ReadLine();
                }
            }
        }
        return fileReference;
    }

    public static int GetKopRegels(string filename)
    {
        int kopRegels;
        int aantalGelezenRegels;
        using (TextReader readFile = new StreamReader(filename))
        {
            string regel = "";
            int jaar = 1900;
            int maand = 0;
            int dag = 0;
            kopRegels = 0;
            int kolomRegels = 0;
            int regelTeller = 0;
            try
            {
                aantalGelezenRegels = LeesDatumHeaderKolomAantal(ref regelTeller, readFile,
                                                                 out jaar, out maand, out dag,
                                                                 out kopRegels, out kolomRegels);
            }
            catch (Exception)
            {

            }
            finally
            {

            }
        }
        return kopRegels;
    }

    public static int LeesDatumHeaderKolomAantal(ref int regelTeller, TextReader streamReader,
                                            out int jaar, out int maand, out int dag,
                                            out int kopRegels, out int kolomRegels)
    {
        // 20091022 = datum berekening
        string regel = LeesVolgendeRegel(ref regelTeller, streamReader);
        //int jaar = 1900;
        if (!int.TryParse(regel.Substring(0, 4), out jaar))
        {
            //m_Logging.LogFoutRegel(regelTeller, "Jaar is geen getal.");
        }
        //int maand = 0;
        if (!int.TryParse(regel.Substring(4, 2), out maand))
        {
            //m_Logging.LogFoutRegel(regelTeller, "Maand is geen getal.");
        }
        //int dag = 0;
        if (!int.TryParse(regel.Substring(6, 2), out dag))
        {
            //m_Logging.LogFoutRegel(regelTeller, "Dag is geen getal.");
        }
        if (regel.Substring(8).TrimEnd() != " = datum berekening")
        {
            //m_Logging.LogWarningRegel(regelTeller, "Tekst wijkt af - {0}", " = datum berekening");
        }
        // Tweede regel
        // 57 = Totaal aantal tekstregels in de kop van de file
        regel = LeesVolgendeRegel(ref regelTeller, streamReader);
        //int kopRegels = 0;
        if (!int.TryParse(regel.Substring(0, 2), out kopRegels))
        {
            //m_Logging.LogFoutRegel(regelTeller, "Aantal kopregels is geen getal.");
        }
        if (regel.Substring(2).TrimEnd() != " = Totaal aantal tekstregels in de kop van de file")
        {
            //m_Logging.LogWarningRegel(regelTeller, "Tekst wijkt af - {0}", " = Totaal aantal tekstregels in de kop van de file");
        }
        // Derde regel
        // 34 = Aantal kolommen in deze file met per kolom een regel verklarende tekst
        regel = LeesVolgendeRegel(ref regelTeller, streamReader);
        //int kolomRegels = 0;
        if (!int.TryParse(regel.Substring(0, 2), out kolomRegels))
        {
            //m_Logging.LogFoutRegel(regelTeller, "Aantal kolomregels is geen getal.");
        }
        if (regel.Substring(2).TrimEnd() != " = Aantal kolommen in deze file met per kolom een regel verklarende tekst")
        {
            //m_Logging.LogWarningRegel(regelTeller, "Tekst wijkt af - {0}", " = Aantal kolommen in deze file met per kolom een regel verklarende tekst");
        }

        // En we retourneren het aantal gelezen headerregels
        // Dit moet overeenstemmen met het aantal gelezen regels...
        return 3;
    }
    public static string LeesVolgendeRegel(ref int regelTeller, TextReader streamReader)
    {
        regelTeller++;
        return streamReader.ReadLine();
    }
    public static Collection<string> ReadFile(string filename)
    {
        //bool isEof = false;
        //Collection<string> fileReference = new Collection<string>();
        //using (TextReader readFile = new StreamReader(filename))
        //{
        //    string line;

        //    while (!isEof)
        //    {
        //        line = readFile.ReadLine();

        //        if (!string.IsNullOrEmpty(line) && line.Length > 1)
        //        {
        //            fileReference.Add(line);
        //        }
        //        else
        //            isEof = true;
        //    }
        //}
        return ReadFile(filename, false);
    }

    Collection<string> IDataConsumer.ReadFile(string filename)
    {
        return ReadFile(filename, false);
    }

    public string ResultDir
    {
        get;
        set;
    }

    public string OutputDir
    {
        get;
        set;
    }

    public string DeploymentDirectory
    {
        get;
        set;
    }

    public string RootDirectory
    {
        get;
        set;
    }

    public string ImplicDirectory
    {
        get;
        set;
    }

    public string FaalkansDirectory
    {
        get;
        set;
    }

    public string TnoDirectory
    {
        get;
        set;
    }

    public string ToetsPeilenDirectory
    {
        get;
        set;
    }
    /// <summary>
    /// Saves the matrix to a datFile. countSpacesPostFix indicates the number of spaces 
    /// between the values per line.
    /// </summary>
    /// <param name="datFile"></param>
    /// <param name="matrix"></param>
    /// <param name="rows"></param>
    /// <param name="colums"></param>
    /// <param name="format"></param>
    /// <param name="countSpacesPostFix"></param>
    /// <returns></returns>
    public static int SaveToDatFile(string datFile, double[][] matrix, int rows, int colums, string format,
        int countSpacesPostFix)
    {
        int lineCount = 0;
        try
        {
            using (StreamWriter sw = new StreamWriter(datFile))
            {

                for (int i = 0; i < rows; i++)
                {
                    string line = "";
                    lineCount++;
                    line = (i + 1).ToString().PrefixThisWithSpaces(1);
                    line = line.PostfixThisWithSpaces(countSpacesPostFix);
                    for (int j = 0; j < colums; j++)
                    {
                        double value = matrix[i][j];
                        string svalue = string.Format(CultureInfo.InvariantCulture, format, value);
                        line += string.Format(CultureInfo.InvariantCulture, "{0}", svalue);
                        line = line.PostfixThisWithSpaces(countSpacesPostFix);
                    }
                    ///File.AppendAllText(datFile, line.Trim());
                    sw.WriteLine(line.Trim());
                }
            }
        }
        catch (CheckedException ex)
        {
            throw new CheckedException(ErrorType.ProcessFailure, "Saving file failed:\n\r:" + ex.Message);
        }
        return lineCount;
    }
    /// <summary>
    /// format = collection of string formats (Precisions),  countPostFixes is the amount of seperators as postfixes.
    /// </summary>
    /// <param name="datFile"></param>
    /// <param name="matrix"></param>
    /// <param name="rows"></param>
    /// <param name="colums"></param>
    /// <param name="format"></param>
    /// <param name="countPostFixes"></param>
    /// <param name="seperator"></param>
    /// <returns></returns>
    public static int SaveToDatFile(string datFile, double[][] matrix, int rows,
        int colums, Collection<string> format, int countPostFixes, string seperator)
    {
        int lineCount = 0;
        try
        {
            using (StreamWriter sw = new StreamWriter(datFile))
            {

                for (int i = 0; i < rows; i++)
                {
                    string line = "";
                    lineCount++;
                    line = (i + 1).ToString().PrefixThisWithSpaces(1);
                    line = line.PostfixThisWithSeperators(countPostFixes, seperator);
                    for (int j = 0; j < colums; j++)
                    {
                        double value = matrix[i][j];
                        string svalue = string.Format(CultureInfo.InvariantCulture, format[j], value);
                        line += string.Format(CultureInfo.InvariantCulture, "{0}", svalue);
                        line = line.PostfixThisWithSeperators(countPostFixes, seperator);
                    }
                    ///File.AppendAllText(datFile, line.Trim());
                    sw.WriteLine(line.Trim());
                }
            }
        }
        catch (CheckedException ex)
        {
            throw new CheckedException(ErrorType.ProcessFailure, "Saving file failed:\n\r:" + ex.Message);
        }
        return lineCount;
    }
    public static int SaveToDatFile(string datFile, double[][] matrix, int rows, int colums, Collection<string> formats,
         string seperator)
    {
        int lineCount = 0;
        try
        {
            using (StreamWriter sw = new StreamWriter(datFile))
            {

                for (int i = 0; i < rows; i++)
                {
                    string line = "";
                    lineCount++;

                    for (int j = 0; j < colums; j++)
                    {
                        double value = matrix[i][j];
                        string svalue = string.Format(CultureInfo.InvariantCulture, formats[j], value);
                        line += string.Format(CultureInfo.InvariantCulture, "{0}", svalue);
                        line = line + seperator;
                    }
                    ///File.AppendAllText(datFile, line.Trim());
                    sw.WriteLine(line.Trim());
                }
            }
        }
        catch (CheckedException ex)
        {
            throw new CheckedException(ErrorType.ProcessFailure, "Saving file failed:\n\r:" + ex.Message);
        }
        return lineCount;
    }
    Collection<string> IDataConsumer.CreateHeader(int projectId, string title, string columnNames)
    {
        return CreateHeader(projectId, title, columnNames);
    }
    /// <summary>
    /// Creates a header for output files.
    /// </summary>
    /// <param name="projectId"></param>
    /// <returns></returns>

    #endregion

    #region Non-interface Implementation
    public static Collection<string> CreateHeader(int projectId, string title, string columnNames)
    {
        Collection<string> header = new Collection<string>();

        using (DataClasses1DataContext contextDb = new DataClasses1DataContext())
        {
            ///DeleteAllData(contextDb);
            var project = (from aProject in contextDb.Projects
                           where aProject.ProjectId == projectId
                           select aProject).FirstOrDefault();
            header.Add(title);
            header.Add("# Resultaten gegenereerd: " + project.BerekenDatum);
            header.Add("# Projectnaam: " + project.ProjectNaam);
            header.Add("# Projectomschrijving: " + project.ProjectOmschrijving);
            header.Add("# Soort project: " + (project.Experimenteel ? "Experimenteel" : "Niet Experimenteel"));
            header.Add("# SOF: " + (project.GegevensSet.SofAanwezig ? "Ja" : "Nee"));
            header.Add("# SAF: " + (project.GegevensSet.SafAanwezig ? "Ja" : "Nee"));
            header.Add("# Gebruikte invoersets: ");
            header.Add("# IMPLIC: " + project.GegevensSet.GegevensSetNaam);
            header.Add("# Faalkansen:  " + project.GegevensSet1.GegevensSetNaam);
            header.Add("# TNO: " + project.GegevensSet2.GegevensSetNaam);
            header.Add("# Toetspeilen: " + project.GegevensSet3.GegevensSetNaam);
            header.Add("# ");//lege regel
            header.Add(columnNames);

        }
        return header;
    }
    /// <summary>
    /// Creates the  toetspeilen en prestatiepeilen per locatienamen.
    ///# Toetspeilen en Prestatiepeilen per locatie in cm t.o.v. NAP
    ///# Resultaten gegenereerd: 20091118 13:34
    ///# Projectnaam: Test23
    ///# Projectomschrijving: Test van de nieuwe versie 0.4 van RETPOK
    ///# Soort locatie: Experimenteel
    ///# SOF: Nee
    ///# SAF: Ja
    ///# Gebruikte invoersets:
    ///# IMPLIC: implic_SAF_20091103
    ///# Faalkansen:  Faalkansen-01
    ///# TNO: TNO-06
    ///# Toetspeilen: Toetspeilen_20090101 
    ///#
    ///Locatie             Toetspeil Prestatiepeil Verschil
    ///Roompot Binnen      350       316           34
    ///Burghsluis          350       317           33
    ///Zeelandbrug Noord   350       325           25
    ///…
    ///…
    ///Marollegat          400       382           18
    /// </summary>
    public static void CreateToetspeilenEnPrestatiepeilen(IEnumerable prestatiePeilen)
    {
        using (DataClasses1DataContext context = new DataClasses1DataContext())
        {
            var project = (from aProject in context.Projects
                           where aProject.ProjectId == ProjectId
                           select aProject).FirstOrDefault();


        }
        ///Create subdir            
        string subDir = InitData.UniqueInstance.OutputDir;
        if (!Directory.Exists(subDir))
        {
            Directory.CreateDirectory(subDir);
        }
        string prefix = Project.ProjectNaam;
        string datFile = String.Format("{0}\\{1}.dat", subDir, prefix);
        if (File.Exists(datFile))
            File.Delete(datFile);

        CreateHeader(datFile, "# Toetspeilen en Prestatiepeilen per locatie in cm t.o.v. NAP",
            "Locatie\tToetspeil\tPrestatiepeil\tVerschil");
        foreach (KeyValuePair<string, double> pair in prestatiePeilen)
        {
            string locatieNaam = pair.Key;

            int index = InitData.UniqueInstance.GetToetsPeilIndex(locatieNaam);
            double toetspeil = InitData.UniqueInstance.Tpeilen[index];
            double prestatiepeil = pair.Value;
            double verschil = toetspeil - prestatiepeil;

            string line = string.Format("{0}\t{1}\t{2}\t{3}", locatieNaam, toetspeil, prestatiepeil, verschil);
            File.AppendAllText(datFile, line + "\n");
        }
    }

    /// <summary>
    ///# Prestatiepeil per locatie in cm t.o.v. NAP
    ///# Resultaten gegenereerd: 20091118 13:34
    ///# Projectnaam: Test23
    ///# Projectomschrijving: Test van de nieuwe versie 0.4 van RETPOK
    ///# Soort locatie: Experimenteel
    ///# SOF: Nee
    ///# SAF: Ja
    ///# Gebruikte invoersets:
    ///# IMPLIC: implic_SAF_20091103
    ///# Faalkansen:  Faalkansen-01
    ///# TNO: TNO-06
    ///# Toetspeilen: Toetspeilen_20090101 
    ///#
    ///Locatie           Prestatiepeil
    ///
    ///Roompot binnen    316        
    ///Burghsluis        317        
    ///Zeelandbrug Noord 325        
    ///Colijnsplaat      323        
    ///Stavenisse        345        
    ///…
    ///…
    ///Marollegat        382        
    /// </summary>
    public static void CreatePrestatiePeilenFile(IEnumerable prestatiePeilen)
    {
        ///Create subdir            
        string subDir = InitData.UniqueInstance.OutputDir;
        if (!Directory.Exists(subDir))
        {
            Directory.CreateDirectory(subDir);
        }
        string prefix = "prestatiepeil";
        string datFile = String.Format("{0}\\{1}.dat", subDir, prefix);
        if (File.Exists(datFile))
            File.Delete(datFile);

        CreateHeader(datFile, "# Prestatiepeil per locatie in cm t.o.v. NAP", "Locatie\tPrestatiepeil");
        foreach (KeyValuePair<string, double> pair in prestatiePeilen)
        {
            string line = string.Format("{0}\t{1}", pair.Key, pair.Value);
            File.AppendAllText(datFile, line + "\n");
        }
    }

    public static void CreateTerugKeerPeriode(IEnumerable prestatiePeilen)
    {
        ///Create subdir            
        string subDir = InitData.UniqueInstance.OutputDir;
        if (!Directory.Exists(subDir))
        {
            Directory.CreateDirectory(subDir);
        }
        string prefix = "terugkeerperiode";
        string datFile = String.Format("{0}\\{1}.dat", subDir, prefix);
        if (File.Exists(datFile))
            File.Delete(datFile);

        CreateHeader(datFile, "# Terugkeerperiode toetspeil overschrijdingP", "Locatie\tPrestatiepeil");
        foreach (KeyValuePair<string, double> pair in prestatiePeilen)
        {
            string locatieNaam = pair.Key;

            int index = InitData.UniqueInstance.GetToetsPeilIndex(locatieNaam);
            double[] overschrijdingsKansen;
            m_OverschrijdingsKansenPerLocatie.TryGetValue(locatieNaam, out overschrijdingsKansen);
            Debug.Assert(overschrijdingsKansen != null);
            //Tpeilen values staan voor waarden vanaf 1. Dus -1 optellen voor array's.
            int toetspeil = Convert.ToInt32(InitData.UniqueInstance.Tpeilen[index]);

            double prestatiepeil = overschrijdingsKansen[toetspeil - 1];
            double result = 1 / prestatiepeil;

            string spaces = "                                                       ";
            string newName = (locatieNaam + spaces).Substring(0, 29);
            string newResult = (string.Format("{0:F0}", result) + spaces).Substring(0, 6);
            string line = string.Format("{0}\t{1}", newName, newResult);

            if (result > 999999) line = string.Format("{0}\t{1:F0}", newName, 999999);

             DataResource.Maintenance.Log.Writeline(line + "\ttoetspeil:\t" + toetspeil);

            File.AppendAllText(datFile, line + "\n");
        }
    }
    /// <summary>
    ///# Overschrijdingskans per locatie per waterstand
    ///# Resultaten gegenereerd: 20091118 13:34
    ///# Projectnaam: Test23
    ///# Projectomschrijving: Test van de nieuwe versie 0.4 van RETPOK
    ///# Soort locatie: Experimenteel
    ///# SOF: Nee
    ///# SAF: Ja
    ///# Gebruikte invoersets:
    ///# IMPLIC: implic_SAF_20091103
    ///# Faalkansen:  Faalkansen-01
    ///# TNO: TNO-06
    ///# Toetspeilen: Toetspeilen_20090101 
    ///#
    ///Locatie         Waterstand Kans
    ///
    ///Roompot Buiten	      1        1,00
    ///Roompot Buiten	      2        1,00
    ///.
    ///.
    ///Roompot Buiten        406        4,61E-06
    ///Roompot Buiten        407        4,60E-06
    ///Roompot Buiten        408        4,59E-06
    ///Roompot Buiten        409        4,58E-06
    ///Roompot Buiten        410        4,57E-06
    ///Roompot Buiten        411        4,56E-06
    ///Roompot Buiten        412        4,55E-06
    ///.
    ///.
    ///Roompot Buiten        998        0,00
    ///Roompot Buiten        999        0,00
    ///Roompot Buiten       1000        0,00
    /// </summary>
    /// <param name="globals"></param>
    /// <param name="naamLocatie"></param>
    /// <param name="datFile"></param>
    public static void CreateOverschrijdingskansenFile(InitData globals, string naamLocatie, string datFile)
    {
        ///Collect for terugkeerperiode.
        m_OverschrijdingsKansenPerLocatie.Add(new KeyValuePair<string, double[]>(naamLocatie.Trim(),
            globals.Overschrijding));

        CreateHeader(datFile, "# Overschrijdingskans per locatie per waterstand", "Locatie\tWaterstand\tKans");
        for (int i = 0; i < 1000; i++)
        {
            //Debug.Assert(i < 228);
            double value = globals.Overschrijding[i];
            string sOverschrijding = string.Format(CultureInfo.InvariantCulture, "{0:E2}", value);//13->480, 15->21
            string line = string.Format(CultureInfo.InvariantCulture, "{0}\t{1}\t{2}\n",
                naamLocatie, (i + 1), sOverschrijding);
            File.AppendAllText(datFile, line);
        }
    }
    public static void CreateHeader(string datFile, string title, string columnNames)
    {

        Collection<string> lines = CreateHeader(InitData.UniqueInstance.ProjectId, title, columnNames);
        foreach (string line in lines)
        {
            File.AppendAllText(datFile, line + "\n");
        }

    }
    public IGraphInfo GetGraphInfo(int projectId, string locatie, string root)
    {

        DataGraph distribution = new DataGraph();
        DataPoint prestatiepeilPunt;
        DataPoint toetspeilPunt;
        try
        {
            // Get a random number generator
            Random rand = new Random();
            double x;
            double y;
            for (int i = 0; i < 1000; i++)
            {
                MathNet.Numerics.Distributions.NormalDistribution bin = new NormalDistribution(0, 1000);
                y = bin.CumulativeDistribution(i);

                distribution.Add(new DataPoint(i, y));
            }

            x = rand.NextDouble() * 20.0 + 1;
            y = Math.Log(10.0 * (x - 1.0) + 1.0) * (rand.NextDouble() * 0.2 + 0.9);
            prestatiepeilPunt = new DataPoint(x, y);

            x = rand.NextDouble() * 20.0 + 1;
            y = Math.Log(10.0 * (x - 1.0) + 1.0) * (rand.NextDouble() * 0.2 + 0.9);
            toetspeilPunt = new DataPoint(x, y);
        }
        catch (Exception ex)
        {
            throw new CheckedException(ErrorType.ParseFailure, ex.Message);
        }
        ///NOTE: met GraphInfoSimple(distribution, toetspeilPunt, prestatiepeilPunt) is loosely-coupling onmogelijk
        ///(assemblies scheiden zonder interfaces te kopieren in de de bussiness layer).
        ///Gebruik minimaal een pattern.
        IGraphInfo result = new Factory<MyAccess>().CreateGraphInfo();
        result.OverschrijdingsKansen = distribution;
        result.PrestatiePeil = prestatiepeilPunt;
        result.ToetsPeil = toetspeilPunt;
                
        return result;
    }

    private void RemoveHeader(Collection<string> fileRef, Collection<string> fileRefOverschrijdingsKansen)
    {
        int i = 0;
        foreach (string line in fileRef)
        {
            if (i > 13)
                fileRefOverschrijdingsKansen.Add(line);
            i++;
        }
    }


    #endregion




    public ICollection OverschrijdingsKansen
    {
        get { throw new NotImplementedException(); }
    }

    public IDataPointBase ToetsPeil
    {
        get { throw new NotImplementedException(); }
    }

    public IDataPointBase PrestatiePeil
    {
        get { throw new NotImplementedException(); }
    }
}