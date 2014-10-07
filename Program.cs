using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReadMarkdownParser
{
    class Program
    {
        static void Main(string[] args)
        {
            int fileCount = 0;
            try
            {

                string postDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_posts");
                if (Directory.Exists(postDir))
                    Directory.Delete(postDir, true);
                Directory.CreateDirectory(postDir);

                string pattern = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Read.regex"));
                Regex regex = new Regex(pattern);
                string input = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Read.md"));
                foreach (Match match in regex.Matches(input))
                {
                    for (int i = 0; i < match.Groups["Read"].Captures.Count; i++)
                    {
                        try
                        {
                            var read = new Read();
                            read.Title = match.Groups["Title"].Captures[i].Value;
                            read.Author = match.Groups["Author"].Captures[i].Value;
                            read.Finished = DateTime.Parse(match.Groups["Year"].Value
                                + "-"
                                + match.Groups["Month"].Captures[i].Value
                                + "-"
                                + match.Groups["Day"].Captures[i].Value);

                            string filePath =
                                string.Format("{0}-{1}.md",
                                read.Finished.ToString("yyyy-MM-dd"),
                                read.Title
                                    .Replace(" - ", "-")
                                    .Replace(":", "")
                                    .Replace(" ", "-")
                                );

                            string fileContents =
                                string.Format("---\nlayout: post\ntitle: {0}\nsubtitle: {1}\n---\n",
                                read.Title,
                                read.Author);

                            File.WriteAllText(Path.Combine(postDir, filePath), fileContents);
                            Console.WriteLine(filePath);
                            fileCount += 1;
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
                Console.WriteLine("{0} files created", fileCount);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
            Console.WriteLine("");
            Console.Write("Press any key...");
            Console.ReadKey();
        }
    }
}
