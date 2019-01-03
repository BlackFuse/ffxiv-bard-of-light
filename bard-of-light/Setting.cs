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
        public static double blockHeightScale = 0.1;
        public static int windowWidth = (int)SystemParameters.PrimaryScreenWidth;
        public static int windowHeight = (int)SystemParameters.PrimaryScreenHeight - 300;
        public static int middlePosX = windowWidth / 2;
        public static double frameTime = Math.Round(1000.0f/72,3);
        public static int baseOctave = 3;
        public static double blockMovingSpeed = 200;
        public static double blockMaxHeight = windowHeight;
        public static double playDelay = Math.Round(blockMaxHeight / blockMovingSpeed * 1000.0f, 3);
        public static Dictionary<string, Keys> userKeys = new Dictionary<string, Keys>()
        {
            {"C0", Keys.A},
            {"CSharp0", Keys.A},
            {"D0", Keys.A},
            {"DSharp0", Keys.A},
            {"E0", Keys.A},
            {"F0", Keys.A},
            {"FSharp0", Keys.A},
            {"G0", Keys.A},
            {"GSharp0", Keys.A},
            {"A0", Keys.A},
            {"ASharp0", Keys.A},
            {"B0", Keys.A},

            {"C1", Keys.A},
            {"CSharp1", Keys.A},
            {"D1", Keys.A},
            {"DSharp1", Keys.A},
            {"E1", Keys.A},
            {"F1", Keys.A},
            {"FSharp1", Keys.A},
            {"G1", Keys.A},
            {"GSharp1", Keys.A},
            {"A1", Keys.A},
            {"ASharp1", Keys.A},
            {"B1", Keys.A},

            {"C2", Keys.A},
            {"CSharp2", Keys.A},
            {"D2", Keys.A},
            {"DSharp2", Keys.A},
            {"E2", Keys.A},
            {"F2", Keys.A},
            {"FSharp2", Keys.A},
            {"G2", Keys.A},
            {"GSharp2", Keys.A},
            {"A2", Keys.A},
            {"ASharp2", Keys.A},
            {"B2", Keys.A},
        };
    }
}
