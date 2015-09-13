using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW_TPL
{
    class Program
    {
        static void ReplaceInAllFiles(List<FileInfo> filesInfo, string etalon, string newValue)
        {
            foreach (FileInfo fileInfo in filesInfo)
            {
                string content = File.ReadAllText(fileInfo.FullName);
                content = content.Replace(etalon, newValue);
                File.WriteAllText(fileInfo.FullName, content);
            }
        }

        static List<FileInfo> ParallelFindFilesWhichContain(DirectoryInfo directory, string etalon)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            List<FileInfo> resultFilesInfo = new List<FileInfo>();
            FileInfo[] filesInfo = new FileInfo[0];

            try
            {
                filesInfo = directory.GetFiles("*.txt", SearchOption.AllDirectories);
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(String.Format("The directory with name {0} does not exist", directory.Name));
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(String.Format("You have no enough rights to search in {0}.", directory.FullName));
            }

            Parallel.ForEach(filesInfo, fileInfo =>
            {
               string content = File.ReadAllText(fileInfo.FullName);
               if (content.Contains(etalon))
               {
                   resultFilesInfo.Add(fileInfo);
               }
            });
            sw.Stop();
            Console.WriteLine(sw.Elapsed);

            return resultFilesInfo;
        }

        static List<FileInfo> FindFilesWhichContain(DirectoryInfo directory, string etalon)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            List<FileInfo> resultFilesInfo = new List<FileInfo>();
            FileInfo[] filesInfo = new FileInfo[0];

            try
            {
                filesInfo = directory.GetFiles("*.txt", SearchOption.AllDirectories);
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(String.Format("The directory with name {0} does not exist", directory.Name));
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(String.Format("You have no enough rights to search in {0}.", directory.FullName));
            }

            foreach (FileInfo fileInfo in filesInfo)
            {
                string content = File.ReadAllText(fileInfo.FullName);
                if (content.Contains(etalon))
                {
                    resultFilesInfo.Add(fileInfo);
                }
            }
            sw.Stop();
            Console.WriteLine(sw.Elapsed);

            return resultFilesInfo;
        }

        static void Main(string[] args)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(@"C:\Users\oksana\Desktop");
            List<FileInfo> filesInfo1 = FindFilesWhichContain(dirInfo, "a");
            List<FileInfo> filesInfo2 = ParallelFindFilesWhichContain(dirInfo, "a");

            foreach (FileInfo fileInfo in filesInfo1)
            {
                Console.WriteLine(fileInfo.FullName);
            }

            foreach (FileInfo fileInfo in filesInfo2)
            {
                Console.WriteLine(fileInfo.FullName);
            }
            Console.ReadKey();
        }
    }
}