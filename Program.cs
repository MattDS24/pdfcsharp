using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Text.RegularExpressions;

namespace PdfProperties
{
    class Program
    {
        static void Main(string[] args)
        {

         //load the file names to be processed
         string inputDir = "c:/Users/msc20/InvoiceInput/";
         string outputDir = "c:/Users/msc20/TextOutput/";

         string[] fileEntries = Directory.GetFiles(inputDir);
         foreach (string fileName in fileEntries)
         {
             string textfilename = System.IO.Path.GetFullPath(fileName);
       
            // create a reader (constructor overloaded for path to local file or URL)
            PdfReader reader
                = new PdfReader(textfilename);
            // total number of pages
            int n = reader.NumberOfPages;
            // size of the first page
            Rectangle psize = reader.GetPageSize(1);
            float width = psize.Width;
            float height = psize.Height;
       //     Console.WriteLine("Size of page 1 of {0} => {1} × {2}", n, width, height);
            // file properties
            Dictionary<string, string> infodict = reader.Info;
            theReader myText = new theReader(); 
          
            string myTextResult =myText.ReadPdfFile(textfilename); 
         //   Console.WriteLine(myTextResult);
        //  foreach (KeyValuePair<string, string> kvp in infodict)
          //        Console.WriteLine(kvp.Key + " => " + kvp.Value);


      //    System.IO.StreamWriter  myTextWriter = new System.IO.StreamWriter(System.IO.Path.GetFullPath(fileName) + ".txt");
      //     myTextWriter.WriteLine(myTextResult);
      
      System.IO.File.WriteAllText((outputDir + System.IO.Path.GetFileName(fileName) + ".txt"),myTextResult);


      //This bit is the contract number reader - parse it here from the text file and then call the T1 web service
      var conRegex = new Regex("^[C]\\d\\d\\d\\d\\d$"); 
      var invRegex = new Regex("(?<=Invoice.*?)\\d+"); 
      int conLoopCount = 0; 
      int invLoopCount = 0; 

      foreach (var line in File.ReadLines((outputDir + System.IO.Path.GetFileName(fileName) + ".txt")))
      {
        if((conRegex.IsMatch(line)) & (conLoopCount < 1))
        {
           Console.WriteLine("The file called " + System.IO.Path.GetFileName(fileName) 
           + " contains this contract number: " + conRegex.Match(line).Value);
           conLoopCount ++;
        };
         if((invRegex.IsMatch(line)) & (invLoopCount < 1))
        {
           Console.WriteLine("The file called " + System.IO.Path.GetFileName(fileName) 
           + " contains this invoice number: " + invRegex.Match(line).Value);
           invLoopCount ++;
        };



      };
      conLoopCount = 0; 
      invLoopCount = 0; 






              }
        }

   }
   class theReader
   {
 public string ReadPdfFile(string fileName)
{
    StringBuilder text = new StringBuilder();
    if (File.Exists(fileName))
    {
        PdfReader pdfReader = new PdfReader(fileName);

        for (int page = 1; page <= pdfReader.NumberOfPages; page++)
        {
            ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
            string currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);
           

         //   currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
            text.Append(currentText);
        }
        pdfReader.Close();
    }
    return text.ToString();
}

     
}

}