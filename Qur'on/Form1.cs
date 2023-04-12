using System.Runtime.InteropServices;

namespace Qur_on
{
    public partial class Form1 : Form
    {
        WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();
        int a = 0;
        public Form1()
        {
            InitializeComponent();
            StreamReader output = new StreamReader(@"tarix.txt");
            string[] str = output.ReadToEnd().Split();
            output.Close();
            if (str.Length == 6)
            {
                comboBox1.SelectedIndex = int.Parse(str[0]);
                label2.Text = str[1];
                wplayer.URL = comboBox1.SelectedItem.ToString() + "\\" + str[1];
                wplayer.controls.currentPosition = double.Parse(str[2]);
                slider.Width = (int)((wplayer.controls.currentPosition * (490 / double.Parse(str[3]))) + 0.5);
                label1.Text = vaqt((int)(wplayer.controls.currentPosition + 0.5));
                a = (int)double.Parse(str[2]);
                label3.Text = vaqt((int)double.Parse(str[3]));
                pnlBoshi.Width = int.Parse(str[4]);
                pnlOxiri.Width = int.Parse(str[5]);
                wplayer.controls.stop();
            }
            else comboBox1.SelectedIndex = 0;

        }
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        string[] btn = new string[116];
        public void Create(string a)
        {
            int i = 0;
            var directory = new DirectoryInfo(a);
            pnlList.Controls.Clear();
            FileInfo[] files = directory.GetFiles();
            foreach (FileInfo file in files)
            {
                if (file.Name.Substring(file.Name.Length - 3) == "mp3")
                {
                    Button butt = new Button();
                    butt.Size = new Size(270, 40);
                    butt.ForeColor = Color.White;
                    butt.Text = "🔊 " + file.Name.Substring(0, file.Name.Length - 4);
                    StreamReader reader = new StreamReader("info.txt");
                    string[] inf = reader.ReadToEnd().Split('|');
                    reader.Close();
                    for (int j = 0; j < inf.Length - 1; j++)
                    {
                        if (inf[j] == file.Name.Substring(0, 3)) butt.Text += "   (" + inf[j + 1].Split()[2] + ")";
                    }
                    butt.TextAlign = (System.Drawing.ContentAlignment)HorizontalAlignment.Right;
                    butt.Click += new EventHandler(Butt_Click);
                    butt.MouseHover += new EventHandler(Butt_MouseHover);
                    butt.MouseLeave += new EventHandler(Butt_MouseLeave);
                    butt.Font = new Font("Nirmala UI", 10);
                    butt.FlatStyle = FlatStyle.Flat;
                    butt.FlatAppearance.BorderSize = 0;
                    butt.FlatAppearance.MouseOverBackColor = Color.Transparent;
                    butt.FlatAppearance.MouseDownBackColor = Color.Transparent;
                    butt.Location = new Point(0, 40 * i);
                    butt.Cursor = Cursors.Hand;
                    pnlList.Controls.Add(butt);
                    pnlList.AutoScroll = true;
                    btn[i] = file.Name;
                    i++;
                }
            }
        }
        void Butt_MouseHover(object sender, EventArgs e)
        {
            Button butt = (Button)sender;
            butt.ForeColor = Color.DodgerBlue;
        }
        void Butt_MouseLeave(object sender, EventArgs e)
        {
            Button butt = (Button)sender;
            butt.ForeColor = Color.White;
        }
        void Butt_Click(Object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            timer2.Start();
            pnlBoshi.Width = 10;
            pnlOxiri.Width = 10;
            slider.Width = 1;
            label1.Text = "00:00";
            button2.Text = "⏸";
            if (clickedButton.Name == "button4")
            {
                int k = -1;
                for (int i = 0; i < 115; i++)
                {
                    if (label2.Text == btn[i] && i < 113) k = i;
                }
                wplayer.URL = comboBox1.SelectedItem.ToString() + "\\" + btn[k + 1];
                label2.Text = btn[k + 1];

            }
            else if (clickedButton.Name == "button3")
            {

                int k = 114;
                for (int i = 0; i < 115; i++)
                {
                    if (label2.Text == btn[i] && i > 0) k = i;
                }
                wplayer.URL = comboBox1.SelectedItem.ToString() + "\\" + btn[k - 1];
                label2.Text = btn[k - 1];

            }
            else
            {
                label2.Text = clickedButton.Text.Split()[1] + ".mp3";
                wplayer.controls.currentPosition = 0;
                wplayer.URL = comboBox1.SelectedItem.ToString() + "\\" + clickedButton.Text.Split()[1] + ".mp3";
            }
            timer1.Start();
            label1.Text = vaqt((int)(wplayer.controls.currentPosition));
            label3.Text = vaqt((int)((panel6.Width - pnlOxiri.Width + 10) / (panel3.Width / wplayer.controls.currentItem.duration)));
            wplayer.controls.play();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
            StreamWriter input = new StreamWriter(@"tarix.txt");
            input.Write(comboBox1.SelectedIndex.ToString() + " " + label2.Text + " " + wplayer.controls.currentPosition + " " + wplayer.controls.currentItem.duration + " " + pnlBoshi.Width + " " + pnlOxiri.Width);
            input.Close();
        }
        private void Form1_Load(object sender, EventArgs e) { }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (wplayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                slider.Width = (int)((wplayer.controls.currentPosition * (panel3.Width / wplayer.controls.currentItem.duration)));
                label1.Text = vaqt((int)(wplayer.controls.currentPosition));
                label3.Text = vaqt((int)((panel6.Width - pnlOxiri.Width + 10) / (panel3.Width / wplayer.controls.currentItem.duration)));
                if (slider.Width > panel8.Width - pnlOxiri.Width + 10)
                {
                    wplayer.controls.currentPosition = (int)((pnlBoshi.Width - 11) / (panel3.Width / wplayer.controls.currentItem.duration));
                    slider.Width = pnlBoshi.Width - 11;
                }
                if (slider.Width < pnlBoshi.Width - 11)
                {
                    wplayer.controls.currentPosition = (int)((pnlBoshi.Width - 11) / (panel3.Width / wplayer.controls.currentItem.duration));
                    slider.Width = pnlBoshi.Width - 11;
                }
            }
            if (wplayer.playState != WMPLib.WMPPlayState.wmppsPlaying)
            {
                if (label1.Text == label3.Text) slider.Width = panel6.Width;
                label1.Text = label3.Text;
            }
            if (label1.Text == label3.Text && slider.Width == panel6.Width)
            {
                if (button7.Text == "🔁")
                {
                    int k = -1;
                    for (int i = 0; i < 115; i++)
                    {
                        if (label2.Text == btn[i] && i < 113) k = i;
                    }
                    wplayer.URL = comboBox1.SelectedItem.ToString() + "\\" + btn[k + 1];
                    label2.Text = btn[k + 1];
                }
                else if (button7.Text == "🔂")
                {
                    wplayer.URL = comboBox1.SelectedItem.ToString() + "\\" + label2.Text;
                }
                timer2.Start();
                label1.Text = vaqt((int)(wplayer.controls.currentPosition));
                label3.Text = vaqt((int)((panel6.Width - pnlOxiri.Width + 10) / (panel3.Width / wplayer.controls.currentItem.duration)));
            }
        }
        string vaqt(int a)
        {
            string s = string.Empty;
            if (a / 60 != 0)
            {
                if (a / 3600 != 0)
                {
                    if ((a / 60) % 60 < 10)
                    {
                        if (a % 60 < 10) s = (int)(a / 3600) + ":0" + (int)((a / 60) % 60) + ":0" + (int)(a % 60);
                        else s = (int)(a / 3600) + ":0" + (int)((a / 60) % 60) + ":" + (int)(a % 60);
                    }
                    else
                    {
                        if (a % 60 < 10) s = (int)(a / 3600) + ":" + (int)((a / 60) % 60) + ":0" + (int)(a % 60);
                        else s = (int)(a / 3600) + ":" + (int)((a / 60) % 60) + ":" + (int)(a % 60);
                    }
                }
                else
                {
                    if (a / 60 < 10)
                    {
                        if (a % 60 < 10) s = "0" + (int)(a / 60) + ":0" + (int)(a % 60);
                        else s = "0" + (int)(a / 60) + ":" + (int)(a % 60);
                    }
                    else
                    {
                        if (a % 60 < 10) s = (int)(a / 60) + ":0" + (int)(a % 60);
                        else s = (int)(a / 60) + ":" + (int)(a % 60);

                    }
                }
            }
            else
            {
                if (a < 10) s = "00:0" + a;
                else s = "00:" + a;
            }
            return s;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "⏸")
            {
                a = (int)wplayer.controls.currentPosition;
                wplayer.controls.pause();
                button2.Text = "▶";
                timer1.Stop();
            }
            else if (button2.Text == "▶")
            {
                wplayer.controls.play();
                timer1.Start();
                wplayer.controls.currentPosition = a;
                button2.Text = "⏸";
            }
            label1.Text = vaqt((int)(wplayer.controls.currentPosition));
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Create(comboBox1.SelectedItem.ToString());
        }
        private void button5_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void panel6_Click(object sender, EventArgs e)
        {
            slider.Width = (MousePosition.X - this.Location.X - pnlList.Width);
            wplayer.controls.currentPosition = a = (int)(slider.Width / (panel3.Width / wplayer.controls.currentItem.duration));
            label1.Text = vaqt((int)wplayer.controls.currentPosition);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            wplayer.controls.stop();
            timer1.Stop();
            a = 0;
            button2.Text = "▶";
            label1.Text = "00:00";
            slider.Width = 1;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            button7.Text = (button7.Text == "🔁") ? button7.Text = "🔂" : "🔁";
        }
        bool up = false;
        private void panel6_MouseMove(object sender, MouseEventArgs e)
        {
            if (up && MousePosition.X - this.Location.X - pnlList.Width <= panel6.Width && MousePosition.X - this.Location.X - pnlList.Width >= 0)
            {
                slider.Width = (MousePosition.X - this.Location.X - pnlList.Width);
                label1.Text = vaqt((int)(slider.Width / (panel3.Width / wplayer.controls.currentItem.duration)));
            }
        }

        private void panel6_MouseDown(object sender, MouseEventArgs e)
        {
            up = true;
        }

        private void panel6_MouseUp(object sender, MouseEventArgs e)
        {
            up = false;
            wplayer.controls.currentPosition = a = (int)(slider.Width / (panel3.Width / wplayer.controls.currentItem.duration));

        }
        bool y = false;
        private void Boshi_MouseDown(object sender, MouseEventArgs e)
        {
            y = true;
        }

        private void Boshi_MouseMove(object sender, MouseEventArgs e)
        {
            if (y)
            {
                if ((MousePosition.X - this.Location.X - pnlList.Width) > 11 && (MousePosition.X - this.Location.X - pnlList.Width) < panel3.Width - pnlOxiri.Width)
                    pnlBoshi.Width = (MousePosition.X - this.Location.X - pnlList.Width);
                label1.Text = vaqt((int)((pnlBoshi.Width - 11) / (panel3.Width / wplayer.controls.currentItem.duration)));
            }
        }
        private void Boshi_MouseUp(object sender, MouseEventArgs e)
        {
            y = false;
            wplayer.controls.currentPosition = (int)((pnlBoshi.Width - 11) / (panel3.Width / wplayer.controls.currentItem.duration));
            slider.Width = pnlBoshi.Width - 11;
        }

        private void Oxiri_MouseDown(object sender, MouseEventArgs e)
        {
            y = true;
            label3.Text = vaqt((int)((panel6.Width - pnlOxiri.Width + 10) / (panel3.Width / wplayer.controls.currentItem.duration)));
        }

        private void Oxiri_MouseMove(object sender, MouseEventArgs e)
        {
            if (y)
            {
                if ((MousePosition.X - this.Location.X - pnlList.Width) < panel3.Width - 11 && (MousePosition.X - this.Location.X - pnlList.Width) > pnlBoshi.Width)
                    pnlOxiri.Width = panel3.Width - (MousePosition.X - this.Location.X - pnlList.Width);
                label3.Text = vaqt((int)((panel6.Width - pnlOxiri.Width + 10) / (panel3.Width / wplayer.controls.currentItem.duration)));
            }
        }
        private void Oxiri_MouseUp(object sender, MouseEventArgs e)
        {
            y = false;
            label3.Text = vaqt((int)((panel6.Width - pnlOxiri.Width + 10) / (panel3.Width / wplayer.controls.currentItem.duration)));
        }
        int info = 0;
        private void timer2_Tick(object sender, EventArgs e)
        {
            StreamReader reader = new StreamReader("info.txt");
            string[] inf = reader.ReadToEnd().Split('|');
            reader.Close();
            for (int i = 0; i < inf.Length - 1; i++)
            {
                if (inf[i] == label2.Text.Substring(0, 3)) label4.Text = inf[i + 1];
            }
            info += 1;
            if (info == 12)
            {
                label4.Text = "";
                timer2.Stop();
                info = 0;
            }
        }

        private void panel8_Click(object sender, EventArgs e)
        {

        }
    }
}