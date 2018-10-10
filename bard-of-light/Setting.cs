using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;


namespace bard_of_light {
    class Setting {
        public static int blockWidth = 55;
        public static double blockHeightScale = 1;
        public static int windowWidth = (int)SystemParameters.PrimaryScreenWidth;
        public static int windowHeight = (int)SystemParameters.PrimaryScreenHeight - 300;
        public static int middlePosX = windowWidth / 2;
        public static int frameTime = 1000/72;
        public static int baseOctave = 3;
        public static int blockMovingSpeed = 10;
        public static int blockMaxHeight = 700;
        public static Dictionary<string, Keys> userKeys = new Dictionary<string, Keys>()
        {
            {"C1", Keys.A},
            {"CSharp2", Keys.A},
            {"D1", Keys.A},
            {"E1", Keys.A},
            {"F1", Keys.A},
            {"G1", Keys.A},
            {"A1", Keys.A},
            {"B1", Keys.A},
        };
    }
}
