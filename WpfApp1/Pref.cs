using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    [Serializable]
    public class Pref
    {
        public double KofClickVolume;
        public double KofMusicVolume;
        public double KofBattleVolume;

        public Pref()
        {
            KofClickVolume = 0.5;
            KofMusicVolume = 0.5;
            KofBattleVolume = 0.5;
        }
    }
}
