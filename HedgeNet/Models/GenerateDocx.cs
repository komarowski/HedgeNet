using System;
using System.Linq;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace HedgeNet.Models
{
    class GenerateDocx
    {
        public static void GenerateDocxFile(string path)
        {
            string fileName = path;

            // define title name
            DateTime thisDay = DateTime.Today;
            string title = "Passwords - " + thisDay.ToString("D");
            Formatting titleFormat = new Formatting();
            titleFormat.Size = 14;

            var doc = DocX.Create(fileName);
            Paragraph paragraphTitle = doc.InsertParagraph(title, false, titleFormat);
            paragraphTitle.Alignment = Alignment.center;

            var data = MainWindow.Passwords;
            Table t = doc.AddTable(data.Count, 4);

            t.Design = TableDesign.TableGrid;
            t.AutoFit = AutoFit.Contents;
            t.SetWidthsPercentage(new[] { 13f, 30f, 30f, 27f }, 480);

            Formatting cellFormat = new Formatting();
            cellFormat.Size = 12;

            for (int i = 0; i < data.Count; i++)
            {
                t.Rows[i].Cells[0].Paragraphs.First().Append(data[i].Website, cellFormat);
                t.Rows[i].Cells[1].Paragraphs.First().Append(data[i].Username, cellFormat);
                t.Rows[i].Cells[2].Paragraphs.First().Append(data[i].Email, cellFormat);
                t.Rows[i].Cells[3].Paragraphs.First().Append(data[i].Password, cellFormat);
            }

            doc.InsertParagraph("\n");
            doc.InsertTable(t);
            doc.Save();
        }
    }
}
