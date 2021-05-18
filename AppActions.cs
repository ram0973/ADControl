using System.Text;
using System.Management.Automation;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Management.Automation.Runspaces;
using System.IO;
using System;

namespace ADcontrol
{
    class AppActions
    {
        public static void ConnectViaShadowRDP(string computerName) 
        {

            using PowerShell powerShell = PowerShell.Create();
            //powerShell.AddCommand(string.Format("pwsh qwinsta /server:{0}", computerName));
            //"&{ tclsh --arg1 arg1 --arg2 arg2 | Tee-Object C:\myoutfile.txt }"
            // Collection<PSObject> PSOutput = powerShell.Invoke();

            //Process.Start(@"pwsh");


            Process CMDprocess = new Process();
            ProcessStartInfo startProcessInfo = new ProcessStartInfo();
            startProcessInfo.FileName = "pwsh";
            startProcessInfo.Verb = "runas";

            string projectDir = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            string scriptPath = Path.Combine(projectDir, @"..\Scripts\GetSessionsList.ps1");
            
            startProcessInfo.Arguments = string.Format(@"-i {0} -ComputerName {1}", scriptPath, computerName);
            

            CMDprocess.StartInfo = startProcessInfo;
            
            CMDprocess.Start();
            CMDprocess.WaitForExit();
            CMDprocess.StandardOutput.ReadToEnd();
            //AppMenus.gridContextMenu.Items.Add(path);

            /*

            Runspace runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();

            RunspaceInvoke runSpaceInvoker = new RunspaceInvoke(runspace);
            runSpaceInvoker.Invoke("Set-ExecutionPolicy Unrestricted");

            // create a pipeline and feed it the script text
            Pipeline pipeline = runspace.CreatePipeline();
            Command command = new Command(SCRIPT_PATH);
            foreach (var file in filesToMerge)
            {
                command.Parameters.Add(null, file);
            }
            command.Parameters.Add(null, outputFilename);
            pipeline.Commands.Add(command);

            pipeline.Invoke();
            runspace.Close();
            */
        }
    }
}
