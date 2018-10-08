using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace bard_of_light {
    class Setting {
        public static int blockWidth = 55;
        public static double blockHeightScale = 1;
        public static int windowWidth = (int)SystemParameters.PrimaryScreenWidth;
        public static int windowHeight = (int)SystemParameters.PrimaryScreenHeight - 300;
        public static int middlePosX = windowWidth / 2;
        public static int frameTime = 1000/72;
        public static int baseOctave = 3; 
        
    }
}
