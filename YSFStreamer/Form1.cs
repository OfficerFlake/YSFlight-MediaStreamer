using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
using System.Globalization;

namespace YSFStreamer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            BufferingLabel.Parent = Streamer;
            BufferingLabel.BackColor = Color.Transparent;
            BackColor = Color.Black;
            BufferingLabel.Visible = false;
        }

        public void AdjustVolume(object sender, EventArgs e)
        {
            Streamer.settings.volume = VolumeSlider.Value * 10;
        }

        public TimeSpan GetVariance()
        {
            var client = new TcpClient("64.90.182.55", 13);
            using (var streamReader = new StreamReader(client.GetStream()))
            {
                var response = streamReader.ReadToEnd();
                var utcDateTimeString = response.Substring(7, 17);
                utcDateTimeString = utcDateTimeString.Remove(0, 6).Remove(2) + utcDateTimeString.Remove(0, 2).Remove(4) + utcDateTimeString.Remove(2) + utcDateTimeString.Remove(0, 8);
                bool ShowTime = Boolean.Parse(File.ReadAllLines("./ShowData.txt")[5]);
                //if (ShowTime) MessageBox.Show(utcDateTimeString.Remove(0, 8));
                DateTime Actual = DateTime.Parse(utcDateTimeString);
                if (ShowTime) MessageBox.Show("Actual: " + Actual.ToString());
                DateTime ThisSystem = DateTime.UtcNow;
                if (ShowTime) MessageBox.Show("ThisSystem: " + ThisSystem.ToString());
                TimeSpan Final = (Actual - ThisSystem);
                if (ShowTime) MessageBox.Show("Variance: " + Final.ToString());
                return Final;
            }
        }

        public void Init(object sender, EventArgs e)
        {
            string PreShow = File.ReadAllLines("./ShowData.txt")[0];
            string Show = File.ReadAllLines("./ShowData.txt")[1];
            string PostShow = File.ReadAllLines("./ShowData.txt")[2];
            bool ShowTime = Boolean.Parse(File.ReadAllLines("./ShowData.txt")[5]);
            TimeSpan TimeVariance = GetVariance();
            string StartValue = File.ReadAllLines("./ShowData.txt")[3];
            DateTime StartTime = DateTime.Parse(File.ReadAllLines("./ShowData.txt")[3]) - TimeVariance;
            DateTime EndTime = DateTime.Parse(File.ReadAllLines("./ShowData.txt")[4]) - TimeVariance;
            if (ShowTime) MessageBox.Show(DateTime.UtcNow.ToString());
            if (ShowTime) MessageBox.Show((DateTime.Parse(File.ReadAllLines("./ShowData.txt")[3]) - TimeVariance).ToString());
            Streamer.URL = PreShow;
            Application.DoEvents();
            this.Refresh();
            Streamer.Refresh();
            VolumeSlider.Refresh();
            while (DateTime.UtcNow < StartTime)
            {
                try
                {
                    WaitTimer.Text = "The Show will begin in: " + Math.Abs((DateTime.UtcNow - StartTime).Days).ToString() + " Days, " +
                        Math.Abs((DateTime.UtcNow - StartTime).Hours).ToString() + " Hours, " +
                        Math.Abs((DateTime.UtcNow - StartTime).Minutes).ToString() + " Minutes, " +
                        Math.Abs((DateTime.UtcNow - StartTime).Seconds).ToString() + " Seconds.";
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                }
                Application.DoEvents();
            }
            WaitTimer.Text = "";
            if (DateTime.UtcNow > EndTime)
            {
                WaitTimer.Text = "The Stream has ended.";
                return;
            }
            Streamer.URL = Show;
            Streamer.Ctlcontrols.currentPosition = 0;
            Streamer.Ctlcontrols.pause();
            Streamer.settings.volume = 100;
            if(DateTime.UtcNow > StartTime && DateTime.UtcNow < EndTime) {
                Streamer.Ctlcontrols.currentPosition = (DateTime.UtcNow - StartTime).TotalSeconds;
            }
            while (DateTime.UtcNow >= StartTime && DateTime.UtcNow <= EndTime) {
                string mins = (DateTime.UtcNow - StartTime).Minutes.ToString();
                if (mins.Length == 1)
                {
                    mins = "0" + mins;
                }
                string secs = (DateTime.UtcNow - StartTime).Seconds.ToString();
                if (secs.Length == 1)
                {
                    secs = "0" + secs;
                }
                WaitTimer.Text = mins + ":" + secs;
                Application.DoEvents();
            }
            Streamer.URL = PostShow;
            WaitTimer.Text = "The Stream has ended.";
            WaitTimer.ForeColor = System.Drawing.Color.Red;
        }

        private void Finish(object sender, AxWMPLib._WMPOCXEvents_EndOfStreamEvent e)
        {
            WaitTimer.Text = "The Stream has ended.";
            WaitTimer.ForeColor = System.Drawing.Color.Red;
        }

        private void Resynrchronise(object sender, EventArgs e)
        {
            TimeSpan TimeVariance = GetVariance();
            DateTime StartTime = DateTime.Parse(File.ReadAllLines("./ShowData.txt")[3]) - TimeVariance;
            DateTime EndTime = DateTime.Parse(File.ReadAllLines("./ShowData.txt")[4]) - TimeVariance;
            if (DateTime.UtcNow > StartTime && DateTime.UtcNow < EndTime)
            {
                Streamer.Ctlcontrols.currentPosition = (DateTime.UtcNow - StartTime).TotalSeconds;
            }
        }

        private void UpdateBufferState(object sender, AxWMPLib._WMPOCXEvents_BufferingEvent e)
        {
            if (e.start)
            {
                BufferingLabel.Visible = true;
                BufferingLabel.Text = "Buffering...";
            }
            else BufferingLabel.Visible = false;
        }

        private void UpdateLabel(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            BufferingLabel.Visible = true;
            TimeSpan TimeVariance = GetVariance();
            DateTime StartTime = DateTime.Parse(File.ReadAllLines("./ShowData.txt")[3]) - TimeVariance;
            DateTime EndTime = DateTime.Parse(File.ReadAllLines("./ShowData.txt")[4]) - TimeVariance;
            switch (e.newState) {
                case 1:
                    BufferingLabel.Text = "Stopped.";
                    break;
                case 2:
                    BufferingLabel.Text = "Playing.";
                    //Streamer.Ctlcontrols.play();
                    break;
                case 3:
                    BufferingLabel.Visible = false;
                    break;
                case 4:
                case 5:
                    BufferingLabel.Text = "Scanning.";
                    //Streamer.Ctlcontrols.play();
                    break;
                case 6:
                    BufferingLabel.Text = "Buffering...";
                    if (DateTime.UtcNow > StartTime && DateTime.UtcNow < EndTime)
                    {
                        Streamer.Ctlcontrols.currentPosition = (DateTime.UtcNow - StartTime).TotalSeconds;
                    }
                    break;
                case 7:
                    BufferingLabel.Text = "Waiting On Server...";
                    break;
                case 8:
                    BufferingLabel.Text = "End Of Stream";
                    break;
                case 9:
                    BufferingLabel.Text = "Connecting...";
                    break;
                case 10:
                    if (DateTime.UtcNow > StartTime && DateTime.UtcNow < EndTime)
                    {
                        Streamer.Ctlcontrols.currentPosition = (DateTime.UtcNow - StartTime).TotalSeconds;
                        BufferingLabel.Text = "Ready.";
                    }
                    else
                    {
                        BufferingLabel.Text = "The Show Has Ended.";
                    }
                    break;
                case 11:
                    BufferingLabel.Text = "Reconnecting";
                    break;
                default:
                    BufferingLabel.Text = "State: " + e.newState.ToString();
                    break;
            }
        }
    }
}
