using dnlib.DotNet.Writer;
using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ICSharpCode.Decompiler.CSharp;
using System.Reflection;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.TypeSystem;
using ICSharpCode.NRefactory.CSharp;
using Mono.Cecil;
using System.Net.Http;
using System.Diagnostics;

namespace obfuscript
{
    public partial class MainFormNew : Form
    {

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleParam = base.CreateParams;
                handleParam.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED       
                return handleParam;
            }
        }

        public MainFormNew()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;
        }

        private void MainFormNew_Load(object sender, EventArgs e)
        {

            if (obfuscript.Properties.Settings.Default.FirstTime == true)
            {

                btnUseAI.Visible = false;
                lbUseAI.Visible = false;

                obfuscript.Properties.Settings.Default.FirstTime = false;
                obfuscript.Properties.Settings.Default.Save();
                ObfuscriptDialog dialog = new ObfuscriptDialog("Welcome to Obfuscript", @"Obfuscript is a powerful and easy-to-use obfuscation tool that
can protect your code from reverse engineering & decompilation.", ObfuscriptDialog.Types.KeepOpen);
                dialog.Show();

                pnFirstTime.BringToFront();
                pnFirstTime.Visible = true;

            }
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void Print(string text)
        {
            tbLogs.Text = tbLogs.Text + @"
" + text;
        }

        private string selectedFilePath;

        private void btnSelectPath_Click(object sender, EventArgs e)
        {

            btnSelectPath.Text = "Selecting...";

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Extension Files (DLL)|*.dll|Executable Files (EXE)|*.exe";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = "Select a protectable file.";


                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {

                    lbPath.Text = openFileDialog.SafeFileName;
                    selectedFilePath = openFileDialog.FileName;

                    Print($"File Imported >> [{openFileDialog.SafeFileName} ({openFileDialog.FileName})]");

                }
                else
                {

                    lbPath.Text = "No path selected.";

                }
            }

            btnSelectPath.Text = "Select path";

        }

        private void btnBuildProtection_Click(object sender, EventArgs e)
        {

            piBuilding.Start();
            piBuilding.Visible = true;

            btnBuildProtection.Text = "Building...";
            btnBuildProtection.Enabled = false;

            if (lbPath.Text == "No path selected.")
            {

                piBuilding.Stop();
                piBuilding.Visible = false;

                Print("You haven't selected a file to protect.");

                btnBuildProtection.Text = "Build your protected file";
                btnBuildProtection.Enabled = true;


            }
            else
            {

                bwBuildProtect.RunWorkerAsync();

            }

        }

        private void btnXStringEncryption_Click(object sender, EventArgs e)
        {
            if (btnXStringEncryption.Checked != true)
            {

                btnXStringEncryption.Checked = true;
                btnXStringEncryption.Text = "Deselect";

            }
            else
            {

                btnXStringEncryption.Checked = false;
                btnXStringEncryption.Text = "Select";

            }
        }

        private void btnXMetaDataKiller_Click(object sender, EventArgs e)
        {
            if (btnXMetaDataKiller.Checked != true)
            {

                btnXMetaDataKiller.Checked = true;
                btnXMetaDataKiller.Text = "Deselect";

            }
            else
            {

                btnXMetaDataKiller.Checked = false;
                btnXMetaDataKiller.Text = "Select";

            }
        }

        private void btnXDisguiser_Click(object sender, EventArgs e)
        {
            if (btnXDisguiser.Checked != true)
            {

                btnXDisguiser.Checked = true;
                btnXDisguiser.Text = "Deselect";

            }
            else
            {

                btnXDisguiser.Checked = false;
                btnXDisguiser.Text = "Select";

            }
        }

        private void btnXChanger_Click(object sender, EventArgs e)
        {
            if (btnXChanger.Checked != true)
            {

                btnXChanger.Checked = true;
                btnXChanger.Text = "Deselect";

            }
            else
            {

                btnXChanger.Checked = false;
                btnXChanger.Text = "Select";

            }
        }

        private void btnXBloatAndConfuse_Click(object sender, EventArgs e)
        {
            if (btnXBloatAndConfuse.Checked != true)
            {

                btnXBloatAndConfuse.Checked = true;
                btnXBloatAndConfuse.Text = "Deselect";

            }
            else
            {

                btnXBloatAndConfuse.Checked = false;
                btnXBloatAndConfuse.Text = "Select";

            }
        }

        private void btnXGarbageDisposal_Click(object sender, EventArgs e)
        {
            if (btnXGarbageDisposal.Checked != true)
            {

                btnXGarbageDisposal.Checked = true;
                btnXGarbageDisposal.Text = "Deselect";

            }
            else
            {

                btnXGarbageDisposal.Checked = false;
                btnXGarbageDisposal.Text = "Select";

            }
        }

        private void btnXNoDebug_Click(object sender, EventArgs e)
        {
            if (btnXNoDebug.Checked != true)
            {

                btnXNoDebug.Checked = true;
                btnXNoDebug.Text = "Deselect";

            }
            else
            {

                btnXNoDebug.Checked = false;
                btnXNoDebug.Text = "Select";

            }
        }

        private void btnXdnSpyKiller_Click(object sender, EventArgs e)
        {
            if (btnXdnSpyKiller.Checked != true)
            {

                btnXdnSpyKiller.Checked = true;
                btnXdnSpyKiller.Text = "Deselect";

            }
            else
            {

                btnXdnSpyKiller.Checked = false;
                btnXdnSpyKiller.Text = "Select";

            }
        }

        private void bwBuildProtect_DoWork(object sender, DoWorkEventArgs e)
        {

            Print(@"
Starting protection process, please wait.");
            Print(@"This may take up to 1-2 minute(s), please be patient and do not close the application.");

            ModuleContext modCtx = ModuleDef.CreateModuleContext();
            var module = ModuleDefMD.Load(selectedFilePath, modCtx);
            Print(@"
Context created, module loaded.");

            if (btnXStringEncryption.Checked)
            {
                StringEncryption.Execute(module);
            }
            if (btnXMetaDataKiller.Checked)
            {
                Utilities.LvlProt.MetaKiller.Execute(module.Assembly);
            }
            if (btnXDisguiser.Checked)
            {
                Utilities.LvlProt.RA1.Execute(module);
            }
            if (btnXChanger.Checked)
            {
                Utilities.LvlProt.Stringer.Execute(module);
                Utilities.LvlProt.Proxy.Execute(module);
            }
            if (btnXBloatAndConfuse.Checked)
            {
                Utilities.LvlProt.IntegerConfusion.Execute(module);
                Utilities.LvlProt.Confuse.Execute(module);
            }
            if (btnXGarbageDisposal.Checked)
            {
                Utilities.LvlProt.GarbageDisposal.Execute(module, module.Assembly);
            }
            if (btnXNoDebug.Checked)
            {
                Utilities.LvlProt.AntiDebugging.Execute(module);
            }
            if (btnXdnSpyKiller.Checked)
            {
                Utilities.LvlProt.DnSpyBlocker.Execute(module.Assembly);
            }

            Print("Building protected file..");

            if (selectedFilePath.EndsWith(".exe"))
            {
                var path = selectedFilePath.Remove(selectedFilePath.Length - 4) + ".obf.exe";
                module.Write(path, new ModuleWriterOptions(module) { Logger = DummyLogger.NoThrowInstance });
                Print(@"
Your file was successfully built and protected. >>" + path);
            }

            if (selectedFilePath.EndsWith(".dll"))
            {
                var path = selectedFilePath.Remove(selectedFilePath.Length - 4) + ".obf.dll";
                module.Write(path, new ModuleWriterOptions(module) { Logger = DummyLogger.NoThrowInstance });
                Print(@"
Your file was successfully built and protected. >>" + path);
            }

            piBuilding.Stop();
            piBuilding.Visible = false;

            btnBuildProtection.Text = "Build your protected file";
            btnBuildProtection.Enabled = true;

        }

        private void btnFTContinue_Click(object sender, EventArgs e)
        {
            pnFirstTime.Visible = false;
            //btnUseAI.Visible = true;
            //lbUseAI.Visible = true;
        }

        private void btnUseAI_Click(object sender, EventArgs e)
        {
            pnAI.BringToFront();
            pnAI.Visible = true;
        }

        private void lbUseAI_Click(object sender, EventArgs e)
        {
            pnAI.BringToFront();
            pnAI.Visible = true;
        }

        private void btnSelectPathAI_Click(object sender, EventArgs e)
        {
            btnSelectPath.Text = "Selecting...";
            btnSelectPathAI.Text = "Selecting...";

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Extension Files (DLL)|*.dll|Executable Files (EXE)|*.exe";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = "Select a protectable file.";


                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {

                    lbPath.Text = openFileDialog.SafeFileName;
                    selectedFilePath = openFileDialog.FileName;

                    Print($"File Imported >> [{openFileDialog.SafeFileName} ({openFileDialog.FileName})]");

                }
                else
                {

                    lbPath.Text = "No path selected.";

                }
            }

            btnSelectPath.Text = "Select path";
            btnSelectPathAI.Text = "Reselect";
        }

        public class ChatMessage
        {
            public string Role { get; set; } // "user" or "assistant"
            public string Content { get; set; }
        }

        public class DeepSeekRequest
        {
            public string Model { get; set; } // e.g., "deepseek-chat"
            public List<ChatMessage> Messages { get; set; }
            public double Temperature { get; set; } = 0.7;
        }

        public class DeepSeekResponse
        {
            public List<Choice> Choices { get; set; }
        }

        public class Choice
        {
            public ChatMessage Message { get; set; }
        }

        public async void DecompileAssembly(string assemblyPath, string outputDir)
        {
            

            /*try
            {*/

            // Create output directory if it doesn't exist
            Directory.CreateDirectory(outputDir);

            // Initialize decompiler
            var settings = new DecompilerSettings();
            var decompiler = new CSharpDecompiler(assemblyPath, settings);

            // Load assembly with Mono.Cecil
            var assembly = AssemblyDefinition.ReadAssembly(assemblyPath);

            // Process each type in the assembly
            foreach (var type in assembly.MainModule.Types)
            {
                try
                {
                    // Use FullTypeName to handle .NET type identities [[3]]
                    var fullTypeName = new FullTypeName(type.FullName);
                    string code = decompiler.DecompileTypeAsString(fullTypeName);

                    // Save to file using sanitized name
                    string fileName = $"{type.FullName}.cs".Replace('+', '.');
                    File.WriteAllText(Path.Combine(outputDir, fileName), code);


                }
                catch (Exception ex)
                {
                    // Sanitize type name
                    string safeTypeName = new string(type.Name
                        .Select(c => Path.GetInvalidFileNameChars().Contains(c) ? '_' : c)
                        .ToArray());

                    // Sanitize error message
                    string safeErrorMessage = new string(ex.Message
                        .Where(c => !Path.GetInvalidPathChars().Contains(c))
                        .ToArray());

                    // Write error file
                    File.WriteAllText(
                        Path.Combine(outputDir, $"ERROR_{safeTypeName}.txt"),
                        $"Failed to decompile {type.FullName}: {safeErrorMessage}"
                    );
                }
            }

            /*}
            catch
            {

                ObfuscriptDialog dialog = new ObfuscriptDialog("There was a problem.", @"We're sorry, but obfuscript failed to decompile your code.
Your code is either already obfuscated/encrypted, or may not be 
.NET-based. It may also be possible that obfuscript cannot access
the path the file is in.", ObfuscriptDialog.Types.KeepOpen);
                dialog.Show();

            }*/
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (selectedFilePath != String.Empty)
            {

                // Doesn't work properly as of now
                //DecompileAssembly(selectedFilePath, Application.StartupPath + @"\Decompiled");

            }
        }

        private void btnBackAI_Click(object sender, EventArgs e)
        {
            pnAI.Visible = false;
        }

        private void btnDiscord_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/wmq3nU9YXM");
        }

        private void lbDiscord_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/wmq3nU9YXM");
        }

        private void btnTrustPilot_Click(object sender, EventArgs e)
        {

        }

        private void lbTrustPilot_Click(object sender, EventArgs e)
        {

        }
    }
}
