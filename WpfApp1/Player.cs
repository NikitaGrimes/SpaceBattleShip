using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    [Serializable]
    public class Player
    {
        public int Money;
        public int hp;
        public int energy;
        public bool DG;
        public bool battleship;
        public bool cruiser;
        public bool paladin;
        public bool serafim;
        public bool frigate;
        public int LaserAcc;
        public int LaserDam;
        public int LaserEner;
        public int KineticAcc;
        public int KineticDam;
        public int KineticEner;
        public int KineticShel;
        public int StartMoney = 2000;
        public int StartHP = 1000;
        public int StartEner = 100;
        public string Name;
        public string DateSave;

        public Player()
        {
            energy = StartEner;
            Money = StartMoney;
            hp = StartHP;
            LaserAcc = 70;
            LaserDam = 150;
            LaserEner = 50;
            KineticAcc = 50;
            KineticDam = 300;
            KineticEner = 30;
            KineticShel = 3;
            DG = battleship = cruiser = paladin = serafim = frigate = false;
        }
    }
}
