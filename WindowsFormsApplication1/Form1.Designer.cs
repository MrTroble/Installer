﻿namespace TAS_Installer
{
    partial class TAS
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TAS));
            this.SuspendLayout();
            // 
            // TAS
            // 
            resources.ApplyResources(this, "$this");
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.TitleBar;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::TAS_Installer.Properties.Settings.Default, "tas_ins", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.DataBindings.Add(new System.Windows.Forms.Binding("Location", global::TAS_Installer.Properties.Settings.Default, "size", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Location = global::TAS_Installer.Properties.Settings.Default.size;
            this.Name = "TAS";
            this.ShowIcon = false;
            this.Text = global::TAS_Installer.Properties.Settings.Default.tas_ins;
            this.Load += new System.EventHandler(this.TAS_Load);
            this.ResumeLayout(false);

        }

        #endregion
    }
}

