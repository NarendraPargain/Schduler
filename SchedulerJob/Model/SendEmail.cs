using System;
using System.Net.Mail;
using System.Configuration;
using System.Net.Configuration;
using System.Net;
using System.IO;
using System.Collections.Generic;


namespace SchedulerJob.Model
{
    enum Reminder
    {
        NoReminder = 0,
        FirstReminder,
        SeconReminder,
        ThirdReminder
    }

    class SendEmail
    {
        private static string PopulateBody(string path, string userName, string url, int reminderCount)
        {
            string pathToHTMLFile = path;
            string body = File.ReadAllText(pathToHTMLFile);

            using (FileStream fs = File.Open(pathToHTMLFile, FileMode.Open, FileAccess.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    body = sr.ReadToEnd();
                }
            }
            body = body.Replace("{UserName}", userName);
            if (url != null)
            {
                body = body.Replace("{url}", url);
            }
            if (reminderCount != 0)
            {
                body = body.Replace("{ReminderCount}", reminderCount + "");
            }

            return body;
        }

        public static void Processing()
        {
            string welcomePath = Resource.welcomePath;
            string reminderPath = Resource.reminderPath;
            string thanksPath = Resource.thanksPath;

            List<Employee> list = DataAccess.DataAccess.FetchData();

            foreach (var item in list)
            {
                string temp;
               
                if (item.RemainderCount > (int)Reminder.ThirdReminder) continue;

                 if (item.RemainderCount == (int)Reminder.NoReminder)      // item.IsLinkAccessed is needs not to be validated , It is handled in code.
                {
                        temp = PopulateBody(welcomePath, item.Name, Resource.URL+ item.Email, (int)Reminder.NoReminder);
                        SendingMail(item.Name, item.Email, temp);
                       
                    }
                    else
                    {
                        temp = PopulateBody(reminderPath, item.Name, Resource.URL + item.Email, item.RemainderCount);
                        SendingMail(item.Name, item.Email, temp);
                        
                    }
                    item.RemainderCount++;
                

                DataAccess.DataAccess.updateData(list);
                
            }


        }
        public static void SendingMail(string name, string emailID, string body)
        {
            
            SmtpSection section = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            string from = section.From;
            string targetName = section.Network.TargetName;
            string host = section.Network.Host;
            int port = section.Network.Port;
            bool enableSsl = section.Network.EnableSsl;
            string user = section.Network.UserName;
            string password = section.Network.Password;

            using (MailMessage emailMessage = new MailMessage())
            {
                emailMessage.From = new MailAddress(user, targetName);
                emailMessage.To.Add(new MailAddress(emailID, name));
                emailMessage.Subject = "Subscribe to New Channel";
                emailMessage.Body = body;
                emailMessage.IsBodyHtml = true;

                using (SmtpClient MailClient = new SmtpClient(host, port)) 
                {
                    MailClient.Credentials = new NetworkCredential(from, password);
                    MailClient.Send(emailMessage);
                    MailClient.Credentials = null;
                }
            }
            
        }
    }
}

