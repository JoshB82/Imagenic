
namespace Benchmarking
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.profileComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.statusValueLabel = new System.Windows.Forms.Label();
            this.statusTextLabel = new System.Windows.Forms.Label();
            this.lastExecutionTimeValueLabel = new System.Windows.Forms.Label();
            this.lastExecutionTimeTextLabel = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cpuValueLabel = new System.Windows.Forms.Label();
            this.cpuTextLabel = new System.Windows.Forms.Label();
            this.totalVirtualMemoryValueLabel = new System.Windows.Forms.Label();
            this.totalVirtualMemoryTextLabel = new System.Windows.Forms.Label();
            this.totalPhysicalMemoryValueLabel = new System.Windows.Forms.Label();
            this.totalPhysicalMemoryTextLabel = new System.Windows.Forms.Label();
            this.operatingSystemValueLabel = new System.Windows.Forms.Label();
            this.operatingSystemTextLabel = new System.Windows.Forms.Label();
            this.runButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // profileComboBox
            // 
            this.profileComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.profileComboBox.FormattingEnabled = true;
            this.profileComboBox.Location = new System.Drawing.Point(92, 16);
            this.profileComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.profileComboBox.Name = "profileComboBox";
            this.profileComboBox.Size = new System.Drawing.Size(289, 33);
            this.profileComboBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Profile:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.statusValueLabel);
            this.groupBox1.Controls.Add(this.statusTextLabel);
            this.groupBox1.Controls.Add(this.lastExecutionTimeValueLabel);
            this.groupBox1.Controls.Add(this.lastExecutionTimeTextLabel);
            this.groupBox1.Location = new System.Drawing.Point(15, 59);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(491, 301);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Results";
            // 
            // statusValueLabel
            // 
            this.statusValueLabel.AutoSize = true;
            this.statusValueLabel.Location = new System.Drawing.Point(100, 80);
            this.statusValueLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.statusValueLabel.Name = "statusValueLabel";
            this.statusValueLabel.Size = new System.Drawing.Size(158, 25);
            this.statusValueLabel.TabIndex = 3;
            this.statusValueLabel.Text = "No profile running";
            // 
            // statusTextLabel
            // 
            this.statusTextLabel.AutoSize = true;
            this.statusTextLabel.Location = new System.Drawing.Point(21, 80);
            this.statusTextLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.statusTextLabel.Name = "statusTextLabel";
            this.statusTextLabel.Size = new System.Drawing.Size(64, 25);
            this.statusTextLabel.TabIndex = 2;
            this.statusTextLabel.Text = "Status:";
            // 
            // lastExecutionTimeValueLabel
            // 
            this.lastExecutionTimeValueLabel.AutoSize = true;
            this.lastExecutionTimeValueLabel.Location = new System.Drawing.Point(208, 41);
            this.lastExecutionTimeValueLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lastExecutionTimeValueLabel.Name = "lastExecutionTimeValueLabel";
            this.lastExecutionTimeValueLabel.Size = new System.Drawing.Size(44, 25);
            this.lastExecutionTimeValueLabel.TabIndex = 1;
            this.lastExecutionTimeValueLabel.Text = "N/A";
            // 
            // lastExecutionTimeTextLabel
            // 
            this.lastExecutionTimeTextLabel.AutoSize = true;
            this.lastExecutionTimeTextLabel.Location = new System.Drawing.Point(21, 41);
            this.lastExecutionTimeTextLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lastExecutionTimeTextLabel.Name = "lastExecutionTimeTextLabel";
            this.lastExecutionTimeTextLabel.Size = new System.Drawing.Size(170, 25);
            this.lastExecutionTimeTextLabel.TabIndex = 0;
            this.lastExecutionTimeTextLabel.Text = "Last Execution Time:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cpuValueLabel);
            this.groupBox2.Controls.Add(this.cpuTextLabel);
            this.groupBox2.Controls.Add(this.totalVirtualMemoryValueLabel);
            this.groupBox2.Controls.Add(this.totalVirtualMemoryTextLabel);
            this.groupBox2.Controls.Add(this.totalPhysicalMemoryValueLabel);
            this.groupBox2.Controls.Add(this.totalPhysicalMemoryTextLabel);
            this.groupBox2.Controls.Add(this.operatingSystemValueLabel);
            this.groupBox2.Controls.Add(this.operatingSystemTextLabel);
            this.groupBox2.Location = new System.Drawing.Point(515, 16);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(602, 344);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Machine Information";
            // 
            // cpuValueLabel
            // 
            this.cpuValueLabel.AutoSize = true;
            this.cpuValueLabel.Location = new System.Drawing.Point(86, 175);
            this.cpuValueLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.cpuValueLabel.Name = "cpuValueLabel";
            this.cpuValueLabel.Size = new System.Drawing.Size(41, 25);
            this.cpuValueLabel.TabIndex = 7;
            this.cpuValueLabel.Text = "cpu";
            // 
            // cpuTextLabel
            // 
            this.cpuTextLabel.AutoSize = true;
            this.cpuTextLabel.Location = new System.Drawing.Point(29, 175);
            this.cpuTextLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.cpuTextLabel.Name = "cpuTextLabel";
            this.cpuTextLabel.Size = new System.Drawing.Size(49, 25);
            this.cpuTextLabel.TabIndex = 6;
            this.cpuTextLabel.Text = "CPU:";
            // 
            // totalVirtualMemoryValueLabel
            // 
            this.totalVirtualMemoryValueLabel.AutoSize = true;
            this.totalVirtualMemoryValueLabel.Location = new System.Drawing.Point(302, 130);
            this.totalVirtualMemoryValueLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.totalVirtualMemoryValueLabel.Name = "totalVirtualMemoryValueLabel";
            this.totalVirtualMemoryValueLabel.Size = new System.Drawing.Size(173, 25);
            this.totalVirtualMemoryValueLabel.TabIndex = 5;
            this.totalVirtualMemoryValueLabel.Text = "total virtual memory";
            // 
            // totalVirtualMemoryTextLabel
            // 
            this.totalVirtualMemoryTextLabel.AutoSize = true;
            this.totalVirtualMemoryTextLabel.Location = new System.Drawing.Point(29, 130);
            this.totalVirtualMemoryTextLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.totalVirtualMemoryTextLabel.Name = "totalVirtualMemoryTextLabel";
            this.totalVirtualMemoryTextLabel.Size = new System.Drawing.Size(251, 25);
            this.totalVirtualMemoryTextLabel.TabIndex = 4;
            this.totalVirtualMemoryTextLabel.Text = "Total virtual memory available:";
            // 
            // totalPhysicalMemoryValueLabel
            // 
            this.totalPhysicalMemoryValueLabel.AutoSize = true;
            this.totalPhysicalMemoryValueLabel.Location = new System.Drawing.Point(236, 84);
            this.totalPhysicalMemoryValueLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.totalPhysicalMemoryValueLabel.Name = "totalPhysicalMemoryValueLabel";
            this.totalPhysicalMemoryValueLabel.Size = new System.Drawing.Size(188, 25);
            this.totalPhysicalMemoryValueLabel.TabIndex = 3;
            this.totalPhysicalMemoryValueLabel.Text = "total physical memory";
            // 
            // totalPhysicalMemoryTextLabel
            // 
            this.totalPhysicalMemoryTextLabel.AutoSize = true;
            this.totalPhysicalMemoryTextLabel.Location = new System.Drawing.Point(29, 84);
            this.totalPhysicalMemoryTextLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.totalPhysicalMemoryTextLabel.Name = "totalPhysicalMemoryTextLabel";
            this.totalPhysicalMemoryTextLabel.Size = new System.Drawing.Size(192, 25);
            this.totalPhysicalMemoryTextLabel.TabIndex = 2;
            this.totalPhysicalMemoryTextLabel.Text = "Total Physical Memory:";
            // 
            // operatingSystemValueLabel
            // 
            this.operatingSystemValueLabel.AutoSize = true;
            this.operatingSystemValueLabel.Location = new System.Drawing.Point(200, 42);
            this.operatingSystemValueLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.operatingSystemValueLabel.Name = "operatingSystemValueLabel";
            this.operatingSystemValueLabel.Size = new System.Drawing.Size(150, 25);
            this.operatingSystemValueLabel.TabIndex = 1;
            this.operatingSystemValueLabel.Text = "operating system";
            // 
            // operatingSystemTextLabel
            // 
            this.operatingSystemTextLabel.AutoSize = true;
            this.operatingSystemTextLabel.Location = new System.Drawing.Point(29, 42);
            this.operatingSystemTextLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.operatingSystemTextLabel.Name = "operatingSystemTextLabel";
            this.operatingSystemTextLabel.Size = new System.Drawing.Size(158, 25);
            this.operatingSystemTextLabel.TabIndex = 0;
            this.operatingSystemTextLabel.Text = "Operating System:";
            // 
            // runButton
            // 
            this.runButton.Location = new System.Drawing.Point(390, 16);
            this.runButton.Margin = new System.Windows.Forms.Padding(4);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(118, 36);
            this.runButton.TabIndex = 4;
            this.runButton.Text = "Run!";
            this.runButton.UseVisualStyleBackColor = true;
            this.runButton.Click += new System.EventHandler(this.runButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1132, 375);
            this.Controls.Add(this.runButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.profileComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Benchmarking";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox profileComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lastExecutionTimeValueLabel;
        private System.Windows.Forms.Label lastExecutionTimeTextLabel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label operatingSystemValueLabel;
        private System.Windows.Forms.Label operatingSystemTextLabel;
        private System.Windows.Forms.Label totalPhysicalMemoryValueLabel;
        private System.Windows.Forms.Label totalPhysicalMemoryTextLabel;
        private System.Windows.Forms.Label totalVirtualMemoryValueLabel;
        private System.Windows.Forms.Label totalVirtualMemoryTextLabel;
        private System.Windows.Forms.Label cpuTextLabel;
        private System.Windows.Forms.Label cpuValueLabel;
        private System.Windows.Forms.Button runButton;
        private System.Windows.Forms.Label statusValueLabel;
        private System.Windows.Forms.Label statusTextLabel;
    }
}

