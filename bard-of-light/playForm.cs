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

        private struct playingNote {
            public myNote note;
            public int currentPosY;
            public int posX;
            public bool onScreen;
        }

        private List<playingNote> playingNotes = new List<playingNote>();

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
            foreach (myNote note in notes) {
                this.playingNotes.Add(new playingNote() {
                    note = note,
                    currentPosY = 0,
                    posX = Setting.middlePosX + noteOffset[note.name] * Setting.blockWidth,
                    onScreen = false
                });
            }
        }

        private void initPictureBox() {
            this.pictureBox.BackColor = Color.LimeGreen;
            this.pictureBox.Size = new Size(  Setting.windowWidth , Setting.windowHeight);
            this.pictureBox.Location = new Point(0, 0);
            Console.WriteLine(this.pictureBox.Size.ToString());
        }


        private void pictureBox_Paint(object sender, PaintEventArgs e) {
            foreach (playingNote note in playingNotes) {
                if (!note.onScreen) continue;
                e.Graphics.FillRectangle(new SolidBrush(Color.LightYellow),
                    new Rectangle(note.posX, note.currentPosY, Setting.blockWidth, (int)note.note.length));
            }
        }

        private void invokeNote(int noteIndex) {
            //playingNotes[noteIndex].onScreen = true;
        }
    }
}
