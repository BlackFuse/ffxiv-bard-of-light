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
            public int currentPosY;
            public int posX;
            public bool onScreen;
            public Color color;
        }

        //private List<playingNote> playingNotes = new List<playingNote>();
        private List<playingNote> onScreenNotes = new List<playingNote>();

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

        public PlayForm(List<myNote> notes) {
            InitializeComponent();
            this.BackColor = Color.LimeGreen;
            this.TransparencyKey = Color.LimeGreen;
            this.TopMost = true;
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
            foreach (playingNote note in onScreenNotes) {
                e.Graphics.FillRectangle(new SolidBrush(note.color),
                    new Rectangle(note.posX, note.currentPosY, Setting.blockWidth, (int)note.note.length));
                increasePosY(note);
            }

            while (onScreenNotes[0].currentPosY > Setting.blockMaxHeight)
            {
                onScreenNotes.RemoveAt(0);
            }
        }

        public void invokeNote(myNote in_note) {
            playingNote newNote = new playingNote()
            {
                note = in_note,
                currentPosY = 0,
                posX = Setting.middlePosX + (noteOffset[in_note.name] + 7 * (in_note.octave - Setting.baseOctave)) * Setting.blockWidth,
                onScreen = false,
                color = Color.White
            };
            this.onScreenNotes.Add(newNote);

        }

        private void increasePosY(playingNote note){
            note.currentPosY += Setting.blockMovingSpeed;
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
                    string str = note.note.name + (note.note.octave - Setting.baseOctave).ToString();
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
