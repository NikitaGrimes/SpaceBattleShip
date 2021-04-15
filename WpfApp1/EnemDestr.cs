using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
    public class EnemDestr : Ship
    {
        public Weapon Laser;
        public FiniteWeapon Kin;
        readonly public string UriEnemDestr = "pack://application:,,,/pic/label_pactdestr.png";

        public EnemDestr ()
        {

        }

        public EnemDestr (int hp, int ener)
        {
            Laser = new Weapon();
            Laser.Name = "Laser";
            Laser.Damage = 100;
            Laser.Accuracy = 70;
            Laser.Ener = 50;
            Kin = new FiniteWeapon();
            Kin.Name = "Kinetic";
            Kin.Damage = 250;
            Kin.Accuracy = 50;
            Kin.Ener = 30;
            Kin.NumofShells = 2;
            HP = hp;
            Energy = ener;
            uri = new Uri(UriEnemDestr);
            bitmap = new BitmapImage(uri);
            IsTarget = false;
            IsAlly = false;
            IsEnemy = true;
        }

        new public void Destroy()
        {
            LHP.Visibility = Visibility.Hidden;
        }
    }
}