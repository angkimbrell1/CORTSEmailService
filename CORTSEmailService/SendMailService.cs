using Corts.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using static Corts.Models.Classes;

namespace CORTSEmailService
{
    class SendMailService
    {    
        public static void WriteErrorLog(Exception ex)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + ex.Source.ToString().Trim() + "; " + ex.Message.ToString().Trim());
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }
        public static void WriteErrorLog(string Message)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + Message);
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }
        // This function contains the logic to send mail.    
        public static void SendEmail(List<string> userEmails, String Subj, string Message)
        {
            try
            {
                System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient();
                smtpClient.EnableSsl = true;
                smtpClient.Timeout = 200000;
                //Create a foreach method that creates whats below for each email: 
                for (int i = 0; i < userEmails.Count; i++)
                {
                    MailMessage MailMsg = new MailMessage();
                    System.Net.Mime.ContentType HTMLType = new System.Net.Mime.ContentType("text/html");
                    Dal dal = new Dal();
                    string message = null;
                    message = dal.EmailBody(userEmails[i]);
                    string strBody;
                    strBody = "Your cars breakdown: <br />" + message + "<br /><br /> If your mileage has changed, don't forget to login and update it!";
                    MailMsg.BodyEncoding = System.Text.Encoding.Default;
                    MailMsg.To.Add(userEmails[i]);
                    MailMsg.Priority = System.Net.Mail.MailPriority.High;
                    MailMsg.Subject = "Weekly Maintenance Update";
                    MailMsg.Body = strBody;
                    MailMsg.IsBodyHtml = true;
                    System.Net.Mail.AlternateView HTMLView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(strBody, HTMLType);

                    smtpClient.Send(MailMsg);
                    WriteErrorLog("Mail sent successfully!");
                }
                
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.InnerException.Message);
                throw;
            }
        }
    }
}
