using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
    public class BCell
    {
        private bool isEnemy;
        private bool isAlly;
        private bool isEmpty;
        private bool isRider;
        private bool isEnemCruiser;
        private bool isEnemDestr;
        private bool isCanMove;
        public double X;
        public double Y;
        readonly public double DeltaX = 64.85;
        readonly public double DeltaY = 56.5;
        readonly public int wid = 125;
        readonly public int XLeft = -40;
        readonly public int YTop = 8;
        readonly public int XLeftexp = -45;
        readonly public int YTopexp = -15;
        readonly public int widexp = 180;
        readonly public string UriExp = "pack://application:,,,/pic/expl.png";
        public Image image;
        public Label MoveEn;

        public bool IsEnemy { get => isEnemy; set => isEnemy = value; }
        public bool IsAlly { get => isAlly; set => isAlly = value; }
        public bool IsEmpty { get => isEmpty; set => isEmpty = value; }
        public bool IsRider { get => isRider; set => isRider = value; }
        public bool IsEnemCruiser { get => isEnemCruiser; set => isEnemCruiser = value; }
        public bool IsCanMove { get => isCanMove; set => isCanMove = value; }
        public bool IsEnemDestr { get => isEnemDestr; set => isEnemDestr = value; }

        public BCell()
        {
            IsAlly = IsEnemy = IsRider = IsEnemCruiser = IsCanMove = false;
            IsEmpty = true;
        }

        public bool IsThere(double x, double y)
        {
            if (x < X + DeltaX && x > X && y > Y && y < Y + DeltaY)
                return true;
            return false;
        }

        public void AddEnemy(object _ship)
        {
            this.IsEmpty = false;
            this.IsEnemy = true;
        }

        public void Destroy()
        {
            IsAlly = IsEnemy = IsRider = IsEnemCruiser = false;
            IsEmpty = true;
            this.image.Visibility = Visibility.Hidden;
            this.image = new Image();
            this.image.Width = widexp;
            var uri = new Uri(UriExp);
            var bitmap = new BitmapImage(uri);
            this.image.Source = bitmap;
            Thickness Pos = new Thickness();
            Pos.Left = this.X + XLeftexp;
            Pos.Top = this.Y + YTopexp;
            this.image.Margin = Pos;
        }

        public void Re()
        {
            IsAlly = IsEnemy = IsRider = IsEnemCruiser = false;
            IsEmpty = true;
            image.Visibility = Visibility.Hidden;
            Thickness Pos = new Thickness();
            Pos.Left = this.X + XLeft;
            Pos.Top = this.Y + YTop;
            this.image.Margin = Pos;
            this.image.Width = wid;
        }

        public void Setmove(double EnMove, double LWid, double LSize, double LHei, double LDeltaXM)
        {
            MoveEn = new Label();
            IsCanMove = true;
            MoveEn.Content = "-" + EnMove;
            MoveEn.Width = LWid;
            MoveEn.Height = LHei;
            MoveEn.FontSize = LSize;
            MoveEn.Foreground = new SolidColorBrush(Colors.Azure);
            Thickness Pos1 = new Thickness();
            Pos1.Left = X + LDeltaXM + 10;
            Pos1.Top = Y + 5;
            MoveEn.Margin = Pos1;
        }

        public void HideMove()
        {
            if (IsCanMove && MoveEn != null)
            {
                IsCanMove = false;
                MoveEn.Visibility = Visibility.Hidden;
                MoveEn = null;
            }
        }
    }
}