/*
 * 
 *  Grundlage diese Programms war der KB-Artikel: http://support.microsoft.com/kb/322091/en-us
 *  Ähnlicher Artikel für VB6: http://support.microsoft.com/kb/154078/en-us
 *  
 *  SimpleMapi Unterstützung: http://www.codeproject.com/KB/IP/simplemapidotnet.aspx
 * 
 * *
 * * Testcommandline: h:\d\grafik\hp.out "FRITZfax Drucker"
 * *                  C:\delapro\laser\hp.out "\\server\hp2300 EX"
 * 
 *    (C) 2009-2016 by easy - innovative software
 * */

using System;
using System.Text;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace DlpRawPr
{
    class Program
    {
        static void Main(string[] args)
        {
            bool run = true;
            bool showhelp = false;
            bool appendformfeed = false;
            bool useXPS = false;
            string parameter = "";

            if (args.Length == 0)
                showhelp = true;
            else
                if (args[0] == "/?" || args[0] == "-?")
                    showhelp = true;
                else
                {
                    if (args.Length == 1)
                    {
                        if (args[0].ToUpper() == @"/DRUCKERAUSWAHL")
                        {
                            ShowPrinterDrivers();
                            run = false;
                        }
                        parameter = "";
                    }
                    else
                    {
                        parameter = args[1];
                        // folgende Zeile wurde wieder rausgenommen, weil es im Zusammenhang mit dem Delapro und leeren
                        // Batchparametern %4 %5 %6 %7 usw. Probleme gab und diese obwohl leer als Parameter verstanden
                        // wurden
                        // if (args.Length > 3)
                        //    showhelp = true;
                    }

                    foreach (string para in args)
                	{
                        if (para.ToUpper () == "/FORMFEED")
                        {
                            appendformfeed = true;
                        }
                        if (para.ToUpper() == "/XPS")
                        {
                            useXPS = true;
                        }
                    }

            }

            if (!showhelp)
            {
                if (run)
                {
                    PrintRaw(args[0], parameter, appendformfeed, useXPS);
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                }
                
            }
            else
            {
                System.Reflection.AssemblyProductAttribute productAttribute;
                System.Reflection.AssemblyVersionAttribute versionAttribute;

                productAttribute = (System.Reflection.AssemblyProductAttribute)Attribute.GetCustomAttribute(System.Reflection.Assembly.GetExecutingAssembly(), typeof(System.Reflection.AssemblyProductAttribute));
                versionAttribute = (System.Reflection.AssemblyVersionAttribute) Attribute.GetCustomAttribute (System.Reflection.Assembly.GetExecutingAssembly (), typeof (System.Reflection.AssemblyVersionAttribute));

                //Console.WriteLine ("{0}",  System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);

                Console.WriteLine("Aufruf :");
                Console.WriteLine("{0} <Drucker-RawDatei> [<Windowsdruckertreiber>] [/FORMFEED] [/XPS]", Application.ProductName);
                Console.WriteLine("");
                Console.WriteLine("Version: {0}", Application.ProductVersion);
                Console.WriteLine("");
                Console.WriteLine("Erlaubt das Schreiben von Daten direkt in der vom Drucker verstandenen Sprache.");
                Console.WriteLine("Versteht ein Druckertreiber das verwendete Format nicht, kann es auch sein,");
                Console.WriteLine("dass die Ausgabe einfach verschluckt wird. (pdffactory, Fritzfax usw.)");
                Console.WriteLine("");
                Console.WriteLine("Bei V4-Druckertreiber unter Windows 8 und Windows 10 ist /XPS anzugeben, sonst");
                Console.WriteLine("gibt es keinen Ausdruck und es wird ein Fehler in der Ereignisanzeige erzeugt.");
                Console.WriteLine("");
                Console.WriteLine("Durch Aufruf mit dem Parameter /DRUCKERAUSWAHL werden die installierten Drucker");
                Console.WriteLine("angezeigt.");
            }
        }

        private static void ShowPrinterDrivers()
        {
            foreach (string printerName in PrinterSettings.InstalledPrinters)
            {
                Console.WriteLine ("{0}", printerName);
            }
        }

        private static void PrintRaw(string printerRawDataFile, string printerDriver, bool appendFormfeed, bool useXPS)
        {
            var ps = new PrinterSettings ();
            var encoding = Encoding.ASCII;

            if (printerDriver == String.Empty || printerDriver.ToUpper() == "UNBEKANNT" || printerDriver.ToUpper () == "STANDARD")
                printerDriver = ps.PrinterName;
            
            RawPrinterHelper.SendFileToPrinter(printerDriver, printerRawDataFile, appendFormfeed, useXPS);

            // Der Weg über den Streamreader sorgt trotz Encoding für verpuzzelte Umlaute:
            //var sr = new StreamReader(printerRawDataFile, System.Text.Encoding.GetEncoding(850)); // encoding);
            
            //RawPrinterHelper.SendStringToPrinter(printerDriver, sr.ReadToEnd(), appendFormfeed);
        }
    }
}
