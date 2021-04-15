using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
    public class PlayerShip : Ship
    {
        public FiniteWeapon DeathGun;
        public FinSalWeapon FSW1;
        public FinSalWeapon FSW2;
        public FinSalWeapon FSW3;
        public SalvoWeapon SW1;
        public SalvoWeapon SW2;
        public SalvoWeapon SW3;
        public FiniteWeapon FW1;
        public FiniteWeapon FW2;
        public FiniteWeapon FW3;
        public Weapon W1;
        public Weapon W2;
        public Weapon W3;
        public int StartEnergy = 100;
        public int StartEnMove = 30;
        public int StartHP = 1000;
        public string UriRider = "pack://application:,,,/pic/sunrider.png";
        public int wid = 125;
        public bool IsActive;

        public PlayerShip()
        {
            IsActive = false;
            DeathGun = null;
            FSW1 = FSW2 = FSW3 = null;
            SW1 = SW2 = SW3 = null;
            FW1 = FW2 = FW3 = null;
            W1 = W2 = W3 = null;
            Energy = StartEnergy;
            EnMove = StartEnMove;
            HP = StartHP;
            IsAlly = true;
            IsEnemy = false;
            var uri = new Uri(UriRider);
            var bitmap = new BitmapImage(uri);
            image = new Image();
            image.Source = bitmap;
            image.Width = wid;
        }
    }
}