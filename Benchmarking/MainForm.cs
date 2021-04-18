using Benchmarking.MachineInfo;
using Benchmarking.Profiles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Benchmarking
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            // Initialise profile combo box
            ProfileCollection.PopulateProfileList();
            ProfileCollection.Profiles.ForEach(x => profileComboBox.Items.Add(x.Name));
            profileComboBox.SelectedIndex = 0;

            MemoryInfo memoryInfo = new();

            operatingSystemValueLabel.Text = OSInfo.OperatingSystemVersion;
            //totalPhysicalMemoryValueLabel.Text = $"{memoryInfo.GetTotalPhysicalMemoryBytes()} bytes (~{memoryInfo.GetTotalPhysicalMemoryGigabytes()} GB)";

            //totalVirtualMemoryValueLabel.Text = $"{memoryInfo.GetTotalVirtualMemoryBytes()} bytes (~{memoryInfo.GetTotalVirtualMemoryTerabytes()} TB)";

            cpuValueLabel.Text = CPUInfo.GetCPUName;
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            string selectedProfileName = profileComboBox.SelectedItem.ToString();
            Profile profileToRun = ProfileCollection.Profiles.Find(x => x.Name == selectedProfileName);

            statusValueLabel.Text = "Profile running...";

            Stopwatch stopwatch = new();
            stopwatch.Start();
            profileToRun.Action();
            stopwatch.Stop();

            lastExecutionTimeValueLabel.Text = stopwatch.Elapsed.ToString();

            statusValueLabel.Text = "No profile running";
        }
    }
}