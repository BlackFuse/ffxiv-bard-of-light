using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bard_of_light {
    public partial class PlayForm : Form {

        private Color blackBlock = Color.Black;
        private Color whiteBlock = Color.White;
        private Color pressedColor = Color.Green;

        private struct playingNote {
            public myNote note;
            public int posX;
            public bool onScreen;
            public Color color;
            public double joinedTime;
        }

        //private List<playingNote> playingNotes = new List<playingNote>();
        private List<playingNote> onScreenNotes = new List<playingNote>();

        private Dictionary<string, double> noteOffset = new Dictionary<string, double>()
        {
            {"C", 0},
            {"CSharp", 0.6},
            {"D", 1},
            {"DSharp", 1.6},
            {"E", 2},
            {"F", 3},
            {"FSharp", 3.6},
            {"G", 4},
            {"GSharp", 4.6},
            {"A", 5},
            {"ASharp", 5.6},
            {"B", 6},
        };

        public PlayForm(List<myNote> notes) {
            InitializeComponent();
            this.BackColor = Color.LimeGreen;
            this.TransparencyKey = Color.LimeGreen;
            //this.TopMost = true;
            initPictureBox();
            //playingNotes = new List<playingNote>();
            //foreach (myNote note in notes) {
            //    this.playingNotes.Add(new playingNote() {
            //        note = note,
            //        currentPosY = 0,
            //        posX = Setting.middlePosX + (noteOffset[note.name] + 7 * (note.octave - Setting.baseOctave)) * Setting.blockWidth,
            //        onScreen = false
            //    });
            //}
        }

        private void initPictureBox() {
            this.pictureBox.BackColor = Color.LimeGreen;
            this.pictureBox.Size = new Size(  Setting.windowWidth , Setting.windowHeight);
            this.pictureBox.Location = new Point(0, 0);
            Console.WriteLine(this.pictureBox.Size.ToString());
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e) {
            //Console.WriteLine("painted" + counter.ToString());
            //counter++;
            e.Graphics.Clear(Color.LimeGreen);
            foreach (playingNote note in onScreenNotes) {
                e.Graphics.FillRectangle(new SolidBrush(note.color),
                    new Rectangle(note.posX, (int)((Form1.currentPlayedTime - note.joinedTime)*Setting.blockMovingSpeed), Setting.blockWidth, (int)(note.note.length * Setting.blockHeightScale)));

            }
            if (onScreenNotes.Count < 1) return;
            while ((Form1.currentPlayedTime - onScreenNotes[0].joinedTime) * Setting.blockMovingSpeed > Setting.blockMaxHeight)
            {
                onScreenNotes.RemoveAt(0);
            }
        }

        public void refresh() {
            pictureBox.Refresh();
        }


        public void invokeNote(myNote in_note) {
            playingNote newNote = new playingNote() {
                note = in_note,
                posX =(int)(Setting.middlePosX + (noteOffset[in_note.name] + 7 * (in_note.octave - Setting.baseOctave)) * Setting.blockWidth),
                onScreen = false,
                color = in_note.name.Length > 2 ? this.blackBlock : this.whiteBlock,
                joinedTime = Form1.currentPlayedTime,
            };
            Console.WriteLine(String.Format("middle pos is:{0}", Setting.middlePosX));
            Console.WriteLine(String.Format("{0}{3} posX is: {1}, offset: {2}",newNote.note.name,newNote.posX, (noteOffset[in_note.name] + 7 * (in_note.octave - Setting.baseOctave)),in_note.octave));
            this.onScreenNotes.Add(newNote);
            //pictureBox.Refresh();
        }

        private void playForm_KeyDown(object sender, KeyEventArgs e)
        {
            Keys key;
            Enum.TryParse(e.KeyCode.ToString() , out key);
            if (Setting.userKeys.ContainsValue(key))
            {
                
                MessageBox.Show("Form.KeyPress: '" +
                    e.KeyCode.ToString() + "' pressed.");
                //May have better way to find pressed block
                foreach (playingNote note in onScreenNotes)
                {
                    string str = note.note.name + (note.note.octave - Setting.baseOctave + 1).ToString();
                    if (Setting.userKeys[str] == key){
                        ChangeNoteColor(note, this.pressedColor);
                        break;
                    }
                }
            }
        }

        private void playForm_KeyUp(object sender, KeyEventArgs e)
        {
            Keys key;

            Enum.TryParse(e.KeyCode.ToString(), out key);
            if (Setting.userKeys.ContainsValue(key))
            {

                MessageBox.Show("Form.KeyRelease: '" +
                    e.KeyCode.ToString() + "' pressed.");
                //May have better way to find pressed block
                foreach (playingNote note in onScreenNotes)
                {
                    string str = note.note.name + (note.note.octave - Setting.baseOctave).ToString();
                    if (Setting.userKeys[str] == key)
                    {
                        
                        ChangeNoteColor(note, note.note.name.Length > 2 ? this.blackBlock : this.whiteBlock );
                        break;
                    }
                }

            }

        }



        void ChangeNoteColor(playingNote note, Color color){
            note.color = color;
        }
        
    }
}
