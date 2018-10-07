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
    public partial class Form1 : Form {

        private MidiParser midiParser;

        public Form1() {
            InitializeComponent();
        }

        private void OpenButton_Click(object sender, EventArgs e) {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                string path = openFileDialog1.FileName;
                this.midiParser = new MidiParser(path);
                List<myNote> notes = midiParser.getAllNotes();
                foreach (myNote note in notes) {
                    string str = string.Format("Name: {0}, Octave: {1}, Time: {2}, Length: {3} \r\n"
                        , note.name, note.octave, note.time, note.length);
                    addToNote(str);
                }
                
            }
        }


        private void readyToPlay() {

        }

        private void addToNote(string str) {
            this.noteBox.Text = this.noteBox.Text + str;
        }

        private void editNote(string str) {
            this.noteBox.Text = str;
        }
    }
}
