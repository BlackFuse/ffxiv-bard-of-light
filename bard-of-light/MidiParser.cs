using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Melanchall.DryWetMidi.Smf;
using Melanchall.DryWetMidi.Smf.Interaction;

namespace bard_of_light {
    public struct myNote {
        public string name;
        public int octave;
        public long time;
        public long length;
    }


    class MidiParser {
        MidiFile midiFile;

        public MidiParser(string path) {
            readMidiFile(path);
        }

        private void readMidiFile(string path) {
            this.midiFile = MidiFile.Read(path);
            ChunksCollection chunks = midiFile.Chunks;
            //midiFile
            IEnumerable<Chord> chords = midiFile.GetChords();
        }

        public List<myNote> getAllNotes() {
            List<myNote> notes = new List<myNote>();
            foreach (Note note in midiFile.GetNotes()) {
                myNote n = new myNote() {
                    name = note.NoteName.ToString(),
                    octave = note.Octave,
                    time = note.Time,
                    length = note.Length
                };
                string str = string.Format("Name: {0}, Octave: {1}, Time: {2}, Length: {3} \r\n", n.name, n.octave, n.time, n.length);
                notes.Add(n);
            }
            return notes;
        }

        //public List<TrackChunk> getAllChunk() {
        //    List<TrackChunk> chunks = new List<TrackChunk>();
        //    foreach (Chunk item in collection) {

        //    }
        //}

        



    }
}
