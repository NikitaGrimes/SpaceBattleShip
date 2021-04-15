using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApp1
{
    public class Weapon
    {
        public string Name;
        public string Sound;
        public int Damage;
        public int Accuracy;
        public int Ener;
        public Image image;

        public Weapon()
        {

        }

        public Weapon(string name, string sound, int dam, int acc)
        {
            Name = name;
            Sound = sound;
            Damage = dam;
            Accuracy = acc;
        }
    }
}
