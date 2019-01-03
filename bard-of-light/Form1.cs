using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using WMPLib;


namespace bard_of_light {
    public partial class Form1 : Form {

        private MidiParser midiParser;
        private System.Timers.Timer timer;
        private List<myNote> notes;
        private WindowsMediaPlayer player;
        private PlayForm playForm;
        public static double currentPlayedTime;
        
        private bool started;
        private Dictionary<string, int> noteOffset = new Dictionary<string, int>()
        {
            {"C", 0},
            {"D", 1},
            {"E", 2},
            {"F", 3},
            {"G", 4},
            {"A", 5},
            {"B", 6},
        };

        public Form1() {
            InitializeComponent();
            Console.WriteLine(string.Format("windows height:{0}, block max height:{1}", Setting.windowHeight,Setting.blockMaxHeight));
        }

        private void OpenButton_Click(object sender, EventArgs e) {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                string path = openFileDialog1.FileName;
                this.midiParser = new MidiParser(path);
                this.player = new WindowsMediaPlayer();
                player.URL = path;
                player.controls.stop();
                notes = midiParser.getAllNotes();
                deleteNotInRangeNotes();
                editNote("");
                addToNote(getFullSheet());
                finishTime = notes.Last().time + notes.Last().length;
                addToNote(finishTime.ToString() + ", rate: " + player.settings.rate + "\r\n");
                addToNote(notes.Count + "\r\n");
                
            }
        }

        private void deleteNotInRangeNotes(){
            List<myNote> temp = new List<myNote>();
            foreach (myNote note in notes)
            {
                if (note.octave < Setting.baseOctave - 1 || note.octave > Setting.baseOctave + 1){
                    continue;
                }
                temp.Add(note);
            }
            notes = temp;
        }

        private string getFullSheet() {
            string ans = "";
            foreach (myNote note in notes) {
                string str = string.Format("Name: {0}, Number: {5}, Octave: {1}, Time: {2}, Length: {3}, Velocity:{4} \r\n"
                    , note.name, note.octave, note.time, note.length, note.velocity, note.noteNumber);
                ans += str;
            }
            return ans;
        }

        private string getSimpleSheet() {
            string ans = "";
            long lastTime = -1;
            foreach (myNote note in notes) {
                if (lastTime != -1 && lastTime == note.time) {
                    continue;
                }
                string str = string.Format("{0}", noteOffset[note.name[0].ToString()] + 1);
                if (note.name.Length > 1 && note.name[1].ToString() == "S")
                    str = str + "#";
                if (note.octave == Setting.baseOctave + 1)
                    str = "+" + str;
                if (note.octave == Setting.baseOctave - 1)
                    str = "-" + str;
                ans += str;
                lastTime = note.time;
            }
            return ans;
        }


        private void readyToPlay() {

        }

        private void addToNote(string str) {
            this.noteBox.Text = str + this.noteBox.Text;
            //this.noteBox.Text = this.noteBox.Text + str;
        }

        private void editNote(string str) {
            this.noteBox.Text = str;
        }

        private void StartButton_Click(object sender, EventArgs e) {
            playForm = new PlayForm(this.notes);
            playForm.Show();
            this.timer = new System.Timers.Timer();
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Tick);
            timer.Interval = Setting.frameTime;
            currentNoteIndex = 0;
            //player.controls.play();
            //player.controls.pause();
            currentPlayedTime = -Setting.playDelay / 1000;
            started = false;
            timer.Start();
            editNote("");
            this.noteBox.Text = "";
        }

        private long finishTime;
        private int currentNoteIndex;

        private void StopButton_Click(object sender, EventArgs e) {
            if (playForm != null) {
                playForm.Close();
                return;
            }
        }

        
        private void timer_Tick(object sender, System.Timers.ElapsedEventArgs e) {
            if (started) currentPlayedTime = player.controls.currentPosition;
            //finish
            if (player.controls.currentPosition * 1000 >= finishTime + Setting.playDelay + 2000) {
                timer.Stop();
                player.controls.stop();
                started = false;
            }
            //before really start
            


            if (currentNoteIndex < notes.Count && notes[currentNoteIndex].time <= (currentPlayedTime * 1000 + Setting.playDelay)) {
                MethodInvoker mi = new MethodInvoker(() =>
                {
                    if (notes[currentNoteIndex].time > (currentPlayedTime * 1000 + Setting.playDelay)) return;
                    //Console.WriteLine(String.Format("note added on note.time:{0}, currentPlayedTime:{1}, cc:{2}, delay:{3}", 
                    //    notes[currentNoteIndex].time,
                    //    currentPlayedTime*1000, 
                    //    (currentPlayedTime * 1000 + Setting.playDelay),
                    //    Setting.playDelay));
                    produceNewNote(notes[currentNoteIndex]);
                    currentNoteIndex++;
                    StopButton.Text = currentPlayedTime.ToString();
                    //StartButton.Text = player.controls.currentItem.durationString;
                    playForm.refresh();
                });
                this.BeginInvoke(mi);
            }

            MethodInvoker invoker = new MethodInvoker(() =>
            {
                playForm.refresh();
            });
            this.BeginInvoke(invoker);

            //Console.WriteLine(String.Format("current played time:{0}, frameTime:{1}, current note {2} time is:{3}", currentPlayedTime, Setting.frameTime,currentNoteIndex, notes[currentNoteIndex].time));
            //Console.WriteLine(String.Format("delay:{0}" , Setting.playDelay));
            currentPlayedTime =Math.Round(currentPlayedTime + Setting.frameTime / 1000.0f , 3);
            if (currentPlayedTime >= 0 && !started) {
                player.controls.play();
                started = true;
                Console.WriteLine(String.Format("music starts"));
            }
        }

        private void produceNewNote(myNote note) {
            string str = string.Format("T:{3}, Name: {0}, Octave: {1},Length: {2} \r\n"
                    , note.name, note.octave, note.length, currentPlayedTime);
            addToNote(str);
            playForm.invokeNote(note);
        }
    }
}
