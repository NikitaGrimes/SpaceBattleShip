using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
    public class EnemCruiser : Ship
    {
        public Weapon W1;
        public FinSalWeapon W2;
        readonly public string UriEnemCruiser = "pack://application:,,,/pic/label_pactcruiser.png";

        public EnemCruiser()
        {

        }

        public EnemCruiser(int hp, int ener, int enmove, bool isally)
        {
            W1 = new Weapon();
            W1.Name = "Laser";
            W1.Damage = 100;
            W1.Accuracy = 70;
            W1.Ener = 50;
            W2 = new FinSalWeapon();
            W2.Name = "Missile";
            W2.NumofSalvo = 3;
            W2.NumofShells = 4;
            W2.Damage = 100;
            W2.Accuracy = 90;
            W2.Ener = 50;
            HP = hp;
            Energy = ener;
            EnMove = enmove;
            uri = new Uri(UriEnemCruiser);
            bitmap = new BitmapImage(uri);
            IsTarget = false;
            if (isally)
            {
                IsAlly = true;
                IsEnemy = false;
            }
            else
            {
                IsEnemy = true;
                IsAlly = false;
            }
        }

        new public void Destroy()
        {
            LHP.Visibility = Visibility.Hidden;
        }
    }
}
