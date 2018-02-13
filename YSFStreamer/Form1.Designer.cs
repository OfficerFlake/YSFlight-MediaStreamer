namespace YSFStreamer
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.Streamer = new AxWMPLib.AxWindowsMediaPlayer();
            this.VolumeSlider = new System.Windows.Forms.TrackBar();
            this.WaitTimer = new System.Windows.Forms.Label();
            this.ResyncButton = new System.Windows.Forms.Button();
            this.BufferingLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Streamer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VolumeSlider)).BeginInit();
            this.SuspendLayout();
            // 
            // Streamer
            // 
            this.Streamer.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            resources.ApplyResources(this.Streamer, "Streamer");
            this.Streamer.Name = "Streamer";
            this.Streamer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("Streamer.OcxState")));
            this.Streamer.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(this.UpdateLabel);
            this.Streamer.EndOfStream += new AxWMPLib._WMPOCXEvents_EndOfStreamEventHandler(this.Finish);
            this.Streamer.Buffering += new AxWMPLib._WMPOCXEvents_BufferingEventHandler(this.UpdateBufferState);
            // 
            // VolumeSlider
            // 
            resources.ApplyResources(this.VolumeSlider, "VolumeSlider");
            this.VolumeSlider.BackColor = System.Drawing.Color.Black;
            this.VolumeSlider.Name = "VolumeSlider";
            this.VolumeSlider.Value = 10;
            this.VolumeSlider.ValueChanged += new System.EventHandler(this.AdjustVolume);
            // 
            // WaitTimer
            // 
            resources.ApplyResources(this.WaitTimer, "WaitTimer");
            this.WaitTimer.BackColor = System.Drawing.Color.Black;
            this.WaitTimer.ForeColor = System.Drawing.Color.White;
            this.WaitTimer.Name = "WaitTimer";
            // 
            // ResyncButton
            // 
            resources.ApplyResources(this.ResyncButton, "ResyncButton");
            this.ResyncButton.BackColor = System.Drawing.SystemColors.Control;
            this.ResyncButton.Name = "ResyncButton";
            this.ResyncButton.UseVisualStyleBackColor = false;
            this.ResyncButton.Click += new System.EventHandler(this.Resynrchronise);
            // 
            // BufferingLabel
            // 
            this.BufferingLabel.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.BufferingLabel, "BufferingLabel");
            this.BufferingLabel.ForeColor = System.Drawing.Color.White;
            this.BufferingLabel.Name = "BufferingLabel";
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.BufferingLabel);
            this.Controls.Add(this.ResyncButton);
            this.Controls.Add(this.WaitTimer);
            this.Controls.Add(this.VolumeSlider);
            this.Controls.Add(this.Streamer);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Name = "Form1";
            this.Shown += new System.EventHandler(this.Init);
            ((System.ComponentModel.ISupportInitialize)(this.Streamer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VolumeSlider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AxWMPLib.AxWindowsMediaPlayer Streamer;
        private System.Windows.Forms.TrackBar VolumeSlider;
        private System.Windows.Forms.Label WaitTimer;
        private System.Windows.Forms.Button ResyncButton;
        private System.Windows.Forms.Label BufferingLabel;
    }
}

