using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
    public class Ship
    {
        public int HP;
        public int Energy;
        public int EnMove;
        public int Col;
        public int Str;
        public bool IsAlly;
        public bool IsEnemy;
        public Image image;
        public Uri uri;
        public BitmapImage bitmap;
        public Label LHP;
        public bool IsTarget;

        public void Destroy()
        {

        }
    }
}
