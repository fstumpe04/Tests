using Microsoft.Win32;
using SerializationAndDeserializationXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace XsdHandlingTest
{
    internal class MainViewModel
    {
        private ICommand? _CreateClassesFromXmlSchemaCommand;
        private ICommand? _SerializeToXmlCommand;

        public MainViewModel()
        {
            Model = new MainModel();
        }

        public MainModel Model { get; set; }

        public ICommand? CreateClassesFromXmlSchemaCommand
        {
            get
            {
                if (_CreateClassesFromXmlSchemaCommand == null)
                {
                    _CreateClassesFromXmlSchemaCommand = new RelayCommand(CreateClassesFromXmlSchema);
                }
                return _CreateClassesFromXmlSchemaCommand;
            }
        }

        public ICommand? SerializeToXmlCommand
        {
            get
            {
                if (_SerializeToXmlCommand == null)
                {
                    _SerializeToXmlCommand = new RelayCommand(SerializeToXml);
                }
                return _SerializeToXmlCommand;
            }
        }

        private void CreateClassesFromXmlSchema()
        {
            string xsdExeFullPath = @"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\xsd.exe";

            const string classesArgument = "/classes";
            OpenFileDialog openXsdFileDialog = new OpenFileDialog();
            openXsdFileDialog.DefaultExt = ".xsd";
            openXsdFileDialog.Filter = "Xml Schema documents (.xsd) | *.xsd";
            bool? openXsdFileDialogResult = openXsdFileDialog.ShowDialog();
            if (openXsdFileDialogResult != true)
            {
                return;
            }

            OpenFolderDialog outputOpenFolderDialog = new OpenFolderDialog();
            outputOpenFolderDialog.DefaultDirectory = Path.GetDirectoryName(openXsdFileDialog.FileName);
            bool? outputOpenFolderDialogResult = outputOpenFolderDialog.ShowDialog();
            if (outputOpenFolderDialogResult != true)
            { 
                return; 
            }

            string outPathArgument = $"-out:{outputOpenFolderDialog.FolderName}";

            IEnumerable<string> arguments = new List<string>
            {
                classesArgument,
                openXsdFileDialog.FileName,
                outPathArgument
            };

            ProcessStartInfo processStartInfo = new ProcessStartInfo(xsdExeFullPath, arguments);
            processStartInfo.UseShellExecute = false;
            Process? cmd = Process.Start(processStartInfo);
            if (cmd != null)
            {
                cmd.WaitForExit();
                MessageBox.Show("C# classes are created!");
            }
        }

        private void SerializeToXml()
        {
            OpenFolderDialog outputOpenFolderDialog = new OpenFolderDialog();
            bool? outputOpenFolderDialogResult = outputOpenFolderDialog.ShowDialog();
            XmlSerializerService.SerializeToXml<PurchaseOrderType>($"{nameof(PurchaseOrderType)}.xml", Model.PurchaseOrder);

            OpenFolder(outputOpenFolderDialog.FolderName);
        }

        private void OpenFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    Arguments = folderPath,
                    FileName = "explorer.exe"
                };

                Process.Start(startInfo);
            }
            else
            {
                MessageBox.Show(string.Format("{0} Directory does not exist!", folderPath));
            }
        }
    }
}
