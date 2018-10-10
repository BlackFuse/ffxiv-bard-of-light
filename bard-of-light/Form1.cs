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
            foreach (myNote note in notes)
            {
                if (note.octave < Setting.baseOctave - 1 || note.octave > Setting.baseOctave + 1){
                    notes.Remove(note);
                }
            }
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
            //this.noteBox.Text =  str + this.noteBox.Text;
            this.noteBox.Text = this.noteBox.Text + str;
        }

        private void editNote(string str) {
            this.noteBox.Text = str;
        }

        private void StartButton_Click(object sender, EventArgs e) {
            if (playForm != null) {
                playForm.Close();
            }
            playForm = new PlayForm(this.notes);
            playForm.Show();
        }

        private long finishTime;
        private int currentNoteIndex;

        private void StopButton_Click(object sender, EventArgs e) {
            this.timer = new System.Timers.Timer();
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Tick);
            timer.Interval = Setting.frameTime;
            currentNoteIndex = 0;
            timer.Start();
            
            player.controls.play();
            
            this.noteBox.Text = "";
            //player.controls.currentPosition;

        }

        private void timer_Tick(object sender, System.Timers.ElapsedEventArgs e) {

            if (player.controls.currentPosition * 1000 >= finishTime + 2000) {
                timer.Stop();
                player.controls.stop();
            }
               
            
            if (currentNoteIndex < notes.Count && notes[currentNoteIndex].time <= player.controls.currentPosition * 1000.0) {
                MethodInvoker mi = new MethodInvoker(() =>
                {
                    produceNewNote(notes[currentNoteIndex]);
                    currentNoteIndex++;
                    //StopButton.Text = counter.ToString();
                    StopButton.Text = player.controls.currentItem.durationString;
                });
                this.BeginInvoke(mi);
            }
        }

        private void produceNewNote(myNote note) {
            string str = string.Format("T:{4}, Name: {0}, Octave: {1},Length: {3} \r\n"
                    , note.name, note.octave, note.length, player.controls.currentPosition);
            addToNote(str);
            playForm.invokeNote(note);
        }
    }
}
