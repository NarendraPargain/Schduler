using System;
using System.IO;
using System.Threading;

namespace SchedulerJob.Model
{
    class CreateSchduledJob
    {
        static readonly string path = Resource.path;

        public static void schduler()
        {
            string getDate = DateTime.Now.ToShortDateString();

            /* This loop will run continuously. Intailaly it will create a File and write the current date and sleep upto midnight and triggered the mail.*/
            while (true)
            {
                String currentHour = DateTime.Now.ToString("HH");   // 24 Hour Format

                if (currentHour == "00")  //This value should be 00
                {
                    if (File.Exists(path))
                    {
                        updateFileContentWithCurrentDate(getDate);
                    }
                    else
                    {
                        createFileAddCurrentDate(getDate);

                    }
                }
                else
                {
                    Thread.Sleep(86400000);    // 10 min = 6,00,000 , will wake up after 24 hour
                }
            }
        }

        private static void updateFileContentWithCurrentDate(string date)
        {
            using (StreamReader fileReader = new StreamReader(path))
            {
                string readFirstLine = fileReader.ReadLine();
                if (readFirstLine != date)    // Validating if there is an entry for current date in log file.If not, then only mail will triggered and it will make an entry.
                {
                    fileReader.Dispose();           // Need to Dispose , Before opening the file again.
                    SendEmail.Processing();
                    File.WriteAllText(path, date);  // Replacing existing data with current date
                }

            }
        }
        private static void createFileAddCurrentDate(string date)
        {
            using (StreamWriter w = File.AppendText(path))
            {
                w.WriteLine(date);
            
            }
        }

    }
}
