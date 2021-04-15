using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        readonly int Strok = 11;
        readonly int Column = 18;
        public BCell[,] cells = new BCell[11, 18];
        public int X = 0;
        public int Y = 0;
        public double DX = 64.85;
        public double DY = 56.5;
        public double FX = 156;
        public double FY = 98;
        public double SX = 188.395;
        public double SY = 154.2;
        public double ClickVolume = 0.16;
        public double KVolume = 4;
        public int WereCan = 4;
        readonly double LWid = 110;
        readonly double LHei = 50;
        readonly double LDeltaXM = -1;
        readonly double LDeltaYM = 7;
        readonly double LDeltaXL = 8;
        readonly int LSize = 24;
        public double BDeltaX = -20;
        public double BDeltaY = 2;
        public double BaseWid = 100;
        public double KMusicVolume = 2;
        PlayerShip Rider;
        Player player;
        List<EnemCruiser> EnemCruisers = new List<EnemCruiser>();
        List<EnemDestr> EnemDestrs = new List<EnemDestr>();
        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer BtimerCruiser = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer BtimerDestr = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer TimerWait = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer TimerMove = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer TimerBadEnd = new System.Windows.Threading.DispatcherTimer();
        MediaPlayer PlayerClick = new MediaPlayer();
        MediaPlayer BattleSound = new MediaPlayer();
        MediaPlayer Music = new MediaPlayer();
        MediaPlayer ExpSound = new MediaPlayer();
        MediaPlayer MisVoise = new MediaPlayer();
        public Image TBase;
        public Image PBase;
        public Image EBase;
        Pref pref = new Pref();
        readonly string uriExplosion = "../../mus/explosion.mp3";
        readonly string uriGoT = "../../mus/GoT.mp3";
        readonly string uriKomunizm = "../../mus/sssr.mp3";
        readonly string uriClick = "../../mus/click.mp3";
        readonly string uriRiderSelect = "../../mus/RiderSelect.mp3";
        readonly string uriRiderCant = "../../mus/WeCantRider.mp3";
        readonly string uriRiderTOn = "../../mus/target_locked.mp3";
        readonly string uriLaser1 = "../../mus/Laser1.mp3";
        readonly string BadEnd = "../../mus/Daydream.mp3";
        readonly string GoodEnd = "../../mus/Regium_Finale.mp3";
        readonly string DangerMusic = "../../mus/Sora.mp3";
        readonly string uriKinetic = "../../mus/destroyercannon.mp3";
        readonly string uriGravGun = "../../mus/GravGunSound.mp3";
        readonly string uriMissingAttackRider = "../../mus/AvaMissingAttack.mp3";
        string MainMusic = "../../mus/boukyou.mp3";
        readonly string Sempai = "../../mus/Sempai.mp3";
        readonly string Anime = "../../mus/Anime.mp3";
        readonly string MainPreBattleMusic = "../../mus/de_Lanna.mp3";
        readonly string MainBattleMusic = "../../mus/New_Dawn.mp3";
        readonly string WSave = "user.dat";
        readonly string BlackBack = "pack://application:,,,/pic/black.jpg";
        readonly string RedBack = "pack://application:,,,/pic/bridge_red.jpg";
        readonly string Ava_1 = "pack://application:,,,/pic/ava_1.png";
        readonly string Ava_2 = "pack://application:,,,/pic/ava_2.png";
        readonly string Ava_3 = "pack://application:,,,/pic/ava_3.png";
        readonly string Ava_4 = "pack://application:,,,/pic/ava_4.png";
        readonly string StrPB = "pack://application:,,,/pic/player_base.png";
        readonly string StrTB = "pack://application:,,,/pic/target_base.png";
        readonly string StrEB = "pack://application:,,,/pic/enem_base.png";
        public string PreSave = "prefer.dat";
        readonly string WSave1 = "user1.dat";
        readonly string WSave2 = "user2.dat";
        readonly string WSave3 = "user3.dat";
        readonly string WSave4 = "user4.dat";
        readonly string WSave5 = "user5.dat";
        readonly string WSave6 = "user6.dat";
        public bool isChits = false;
        public bool IsMovingRider = false;
        public bool PreBattle = false;
        public bool Battle = false;
        public bool CanBattle = false;
        public bool ChoiseRider = false;
        public bool IsLaserActiv = false;
        public bool IsWeaponActiv = false;
        public bool IsTargetOn = false;
        public bool IsGGUse = false;
        public bool IsTrainerMod = false;
        public bool SaveMode = false;
        public int SomeTimes = 0;
        public int NumofPic = 0;

        public MainWindow()
        {
            InitializeComponent();
            TimerBadEnd.Tick += new EventHandler(BadEndTick);
            TimerBadEnd.Interval = new TimeSpan(0, 0, 5);
            TimerMove.Tick += new EventHandler(TickMove);
            TimerMove.Interval = new TimeSpan(0, 0, 1);
            TimerWait.Tick += new EventHandler(WaitFire);
            TimerWait.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += new EventHandler(Tick);
            //создaние музыки
            BattleSound.Volume = ClickVolume * KVolume * pref.KofBattleVolume;
            ExpSound.Volume = BattleSound.Volume;
            ExpSound.Open(new Uri(uriExplosion, UriKind.RelativeOrAbsolute));
            MisVoise.Volume = BattleSound.Volume;
            MisVoise.Open(new Uri(uriMissingAttackRider, UriKind.RelativeOrAbsolute));
            MisVoise.Stop();
            ExpSound.Stop();
            PlayerClick.Open(new Uri(uriClick, UriKind.RelativeOrAbsolute));
            PlayerClick.Volume = ClickVolume * pref.KofClickVolume;
            Music.Volume = ClickVolume * KMusicVolume * pref.KofMusicVolume;
            Music.Open(new Uri(MainMusic, UriKind.RelativeOrAbsolute));
            Music.MediaEnded += new EventHandler(Music_Ret);
            Music.Play();
            //создание "баз"
            var uri1 = new Uri(StrPB);
            var bitmap1 = new BitmapImage(uri1);
            PBase = new Image();
            PBase.Source = bitmap1;
            PBase.Width = BaseWid;
            uri1 = new Uri(StrTB);
            bitmap1 = new BitmapImage(uri1);
            TBase = new Image();
            TBase.Source = bitmap1;
            TBase.Width = BaseWid;
            uri1 = new Uri(StrEB);
            bitmap1 = new BitmapImage(uri1);
            EBase = new Image();
            EBase.Source = bitmap1;
            EBase.Width = BaseWid;
            C03.Children.Add(PBase);
            C03.Children.Add(EBase);
            C03.Children.Add(TBase);
            PBase.Visibility = Visibility.Hidden;
            EBase.Visibility = Visibility.Hidden;
            TBase.Visibility = Visibility.Hidden;
            //создание поля боя
            for (int i = 0; i < Strok; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    cells[i, j] = new BCell();
                    cells[i, j].IsAlly = false;
                    cells[i, j].IsEmpty = true;
                    cells[i, j].IsEnemy = false;
                    cells[i, j].image = new Image();
                }
            }
            for (int j = 0; j < Strok; j++)
            {
                for (int i = 0; i < Column; i++)
                {
                    if (j % 2 == 0)
                    {
                        cells[j, i].X = FX + DX * i;
                        cells[j, i].Y = FY + DY * j;
                    }
                    else
                    {
                        cells[j, i].X = SX + DX * i;
                        cells[j, i].Y = SY + DY * (j - 1);
                    }
                    Thickness Pos = new Thickness();
                    Pos.Left = cells[j, i].X + cells[j, i].XLeft;
                    Pos.Top = cells[j, i].Y + cells[j, i].YTop;
                    cells[j, i].image.Margin = Pos;
                    cells[j, i].image.Width = cells[j, i].wid;
                    C03.Children.Add(cells[j, i].image);
                }
            }
            //загрузка настроек
            BinaryFormatter bin = new BinaryFormatter();
            using (Stream fstream = new FileStream(PreSave, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                pref = (Pref)bin.Deserialize(fstream);
            }
            SelM.Value = pref.KofMusicVolume * 100.0;
            SelB.Value = pref.KofBattleVolume * 100.0;
            SelC.Value = pref.KofClickVolume * 100.0;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsTrainerMod)
            {
                if (NumofPic == 1)
                {
                    TrainerLabel.Content = "КАПИТАН?!";
                    NumofPic++;
                }
                else if (NumofPic == 2)
                {
                    TrainerLabel.Content = "...";
                    NumofPic++;
                }
                else if (NumofPic == 3)
                {
                    TrainerLabel.Content = "Капитан! Очнитесь!";
                    NumofPic++;
                }
                else if (NumofPic == 4)
                {
                    var uripic = new Uri(RedBack);
                    var bitmap = new BitmapImage(uripic);
                    TrainerImage.Source = bitmap;
                    TrainerLabelName.Content = "Вы: ";
                    TrainerLabel.Content = "Арг! Как же голова трещит!";
                    NumofPic++;
                }
                else if (NumofPic == 5)
                {
                    var uripic = new Uri(Ava_1);
                    var bitmap = new BitmapImage(uripic);
                    TrainerAva.Source = bitmap;
                    TrainerAva.Visibility = Visibility.Visible;
                    TrainerLabelName.Content = "Старпом: ";
                    TrainerLabel.Content = "Вы неплохо так ударились головой. Вы помните Ваше имя?";
                    NumofPic++;
                }
                else if (NumofPic == 6)
                {
                    var uripic = new Uri(Ava_2);
                    var bitmap = new BitmapImage(uripic);
                    TrainerAva.Source = bitmap;
                    TrainerLabelName.Content = "Вы: ";
                    TrainerLabel.Content = "Моё имя...";
                    NumofPic++;
                }
                else if (NumofPic == 7)
                {
                    TrainerLabelName.Content = "Введите имя: ";
                    TrainerLabel.Content = "";
                    TrainerTextBox.Text = "";
                    TrainerTextBox.Visibility = Visibility.Visible;
                    NumofPic++;
                }
                else if (NumofPic == 9)
                {
                    AdName.Content = player.Name;
                    var uripic = new Uri(Ava_1);
                    var bitmap = new BitmapImage(uripic);
                    TrainerAva.Source = bitmap;
                    if (player.Name == "Сталин")
                    {
                        uripic = new Uri(Ava_4);
                        TrainerLabel.Content = "Служу Советскому Союзу!";
                        player.Money = 10000;
                    }
                    else if (player.Name == "Эддард")
                    {
                        TrainerLabel.Content = "Зима близко!";
                    }
                    else if (player.Name == "Никита")
                    {
                        TrainerLabel.Content = "О! Разраб пришел. Вот тебе кроны)";
                        player.Money = 200000;
                    }
                    else
                    {
                        TrainerLabel.Content = "Слишком сильно ударились...";
                    }
                    NumofPic++;
                }
                else if (NumofPic == 10)
                {
                    TrainerLabel.Content = "Но ладно. У нас есть проблемка серьезней.";
                    NumofPic++;
                }
                else if (NumofPic == 11)
                {
                    var uripic = new Uri(Ava_3);
                    var bitmap = new BitmapImage(uripic);
                    TrainerAva.Source = bitmap;
                    TrainerLabel.Content = "Наш корабль еще не развалился, но мы близки к этому!";
                    NumofPic++;
                }
                else if (NumofPic == 12)
                {
                    TrainerLabel.Content = "Мы уже использовали Грав-ядро в начале боя, и оно еще не успело остыть, но...";
                    NumofPic++;
                }
                else if (NumofPic == 13)
                {
                    var uripic = new Uri(Ava_2);
                    var bitmap = new BitmapImage(uripic);
                    TrainerAva.Source = bitmap;
                    TrainerLabel.Content = "...";
                    NumofPic++;
                }
                else if (NumofPic == 14)
                {
                    var uripic = new Uri(Ava_1);
                    var bitmap = new BitmapImage(uripic);
                    TrainerAva.Source = bitmap;
                    TrainerLabel.Content = "У нас нет энергии. Совсем.";
                    NumofPic++;
                }
                else if (NumofPic == 15)
                {
                    TrainerLabel.Content = "Нам надо использовать Грав-орудие еще раз, иначе нас уничтожат!";
                    NumofPic++;
                }
                else if (NumofPic == 16)
                {
                    TrainerLabel.Content = "Это разрушит Грав-ядро, но мы останемся вживых.";
                    NumofPic++;
                }
                else if (NumofPic == 17)
                {
                    Music.Open(new Uri(DangerMusic, UriKind.RelativeOrAbsolute));
                    Music.Play();
                    var uripic = new Uri(Ava_4);
                    var bitmap = new BitmapImage(uripic);
                    TrainerAva.Source = bitmap;
                    TrainerLabel.Content = "Удачи, " + player.Name + "!";
                    NumofPic++;
                }
                else if (NumofPic == 18)
                {
                    TrainerLabelName.Content = "Подсказка: ";
                    TrainerLabel.Content = "Скоро перед Вами окажется поле боя.";
                    NumofPic++;
                }
                else if (NumofPic == 19)
                {
                    TrainerLabel.Content = "Выберите Ваш корабль 'Райдер' (он левее), а после выберите вашу цель.";
                    NumofPic++;
                }
                else if (NumofPic == 20)
                {
                    TrainerLabel.Content = "Появится кнопка выбора оружия. Так как у Райдера закончилась энергия, доступна будет\nтолько Грав-орудие.";
                    NumofPic++;
                }
                else if (NumofPic == 21)
                {
                    TrainerLabel.Content = "После уничтожения врага Вы сможете выйти из боя в ангар.";
                    NumofPic++;
                }
                else if (NumofPic == 22)
                {
                    MakeEnemy(0, 1);
                    Rider.HP = 30;
                    Rider.Energy = 0;
                    Rider.IsActive = true;
                    int i = 5;
                    int j = 1;
                    Rider.Str = i;
                    Rider.Col = j;
                    cells[i, j].image.Source = Rider.bitmap;
                    cells[i, j].IsAlly = cells[i, j].IsRider = true;
                    cells[i, j].IsEmpty = false;
                    IsMovingRider = false;
                    cells[i, j].image.Visibility = Visibility.Visible;
                    Battle = true;
                    NumofPic++;
                    StertBattle.Visibility = Visibility.Hidden;
                    C03.Visibility = Visibility.Visible;
                    Trainer.Visibility = Visibility.Hidden;
                    RepBattle.Visibility = Visibility.Hidden;
                    Esca.Visibility = Visibility.Hidden;
                    But1.Visibility = Visibility.Hidden;
                    PreBattle = false;
                }
            }
            else if (PreBattle)//расстановка кораблей
            {
                Point p = e.GetPosition(this);
                bool Find = false;
                for (int i = 0; i < Strok; ++i)
                {
                    for (int j = 0; j < Column; ++j)
                    {
                        if (cells[i, j].IsThere(p.X, p.Y))
                        {
                            Find = true;
                            if (IsMovingRider && j < WereCan)//постановка райдера
                            {
                                Rider.IsActive = true;
                                Rider.Str = i;
                                Rider.Col = j;
                                cells[i, j].image.Source = Rider.bitmap;
                                cells[i, j].IsAlly = cells[i, j].IsRider = true;
                                cells[i, j].IsEmpty = false;
                                IsMovingRider = false;
                                cells[i, j].image.Visibility = Visibility.Visible;
                            }
                        }
                        if (Find)
                        {
                            break;
                        }
                    }
                    if (Find)
                    {
                        break;
                    }
                }
            }
            if (Battle)//битва
            {
                Point p = e.GetPosition(this);
                bool Find = false;
                for (int i = 0; i < Strok; ++i)
                {
                    for (int j = 0; j < Column; ++j)
                    {
                        if (cells[i, j].IsThere(p.X, p.Y))//если тык на сетку
                        {
                            if (cells[i, j].IsCanMove)//если тык на сетку, на которую можно переместиться
                            {
                                if (ChoiseRider && Rider.Energy >= Rider.EnMove)//перемещение на клетку
                                {
                                    X = i;
                                    Y = j;
                                    var animation = new ThicknessAnimation();
                                    Thickness Pos1 = new Thickness();
                                    Pos1.Left = cells[Rider.Str, Rider.Col].X + cells[i, j].XLeft;
                                    Pos1.Top = cells[Rider.Str, Rider.Col].Y + cells[i, j].YTop;
                                    animation.From = Pos1;
                                    Pos1.Left = cells[i, j].X + cells[i, j].XLeft;
                                    Pos1.Top = cells[i, j].Y + cells[i, j].YTop;
                                    animation.To = Pos1;
                                    animation.Duration = TimeSpan.FromSeconds(1);
                                    cells[Rider.Str, Rider.Col].image.BeginAnimation(MarginProperty ,animation);
                                    TimerMove.Start();
                                    Rider.Energy -= Rider.EnMove;
                                    ShowEn.Content = Rider.Energy;
                                    for (int i1 = 0; i1 < Strok; i1++)
                                    {
                                        for (int j1 = 0; j1 < Column; j1++)
                                        {
                                            cells[i1, j1].HideMove();
                                        }
                                    }
                                    //cells[Rider.Str, Rider.Col].Re();
                                    //Rider.Str = i;
                                    //Rider.Col = j;
                                    //cells[i, j].image.Source = Rider.bitmap;
                                    //cells[i, j].IsAlly = cells[i, j].IsRider = true;
                                    //cells[i, j].IsEmpty = false;
                                    //cells[i, j].image.Visibility = Visibility.Visible;
                                    //if (Rider.Energy >= Rider.EnMove) //повторное построение клеток хода
                                    //{
                                    //    if (i != Strok - 1 && cells[i + 1, j].IsEmpty)
                                    //    {
                                    //        cells[i + 1, j].Setmove(Rider.EnMove, LWid, LSize, LHei, LDeltaXM);
                                    //        C03.Children.Add(cells[i + 1, j].MoveEn);
                                    //    }
                                    //    if (i != 0 && cells[i - 1, j].IsEmpty)
                                    //    {
                                    //        cells[i - 1, j].Setmove(Rider.EnMove, LWid, LSize, LHei, LDeltaXM);
                                    //        C03.Children.Add(cells[i - 1, j].MoveEn);
                                    //    }
                                    //    if (j != 0 && cells[i, j - 1].IsEmpty)
                                    //    {
                                    //        cells[i, j - 1].Setmove(Rider.EnMove, LWid, LSize, LHei, LDeltaXM);
                                    //        C03.Children.Add(cells[i, j - 1].MoveEn);
                                    //    }
                                    //    if (j != Column - 1 && cells[i, j + 1].IsEmpty)
                                    //    {
                                    //        cells[i, j + 1].Setmove(Rider.EnMove, LWid, LSize, LHei, LDeltaXM);
                                    //        C03.Children.Add(cells[i, j + 1].MoveEn);
                                    //    }
                                    //    if (i % 2 == 0)
                                    //    {
                                    //        if (j != 0 && i != 0 && cells[i - 1, j - 1].IsEmpty)
                                    //        {
                                    //            cells[i - 1, j - 1].Setmove(Rider.EnMove, LWid, LSize, LHei, LDeltaXM);
                                    //            C03.Children.Add(cells[i - 1, j - 1].MoveEn);
                                    //        }
                                    //        if (j != 0 && i != Strok - 1 && cells[i + 1, j - 1].IsEmpty)
                                    //        {
                                    //            cells[i + 1, j - 1].Setmove(Rider.EnMove, LWid, LSize, LHei, LDeltaXM);
                                    //            C03.Children.Add(cells[i + 1, j - 1].MoveEn);
                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        if (j != Column - 1 && cells[i - 1, j + 1].IsEmpty)
                                    //        {
                                    //            cells[i - 1, j + 1].Setmove(Rider.EnMove, LWid, LSize, LHei, LDeltaXM);
                                    //            C03.Children.Add(cells[i - 1, j + 1].MoveEn);
                                    //        }
                                    //        if (j != Column - 1 && i != Strok - 1 && cells[i + 1, j + 1].IsEmpty)
                                    //        {
                                    //            cells[i + 1, j + 1].Setmove(Rider.EnMove, LWid, LSize, LHei, LDeltaXM);
                                    //            C03.Children.Add(cells[i + 1, j + 1].MoveEn);
                                    //        }
                                    //    }
                                    //}
                                    //Thickness Pos = new Thickness();
                                    //Pos.Left = cells[i, j].X + BDeltaX;
                                    //Pos.Top = cells[i, j].Y + BDeltaY;
                                    //PBase.Margin = Pos;
                                    PBase.Visibility = Visibility.Hidden;
                                }
                                Find = true;
                            }
                            else if (cells[i, j].IsAlly && !IsTargetOn)//построение клеток перемещения при клике на союзника
                            {
                                if (cells[i, j].IsRider)
                                {
                                    for (int i1 = 0; i1 < Strok; i1++)
                                    {
                                        for (int j1 = 0; j1 < Column; j1++)
                                        {
                                            cells[i1, j1].HideMove();
                                        }
                                    }
                                    BattleSound.Open(new Uri(uriRiderSelect, UriKind.RelativeOrAbsolute));
                                    BattleSound.Play();
                                    ShowHP.Content = Rider.HP;
                                    ShowEn.Content = Rider.Energy;
                                    ChoiseRider = true;
                                    Thickness Pos = new Thickness();
                                    Pos.Left = cells[i, j].X + BDeltaX;
                                    Pos.Top = cells[i, j].Y + BDeltaY;
                                    PBase.Margin = Pos;
                                    PBase.Visibility = Visibility.Visible;
                                    if (Rider.Energy >= Rider.EnMove)
                                    {
                                        if (i != Strok - 1 && cells[i + 1, j].IsEmpty)
                                        {
                                            cells[i + 1, j].Setmove(Rider.EnMove, LWid, LSize, LHei, LDeltaXM);
                                            C03.Children.Add(cells[i + 1, j].MoveEn);
                                        }
                                        if (i != 0 && cells[i - 1, j].IsEmpty)
                                        {
                                            cells[i - 1, j].Setmove(Rider.EnMove, LWid, LSize, LHei, LDeltaXM);
                                            C03.Children.Add(cells[i - 1, j].MoveEn);
                                        }
                                        if (j != 0 && cells[i, j - 1].IsEmpty)
                                        {
                                            cells[i, j - 1].Setmove(Rider.EnMove, LWid, LSize, LHei, LDeltaXM);
                                            C03.Children.Add(cells[i, j - 1].MoveEn);
                                        }
                                        if (j != Column - 1 && cells[i, j + 1].IsEmpty)
                                        {
                                            cells[i, j + 1].Setmove(Rider.EnMove, LWid, LSize, LHei, LDeltaXM);
                                            C03.Children.Add(cells[i, j + 1].MoveEn);
                                        }
                                        if (i % 2 == 0)
                                        {
                                            if (j != 0 && i != 0 && cells[i - 1, j - 1].IsEmpty)
                                            {
                                                cells[i - 1, j - 1].Setmove(Rider.EnMove, LWid, LSize, LHei, LDeltaXM);
                                                C03.Children.Add(cells[i - 1, j - 1].MoveEn);
                                            }
                                            if (j != 0 && i != Strok - 1 && cells[i + 1, j - 1].IsEmpty)
                                            {
                                                cells[i + 1, j - 1].Setmove(Rider.EnMove, LWid, LSize, LHei, LDeltaXM);
                                                C03.Children.Add(cells[i + 1, j - 1].MoveEn);
                                            }
                                        }
                                        else
                                        {
                                            if (j != Column - 1 && cells[i - 1, j + 1].IsEmpty)
                                            {
                                                cells[i - 1, j + 1].Setmove(Rider.EnMove, LWid, LSize, LHei, LDeltaXM);
                                                C03.Children.Add(cells[i - 1, j + 1].MoveEn);
                                            }
                                            if (j != Column - 1 && i != Strok - 1 && cells[i + 1, j + 1].IsEmpty)
                                            {
                                                cells[i + 1, j + 1].Setmove(Rider.EnMove, LWid, LSize, LHei, LDeltaXM);
                                                C03.Children.Add(cells[i + 1, j + 1].MoveEn);
                                            }
                                        }
                                    }
                                    Find = true;
                                }
                            }
                            else if (ChoiseRider && cells[i, j].IsEnemy)//захват цели
                            {
                                if (cells[i, j].IsEnemCruiser)
                                {
                                    for (int i1 = 0; i1 < Strok; i1++)
                                    {
                                        for (int j1 = 0; j1 < Column; j1++)
                                        {
                                            cells[i1, j1].HideMove();
                                        }
                                    }
                                    for (int k = 0; k < EnemDestrs.Count; k++)
                                        EnemDestrs[k].IsTarget = false;
                                    for (int k = 0; k < EnemCruisers.Count; k++)
                                        EnemCruisers[k].IsTarget = false;
                                    for (int k = 0; k < EnemCruisers.Count; k++)
                                    {
                                        if (EnemCruisers[k].Str == i && EnemCruisers[k].Col == j)
                                        {
                                            Find = true;
                                            Thickness Pos = new Thickness();
                                            Pos.Left = cells[i, j].X + BDeltaX;
                                            Pos.Top = cells[i, j].Y + BDeltaY;
                                            TBase.Margin = Pos;
                                            TBase.Visibility = Visibility.Visible;
                                            EnemCruisers[k].IsTarget = true;
                                            IsTargetOn = true;
                                            BattleSound.Open(new Uri(uriRiderTOn, UriKind.RelativeOrAbsolute));
                                            BattleSound.Play();
                                            if (!IsGGUse && player.DG)
                                                ButGG.Visibility = Visibility.Visible;
                                            if (Rider.Energy >= Rider.W1.Ener)
                                            {
                                                LabelEnerLaser.Visibility = Visibility.Visible;
                                                ButLaser.Visibility = Visibility.Visible;
                                            }
                                            if (Rider.Energy >= Rider.FW1.Ener && Rider.FW1.NumofShells > 0)
                                            {
                                                LabelEnerKin.Visibility = Visibility.Visible;
                                                LabelShellKin.Visibility = Visibility.Visible;
                                                ButKinetic.Visibility = Visibility.Visible;
                                            }
                                        }
                                        if (Find)
                                            break;
                                    }
                                }
                                else if (cells[i,j].IsEnemDestr)
                                {
                                    for (int i1 = 0; i1 < Strok; i1++)
                                    {
                                        for (int j1 = 0; j1 < Column; j1++)
                                        {
                                            cells[i1, j1].HideMove();
                                        }
                                    }
                                    for (int k = 0; k < EnemDestrs.Count; k++)
                                        EnemDestrs[k].IsTarget = false;
                                    for (int k = 0; k < EnemCruisers.Count; k++)
                                        EnemCruisers[k].IsTarget = false;
                                    for (int k = 0; k < EnemDestrs.Count; k++)
                                    {
                                        if (EnemDestrs[k].Str == i && EnemDestrs[k].Col == j)
                                        {
                                            Find = true;
                                            Thickness Pos = new Thickness();
                                            Pos.Left = cells[i, j].X + BDeltaX;
                                            Pos.Top = cells[i, j].Y + BDeltaY;
                                            TBase.Margin = Pos;
                                            TBase.Visibility = Visibility.Visible;
                                            EnemDestrs[k].IsTarget = true;
                                            IsTargetOn = true;
                                            BattleSound.Open(new Uri(uriRiderTOn, UriKind.RelativeOrAbsolute));
                                            BattleSound.Play();
                                            if (!IsGGUse && player.DG)
                                                ButGG.Visibility = Visibility.Visible;
                                            if (IsTrainerMod)
                                                ButGG.Visibility = Visibility.Visible;
                                            if (Rider.Energy >= Rider.W1.Ener)
                                            {
                                                LabelEnerLaser.Visibility = Visibility.Visible;
                                                ButLaser.Visibility = Visibility.Visible;
                                            }
                                            if (Rider.Energy >= Rider.FW1.Ener && Rider.FW1.NumofShells > 0)
                                            {
                                                LabelEnerKin.Visibility = Visibility.Visible;
                                                LabelShellKin.Visibility = Visibility.Visible;
                                                ButKinetic.Visibility = Visibility.Visible;
                                            }
                                        }
                                        if (Find)
                                            break;
                                    }
                                }
                                if (IsTrainerMod)
                                {
                                    RepBattle.Visibility = Visibility.Hidden;
                                    Esca.Visibility = Visibility.Hidden;
                                }
                            }
                            else if (ChoiseRider)//сброс выбора Райдера
                            {
                                for (int i1 = 0; i1 < Strok; i1++)
                                {
                                    for (int j1 = 0; j1 < Column; j1++)
                                    {
                                        cells[i1, j1].HideMove();
                                    }
                                }
                                for (int k = 0; k < EnemCruisers.Count; k++)
                                    EnemCruisers[k].IsTarget = false;
                                TBase.Visibility = Visibility.Hidden;
                                PBase.Visibility = Visibility.Hidden;
                                LabelEnerKin.Visibility = Visibility.Hidden;
                                LabelEnerLaser.Visibility = Visibility.Hidden;
                                LabelShellKin.Visibility = Visibility.Hidden;
                                ButLaser.Visibility = Visibility.Hidden;
                                ButKinetic.Visibility = Visibility.Hidden;
                                ButGG.Visibility = Visibility.Hidden;
                                IsTargetOn = false;
                                ChoiseRider = false;
                                ShowHP.Content = "";
                                ShowEn.Content = "";
                                Find = true;
                            }
                        }
                        if (Find)
                            break;
                    }
                    if (Find)
                    {
                        break;
                    }
                }
            }
        }

        private void TickMove(object sender, EventArgs e)
        {
            PBase.Visibility = Visibility.Visible;
            for (int i1 = 0; i1 < Strok; i1++)
            {
                for (int j1 = 0; j1 < Column; j1++)
                {
                    cells[i1, j1].HideMove();
                }
            }
            cells[Rider.Str, Rider.Col].image.Visibility = Visibility.Hidden;
            cells[Rider.Str, Rider.Col].Re();

            cells[Rider.Str, Rider.Col].image = new Image();
            cells[Rider.Str, Rider.Col].image.Visibility = Visibility.Hidden;
            Thickness Pos2 = new Thickness();
            Pos2.Left = cells[Rider.Str, Rider.Col].X + cells[Rider.Str, Rider.Col].XLeft;
            Pos2.Top = cells[Rider.Str, Rider.Col].Y + cells[Rider.Str, Rider.Col].YTop;
            cells[Rider.Str, Rider.Col].image.Margin = Pos2;
            cells[Rider.Str, Rider.Col].image.Width = cells[Rider.Str, Rider.Col].wid;
            C03.Children.Add(cells[Rider.Str, Rider.Col].image);

            Rider.Str = X;
            Rider.Col = Y;
            cells[X, Y].image.Source = Rider.bitmap;
            cells[X, Y].IsAlly = cells[X, Y].IsRider = true;
            cells[X, Y].IsEmpty = false;
            cells[X, Y].image.Visibility = Visibility.Visible;
            if (Rider.Energy >= Rider.EnMove) //повторное построение клеток хода
            {
                if (X != Strok - 1 && cells[X + 1, Y].IsEmpty)
                {
                    cells[X + 1, Y].Setmove(Rider.EnMove, LWid, LSize, LHei, LDeltaXM);
                    C03.Children.Add(cells[X + 1, Y].MoveEn);
                }
                if (X != 0 && cells[X - 1, Y].IsEmpty)
                {
                    cells[X - 1, Y].Setmove(Rider.EnMove, LWid, LSize, LHei, LDeltaXM);
                    C03.Children.Add(cells[X - 1, Y].MoveEn);
                }
                if (Y != 0 && cells[X, Y - 1].IsEmpty)
                {
                    cells[X, Y - 1].Setmove(Rider.EnMove, LWid, LSize, LHei, LDeltaXM);
                    C03.Children.Add(cells[X, Y - 1].MoveEn);
                }
                if (Y != Column - 1 && cells[X, Y + 1].IsEmpty)
                {
                    cells[X, Y + 1].Setmove(Rider.EnMove, LWid, LSize, LHei, LDeltaXM);
                    C03.Children.Add(cells[X, Y + 1].MoveEn);
                }
                if (X % 2 == 0)
                {
                    if (Y != 0 && X != 0 && cells[X - 1, Y - 1].IsEmpty)
                    {
                        cells[X - 1, Y - 1].Setmove(Rider.EnMove, LWid, LSize, LHei, LDeltaXM);
                        C03.Children.Add(cells[X - 1, Y - 1].MoveEn);
                    }
                    if (Y != 0 && X != Strok - 1 && cells[X + 1, Y - 1].IsEmpty)
                    {
                        cells[X + 1, Y - 1].Setmove(Rider.EnMove, LWid, LSize, LHei, LDeltaXM);
                        C03.Children.Add(cells[X + 1, Y - 1].MoveEn);
                    }
                }
                else
                {
                    if (Y != Column - 1 && cells[X - 1, Y + 1].IsEmpty)
                    {
                        cells[X - 1, Y + 1].Setmove(Rider.EnMove, LWid, LSize, LHei, LDeltaXM);
                        C03.Children.Add(cells[X - 1, Y + 1].MoveEn);
                    }
                    if (Y != Column - 1 && X != Strok - 1 && cells[X + 1, Y + 1].IsEmpty)
                    {
                        cells[X + 1, Y + 1].Setmove(Rider.EnMove, LWid, LSize, LHei, LDeltaXM);
                        C03.Children.Add(cells[X + 1, Y + 1].MoveEn);
                    }
                }
            }
            Thickness Pos = new Thickness();
            Pos.Left = cells[X, Y].X + BDeltaX;
            Pos.Top = cells[X, Y].Y + BDeltaY;
            PBase.Margin = Pos;
            TimerMove.Stop();
        }

        private void Tick(object sender, EventArgs e)
        {
            cells[X, Y].Re();
            timer.Stop();
        }

        private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (PreBattle)
            {
                Point p = e.GetPosition(this);
                bool Find = false;
                for (int i = 0; i < Strok; i++)
                {
                    for (int j = 0; j < Column; j++)
                    {
                        if (cells[i, j].IsThere(p.X, p.Y) && cells[i, j].IsRider)
                        {
                            Rider.IsActive = false;
                            Find = true;
                            cells[i, j].image.Visibility = Visibility.Hidden;
                            But1.Visibility = Visibility.Visible;
                            cells[i, j].IsAlly = false;
                            cells[i, j].IsEmpty = true;
                        }
                        if (Find)
                        {
                            break;
                        }
                    }
                    if (Find)
                    {
                        break;
                    }
                }
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Oem3")
            {
                if (!isChits)
                {
                    Chits.Text = "";
                    Chits.Visibility = Visibility.Visible;
                    isChits = true;
                }
                else
                {
                    Chits.Visibility = Visibility.Hidden;
                    isChits = false;
                }
            }
            if (e.Key.ToString() == "Return")
            {
                if (IsTrainerMod && NumofPic == 8)
                {
                    player.Name = TrainerTextBox.Text;
                    if (player.Name == "Сталин")
                    {
                        Music.Open(new Uri(uriKomunizm, UriKind.RelativeOrAbsolute));
                        Music.Play();
                    }
                    else if (player.Name == "Эддард")
                    {
                        Music.Open(new Uri(uriGoT, UriKind.RelativeOrAbsolute));
                        Music.Play();
                    }
                    NumofPic++;
                    TrainerTextBox.Visibility = Visibility.Hidden;
                    TrainerLabelName.Content = "Старпом: ";
                    TrainerLabel.Content = "...";
                }
                try
                {
                    if (Chits.Text == "Sempai")
                    {
                        MainMusic = Sempai;
                        Music.Open(new Uri(MainMusic, UriKind.RelativeOrAbsolute));
                        Music.Play();
                    }
                    else if (Chits.Text == "Anime")
                    {
                        MainMusic = Anime;
                        Music.Open(new Uri(MainMusic, UriKind.RelativeOrAbsolute));
                        Music.Play();
                    }
                    else if (Chits.Text == "GetMoney")
                    {
                        player.Money += 5000;
                        KronSize.Content = KronSizeShop.Content = player.Money;

                    }
                    else if (Chits.Text == "Rep")
                    {
                        Rider.HP = player.hp;
                        ShowHP.Content = Rider.HP;
                    }
                    else if (Chits.Text == "Ener")
                    {
                        Rider.Energy = player.energy;
                        ShowEn.Content = Rider.Energy;
                        if (IsTargetOn)
                        {
                            ButLaser.Visibility = Visibility.Visible;
                            if (Rider.FW1.NumofShells > 0)
                                ButKinetic.Visibility = Visibility.Visible;
                        }
                    }
                    else if (Chits.Text == "FullShell")
                    {
                        Rider.FW1.NumofShells = player.KineticShel;
                        if (Rider.Energy >= Rider.FW1.Ener)
                            ButKinetic.Visibility = Visibility.Visible;
                    }
                    else if (Chits.Text == "KillTarget")
                    {
                        if (IsTargetOn)
                        {
                            bool Find = false;
                            LabelEnerKin.Visibility = Visibility.Hidden;
                            LabelEnerLaser.Visibility = Visibility.Hidden;
                            LabelShellKin.Visibility = Visibility.Hidden;
                            for (int i = 0; i < EnemCruisers.Count; i++)
                            {
                                if (EnemCruisers[i].IsTarget)
                                {
                                    ExpSound.Stop();
                                    ExpSound.Play();
                                    Find = true;
                                    EnemCruisers[i].LHP.Content = EnemCruisers[i].HP;
                                    KillCruiser(i);
                                }
                                if (Find)
                                    break;
                            }
                            for (int i = 0; i < EnemDestrs.Count; i++)
                            {
                                if (EnemDestrs[i].IsTarget)
                                {
                                    ExpSound.Stop();
                                    ExpSound.Play();
                                    Find = true;
                                    EnemDestrs[i].LHP.Content = EnemDestrs[i].HP;
                                    KillDestr(i);
                                }
                                if (Find)
                                    break;
                            }
                            if (EnemCruisers.Count == 0 && EnemDestrs.Count == 0)
                            {
                                Music.Open(new Uri(GoodEnd, UriKind.RelativeOrAbsolute));
                                Music.Play();
                            }
                        }
                    }
                }
                catch(Exception)
                {

                }
                Chits.Visibility = Visibility.Hidden;
                isChits = false;
            }
        }
        //-------------------------------------------------3 стр----------------------------------------
        private void Button_Click_31(object sender, RoutedEventArgs e) // Райдер
        {
            PlayerClick.Stop();
            PlayerClick.Play();
            IsMovingRider = true;
            But1.Visibility = Visibility.Hidden;
        }

        private void BTickCr(object sender, EventArgs e) // ВРАЖЕСКИЕ ХОДЫ Cruisers
        {
            SomeTimes -= 1;
            if (SomeTimes < 0)
                goto l1;
            Thickness Pos = new Thickness();
            Pos.Left = cells[EnemCruisers[SomeTimes].Str, EnemCruisers[SomeTimes].Col].X + BDeltaX;
            Pos.Top = cells[EnemCruisers[SomeTimes].Str, EnemCruisers[SomeTimes].Col].Y + BDeltaY;
            EBase.Margin = Pos;
            EBase.Visibility = Visibility.Visible;
            Pos.Left = cells[Rider.Str, Rider.Col].X + BDeltaX;
            Pos.Top = cells[Rider.Str, Rider.Col].Y + BDeltaY;
            TBase.Margin = Pos;
            TBase.Visibility = Visibility.Visible;
            Random random = new Random();
            int rand;
            EnemCruisers[SomeTimes].Energy -= EnemCruisers[SomeTimes].W1.Ener;
            rand = random.Next(101);
            BattleSound.Open(new Uri(uriLaser1, UriKind.RelativeOrAbsolute));
            BattleSound.Stop();
            BattleSound.Play();
            if (rand <= EnemCruisers[SomeTimes].W1.Accuracy)
            {
                rand = random.Next(90, 111);
                double RandDamage = rand / 100.0;
                RandDamage = RandDamage * EnemCruisers[SomeTimes].W1.Damage;
                Rider.HP -= (int)RandDamage;
                if (Rider.HP <= 0)
                {
                    ExpSound.Stop();
                    ExpSound.Play();
                    BtimerCruiser.Tick -= new EventHandler(BTickCr);
                    BtimerCruiser.Stop();
                    Music.Open(new Uri(BadEnd, UriKind.RelativeOrAbsolute));
                    Music.Play();
                    ClearBattle();
                    BadEndCanvas.Visibility = Visibility.Visible;
                    TimerBadEnd.Start();
                    C02.Visibility = Visibility.Hidden;
                    SomeTimes = 0;
                    goto l1;
                }
            }
            ShowHP.Content = Rider.HP;
            if (EnemCruisers[SomeTimes].Energy >= EnemCruisers[SomeTimes].W1.Ener)
                SomeTimes++;
        l1:
            if (SomeTimes < 0)
            {
                BtimerCruiser.Stop();
                BtimerCruiser.Tick -= new EventHandler(BTickCr);
                if (EnemDestrs.Count > 0)
                {
                    SomeTimes = EnemDestrs.Count;
                    BtimerDestr.Interval = new TimeSpan(0, 0, 1);
                    BtimerDestr.Tick += new EventHandler(BTickDe);
                    BtimerDestr.Start();
                }
                else
                {
                    TBase.Visibility = Visibility.Hidden;
                    EBase.Visibility = Visibility.Hidden;
                    RepEnerEnemy();
                    Thread.Sleep(500);
                    if (Rider.HP <= player.hp / 2)
                    {
                        Music.Open(new Uri(DangerMusic, UriKind.RelativeOrAbsolute));
                        Music.Play();
                    }
                    ShowHP.Content = "";
                    ShowEn.Content = "";
                    RepBattle.Visibility = Visibility.Visible;
                    Esca.Visibility = Visibility.Visible;
                    this.IsEnabled = true;
                }
            }
        }

        private void BTickDe(object sender, EventArgs e) // ВРАЖЕСКИЕ ХОДЫ Destrs
        {
            SomeTimes -= 1;
            if (SomeTimes < 0)
                goto l2;
            Thickness Pos = new Thickness();
            Pos.Left = cells[EnemDestrs[SomeTimes].Str, EnemDestrs[SomeTimes].Col].X + BDeltaX;
            Pos.Top = cells[EnemDestrs[SomeTimes].Str, EnemDestrs[SomeTimes].Col].Y + BDeltaY;
            EBase.Margin = Pos;
            EBase.Visibility = Visibility.Visible;
            Pos.Left = cells[Rider.Str, Rider.Col].X + BDeltaX;
            Pos.Top = cells[Rider.Str, Rider.Col].Y + BDeltaY;
            TBase.Margin = Pos;
            TBase.Visibility = Visibility.Visible;
            if (EnemDestrs[SomeTimes].Kin.NumofShells > 0 && EnemDestrs[SomeTimes].Energy >= EnemDestrs[SomeTimes].Kin.Ener)
            {
                Random random = new Random();
                int rand;
                EnemDestrs[SomeTimes].Energy -= EnemDestrs[SomeTimes].Kin.Ener;
                rand = random.Next(101);
                EnemDestrs[SomeTimes].Kin.NumofShells--;
                BattleSound.Open(new Uri(uriKinetic, UriKind.RelativeOrAbsolute));
                BattleSound.Stop();
                BattleSound.Play();
                if (rand <= EnemDestrs[SomeTimes].Kin.Accuracy)
                {
                    rand = random.Next(90, 111);
                    double RandDamage = rand / 100.0;
                    RandDamage = RandDamage * EnemDestrs[SomeTimes].Kin.Damage;
                    Rider.HP -= (int)RandDamage;
                }
                ShowHP.Content = Rider.HP;
            }
            else if (EnemDestrs[SomeTimes].Energy >= EnemDestrs[SomeTimes].Laser.Ener)
            {
                Random random = new Random();
                int rand;
                EnemDestrs[SomeTimes].Energy -= EnemDestrs[SomeTimes].Laser.Ener;
                rand = random.Next(101);
                BattleSound.Open(new Uri(uriLaser1, UriKind.RelativeOrAbsolute));
                BattleSound.Stop();
                BattleSound.Play();
                if (rand <= EnemDestrs[SomeTimes].Laser.Accuracy)
                {
                    rand = random.Next(90, 111);
                    double RandDamage = rand / 100.0;
                    RandDamage = RandDamage * EnemDestrs[SomeTimes].Laser.Damage;
                    Rider.HP -= (int)RandDamage;
                }
                ShowHP.Content = Rider.HP;
            }
            if (Rider.HP <= 0)
            {
                ExpSound.Stop();
                ExpSound.Play();
                ClearBattle();
                BadEndCanvas.Visibility = Visibility.Visible;
                C02.Visibility = Visibility.Hidden;
                Music.Open(new Uri(BadEnd, UriKind.RelativeOrAbsolute));
                Music.Play();
                BtimerDestr.Tick -= new EventHandler(BTickDe);
                BtimerDestr.Stop();
                TimerBadEnd.Start();
                SomeTimes = 0;
                goto l2;
            }
            if (EnemDestrs[SomeTimes].Energy >= EnemDestrs[SomeTimes].Laser.Ener || (EnemDestrs[SomeTimes].Energy >= EnemDestrs[SomeTimes].Kin.Ener && EnemDestrs[SomeTimes].Kin.NumofShells > 0))
                SomeTimes++;
            l2:
            if (SomeTimes < 0)
            {
                TBase.Visibility = Visibility.Hidden;
                EBase.Visibility = Visibility.Hidden;
                BtimerDestr.Stop();
                RepEnerEnemy();
                Thread.Sleep(500);
                if (Rider.HP <= player.hp / 2)
                {
                    Music.Open(new Uri(DangerMusic, UriKind.RelativeOrAbsolute));
                    Music.Play();
                }
                BtimerDestr.Tick -= new EventHandler(BTickDe);
                ShowHP.Content = "";
                ShowEn.Content = "";
                RepBattle.Visibility = Visibility.Visible;
                Esca.Visibility = Visibility.Visible;
                this.IsEnabled = true;
            }
        }

        private void RepBattle_Click(object sender, RoutedEventArgs e) // КОНЕЦ ХОДА
        {
            PlayerClick.Stop();
            PlayerClick.Play();
            for (int i1 = 0; i1 < Strok; i1++)
            {
                for (int j1 = 0; j1 < Column; j1++)
                {
                    cells[i1, j1].HideMove();
                }
            }
            SomeTimes = EnemCruisers.Count;
            BtimerCruiser.Tick += new EventHandler(BTickCr);
            BtimerCruiser.Interval = new TimeSpan(0, 0, 2);
            BtimerCruiser.Start();
            Rider.Energy = player.energy;
            PBase.Visibility = Visibility.Hidden;
            TBase.Visibility = Visibility.Hidden;
            ChoiseRider = false;
            IsTargetOn = false;
            for (int i = 0; i < EnemCruisers.Count; i++)
            {
                EnemCruisers[i].IsTarget = false;
            }
            ButLaser.Visibility = Visibility.Hidden;
            RepBattle.Visibility = Visibility.Hidden;
            Esca.Visibility = Visibility.Hidden;
            ButGG.Visibility = Visibility.Hidden;
            LabelEnerKin.Visibility = Visibility.Hidden;
            LabelEnerLaser.Visibility = Visibility.Hidden;
            LabelShellKin.Visibility = Visibility.Hidden;
            this.IsEnabled = false;
        }
        
        private void ButLaser_Click(object sender, RoutedEventArgs e) // ВЫБОР ЛАЗЕРА
        {
            BattleSound.Open(new Uri(uriLaser1, UriKind.RelativeOrAbsolute));
            BattleSound.Play();
            TimerWait.Start();
            LabelEnerKin.Visibility = Visibility.Hidden;
            LabelEnerLaser.Visibility = Visibility.Hidden;
            LabelShellKin.Visibility = Visibility.Hidden;
            ButGG.Visibility = Visibility.Hidden;
            if (ChoiseRider && IsTargetOn)
            {
                bool Find = false;
                for (int i = 0; i < EnemCruisers.Count; i++)
                {
                    if (EnemCruisers[i].IsTarget)
                    {
                        ButKinetic.Visibility = Visibility.Hidden;
                        ButLaser.Visibility = Visibility.Hidden;
                        Find = true;
                        Random random = new Random();
                        int rand;
                        rand = random.Next(101);
                        Rider.Energy -= Rider.W1.Ener;
                        //if (Rider.Energy < Rider.W1.Ener)
                        //    ButLaser.Visibility = Visibility.Hidden;
                        //if (Rider.Energy < Rider.FW1.Ener)
                        //    ButKinetic.Visibility = Visibility.Hidden;
                        ShowEn.Content = Rider.Energy;
                        if (rand <= Rider.W1.Accuracy)
                        {
                            rand = random.Next(90, 111);
                            double RandDamage = rand / 100.0;
                            RandDamage = RandDamage * Rider.W1.Damage;
                            EnemCruisers[i].HP -= (int)RandDamage;
                            if (EnemCruisers[i].HP < 1000)
                            {
                                Thickness Pos = new Thickness();
                                Pos.Left = cells[EnemCruisers[i].Str, EnemCruisers[i].Col].X + LDeltaXL;
                                Pos.Top = cells[EnemCruisers[i].Str, EnemCruisers[i].Col].Y;
                                EnemCruisers[i].LHP.Margin = Pos;
                            }
                            EnemCruisers[i].LHP.Content = EnemCruisers[i].HP;
                            if (EnemCruisers[i].HP <= 0)
                            {
                                KillCruiser(i);
                            }
                        }
                        else
                        {
                            MisVoise.Stop();
                            MisVoise.Play();
                        }
                    }
                    if (Find)
                        break;
                }
                for (int i = 0; i < EnemDestrs.Count; i++)
                {
                    if (EnemDestrs[i].IsTarget)
                    {
                        ButKinetic.Visibility = Visibility.Hidden;
                        ButLaser.Visibility = Visibility.Hidden;
                        Find = true;
                        Random random = new Random();
                        int rand;
                        rand = random.Next(101);
                        Rider.Energy -= Rider.W1.Ener;
                        //if (Rider.Energy < Rider.W1.Ener)
                        //    ButLaser.Visibility = Visibility.Hidden;
                        //if (Rider.Energy < Rider.FW1.Ener)
                        //    ButKinetic.Visibility = Visibility.Hidden;
                        ShowEn.Content = Rider.Energy;
                        if (rand <= Rider.W1.Accuracy)
                        {
                            rand = random.Next(90, 111);
                            double RandDamage = rand / 100.0;
                            RandDamage = RandDamage * Rider.W1.Damage;
                            EnemDestrs[i].HP -= (int)RandDamage;
                            EnemDestrs[i].LHP.Content = EnemDestrs[i].HP;
                            if (EnemDestrs[i].HP <= 0)
                            {
                                KillDestr(i);
                            }
                        }
                        else
                        {
                            MisVoise.Stop();
                            MisVoise.Play();
                        }
                    }
                    if (Find)
                        break;
                }
                if (EnemCruisers.Count == 0 && EnemDestrs.Count == 0)
                {
                    Music.Open(new Uri(GoodEnd, UriKind.RelativeOrAbsolute));
                    Music.Play();
                }
            }
            //if (Rider.Energy < Rider.W1.Ener)
            //    ButLaser.Visibility = Visibility.Hidden;
            //if (Rider.Energy < Rider.FW1.Ener)
            //    ButKinetic.Visibility = Visibility.Hidden;
        }
        
        private void ButKinetic_Click(object sender, RoutedEventArgs e) //ВЫБОР КИНЕТИКИ
        {
            BattleSound.Open(new Uri(uriKinetic, UriKind.RelativeOrAbsolute));
            BattleSound.Play();
            LabelEnerKin.Visibility = Visibility.Hidden;
            LabelEnerLaser.Visibility = Visibility.Hidden;
            LabelShellKin.Visibility = Visibility.Hidden;
            ButGG.Visibility = Visibility.Hidden;
            TimerWait.Start();
            bool Find = false;
            for (int i = 0; i < EnemCruisers.Count; i++)
            {
                if (EnemCruisers[i].IsTarget)
                {
                    ButKinetic.Visibility = Visibility.Hidden;
                    ButLaser.Visibility = Visibility.Hidden;
                    Find = true;
                    Random random = new Random();
                    int rand;
                    rand = random.Next(101);
                    Rider.Energy -= Rider.FW1.Ener;
                    Rider.FW1.NumofShells--;
                    //if (Rider.FW1.NumofShells == 0)
                    //    ButKinetic.Visibility = Visibility.Hidden;
                    //if (Rider.Energy < Rider.W1.Ener)
                    //    ButLaser.Visibility = Visibility.Hidden;
                    //if (Rider.Energy < Rider.FW1.Ener)
                    //    ButKinetic.Visibility = Visibility.Hidden;
                    ShowEn.Content = Rider.Energy;
                    if (rand <= Rider.FW1.Accuracy)
                    {
                        rand = random.Next(90, 111);
                        double RandDamage = rand / 100.0;
                        RandDamage = RandDamage * Rider.FW1.Damage;
                        EnemCruisers[i].HP -= (int)RandDamage;
                        if (EnemCruisers[i].HP < 1000)
                        {
                            Thickness Pos = new Thickness();
                            Pos.Left = cells[EnemCruisers[i].Str, EnemCruisers[i].Col].X + LDeltaXL;
                            Pos.Top = cells[EnemCruisers[i].Str, EnemCruisers[i].Col].Y;
                            EnemCruisers[i].LHP.Margin = Pos;
                        }
                        EnemCruisers[i].LHP.Content = EnemCruisers[i].HP;
                        if (EnemCruisers[i].HP <= 0)
                        {
                            KillCruiser(i);
                        }
                    }
                    else
                    {
                        MisVoise.Stop();
                        MisVoise.Play();
                    }
                }
                if (Find)
                    break;
            }
            for (int i = 0; i < EnemDestrs.Count; i++)
            {
                if (EnemDestrs[i].IsTarget)
                {
                    ButKinetic.Visibility = Visibility.Hidden;
                    ButLaser.Visibility = Visibility.Hidden;
                    Find = true;
                    Random random = new Random();
                    int rand;
                    rand = random.Next(101);
                    Rider.Energy -= Rider.FW1.Ener;
                    Rider.FW1.NumofShells--;
                    //if (Rider.FW1.NumofShells == 0)
                    //    ButKinetic.Visibility = Visibility.Hidden;
                    //if (Rider.Energy < Rider.W1.Ener)
                    //    ButLaser.Visibility = Visibility.Hidden;
                    //if (Rider.Energy < Rider.FW1.Ener)
                    //    ButKinetic.Visibility = Visibility.Hidden;
                    ShowEn.Content = Rider.Energy;
                    if (rand <= Rider.FW1.Accuracy)
                    {
                        rand = random.Next(90, 111);
                        double RandDamage = rand / 100.0;
                        RandDamage = RandDamage * Rider.FW1.Damage;
                        EnemDestrs[i].HP -= (int)RandDamage;
                        if (EnemDestrs[i].HP < 1000)
                        {
                            Thickness Pos = new Thickness();
                            Pos.Left = cells[EnemDestrs[i].Str, EnemDestrs[i].Col].X + LDeltaXL;
                            Pos.Top = cells[EnemDestrs[i].Str, EnemDestrs[i].Col].Y;
                            EnemDestrs[i].LHP.Margin = Pos;
                        }
                        EnemDestrs[i].LHP.Content = EnemDestrs[i].HP;
                        if (EnemDestrs[i].HP <= 0)
                        {
                            KillDestr(i);
                        }
                    }
                    else
                    {
                        MisVoise.Stop();
                        MisVoise.Play();
                    }
                }
                if (Find)
                    break;
            }
            LabelShellKin.Content = Rider.FW1.NumofShells;
            if (EnemCruisers.Count == 0 && EnemDestrs.Count == 0)
            {
                Music.Open(new Uri(GoodEnd, UriKind.RelativeOrAbsolute));
                Music.Play();
            }
        }

        private void ButGG_Click(object sender, RoutedEventArgs e) //ВЫБОР ГРАВПУШКИ
        {
            BattleSound.Open(new Uri(uriGravGun, UriKind.RelativeOrAbsolute));
            BattleSound.Play();
            IsGGUse = true;
            LabelEnerKin.Visibility = Visibility.Hidden;
            LabelEnerLaser.Visibility = Visibility.Hidden;
            LabelShellKin.Visibility = Visibility.Hidden;
            ButGG.Visibility = Visibility.Hidden;
            ButLaser.Visibility = Visibility.Hidden;
            ButKinetic.Visibility = Visibility.Hidden;
            TimerWait.Start();
            if (IsTrainerMod)
            {
                IsTrainerMod = false;
                Esca.Visibility = Visibility.Visible;
            }
            bool Find = false;
            for (int i = 0; i < EnemCruisers.Count; i++)
            {
                if (EnemCruisers[i].IsTarget)
                {
                    ButKinetic.Visibility = Visibility.Hidden;
                    ButLaser.Visibility = Visibility.Hidden;
                    EnemCruisers[i].HP = 0;
                    KillCruiser(i);
                }
                if (Find)
                    break;
            }
            for (int i = 0; i < EnemDestrs.Count; i++)
            {
                if (EnemDestrs[i].IsTarget)
                {
                    ButKinetic.Visibility = Visibility.Hidden;
                    ButLaser.Visibility = Visibility.Hidden;
                    Find = true;
                    EnemDestrs[i].HP = 0;
                    KillDestr(i);
                    break;
                }
            }
            if (EnemCruisers.Count == 0 && EnemDestrs.Count == 0)
            {
                Music.Open(new Uri(GoodEnd, UriKind.RelativeOrAbsolute));
                Music.Play();
            }
        }

        private void WaitFire(object sender, EventArgs e)
        {
            if (IsTargetOn)
            {
                if (Rider.Energy >= Rider.W1.Ener)
                {
                    LabelEnerLaser.Visibility = Visibility.Visible;
                    ButLaser.Visibility = Visibility.Visible;
                }
                if (Rider.Energy >= Rider.FW1.Ener && Rider.FW1.NumofShells > 0)
                {
                    LabelEnerKin.Visibility = Visibility.Visible;
                    LabelShellKin.Visibility = Visibility.Visible;
                    ButKinetic.Visibility = Visibility.Visible;
                }
                if (!IsGGUse)
                    ButGG.Visibility = Visibility.Visible;
            }
            TimerWait.Stop();
        }

        private void Esca_Click_32(object sender, RoutedEventArgs e) // СБЕЖАТЬ
        {
            PlayerClick.Stop();
            PlayerClick.Play();
            NumofPic = 0;
            IsTrainerMod = false;
            bool Can = true;
            for (int i = 0; i < Strok; i++)
            {
                for (int j = WereCan; j < Column; j++)
                {
                    if (cells[i, j].IsAlly)
                    {
                        Can = false;
                        BattleSound.Open(new Uri(uriRiderCant, UriKind.RelativeOrAbsolute));
                        BattleSound.Play();
                    }
                    if (!Can)
                        break;
                }
                if (!Can)
                    break;
            }
            if (Can)
            {
                Music.Open(new Uri(MainMusic, UriKind.RelativeOrAbsolute));
                Music.Play();
                IsGGUse = false;
                ClearBattle();
            }
        }

        private void StertBattle_Click(object sender, RoutedEventArgs e) // В БОЙ
        {
            PlayerClick.Stop();
            PlayerClick.Play();
            Music.Open(new Uri(MainBattleMusic, UriKind.RelativeOrAbsolute));
            Music.Play();
            if (Rider.IsActive)
            {
                Battle = true;
                PreBattle = false;
                IsMovingRider = false;
                StertBattle.Visibility = Visibility.Hidden;
                RepBattle.Visibility = Visibility.Visible;
            }
            else
                MessageBox.Show("Поставьте Райдера на поле битвы!");
        }
        //------------------------------------------------------2 стр--------------------------------------------
        private void Button_Click_21(object sender, RoutedEventArgs e) // В БОЙ
        {
            PlayerClick.Stop();
            PlayerClick.Play();
            LabelEnerLaser.Content = player.LaserEner + "En";
            LabelEnerKin.Content = player.KineticEner + "En";
            LabelShellKin.Content = player.KineticShel;
            CBattle.Visibility = Visibility.Visible;
            C02.Visibility = Visibility.Hidden;
            for (int i1 = 0; i1 < Strok; i1++)
            {
                for (int j1 = 0; j1 < Column; j1++)
                {
                    cells[i1, j1].HideMove();
                }
            }
        }

        private void Button_Char(object sender, RoutedEventArgs e) // ХАРАКТЕРИСТИКИ РАЙДЕРА
        {
            PlayerClick.Stop();
            PlayerClick.Play();
            LCRiderHP.Content = player.hp;
            LCRiderEn.Content = player.energy;
            LCShellKin.Content = player.KineticShel;
            LCEnKin.Content = player.KineticEner;
            LCDamKin.Content = player.KineticDam;
            LCAccKin.Content = player.KineticAcc;
            LCEnLas.Content = player.LaserEner;
            LCDamLas.Content = player.LaserDam;
            LCAccLas.Content = player.LaserAcc;
            C02.Visibility = Visibility.Hidden;
            CharRider.Visibility = Visibility.Visible;
        }

        private void Button_Click_22(object sender, RoutedEventArgs e) // МАГАЗИН
        {
            PlayerClick.Stop();
            PlayerClick.Play();
            if (player.DG)
                ButGravCore.Content = "SOLD";
            else ButGravCore.Content = "Грав-ядро";
            if (player.KineticShel == 10)
                ButKinShell.Content = "MAX";
            if (player.hp == 2500)
                ButHP.Content = "MAX";
            if (player.energy == 200)
                ButEn.Content = "MAX";
            if (player.KineticAcc == 80)
                ButKinAcc.Content = "MAX";
            if (player.KineticDam == 600)
                ButKinDam.Content = "MAX";
            if (player.KineticEner == 20)
                ButKinEn.Content = "MAX";
            if (player.LaserAcc == 95)
                ButLaserAcc.Content = "MAX";
            if (player.LaserDam == 300)
                ButLaserDam.Content = "MAX";
            if (player.LaserEner == 30)
                ButLaserEn.Content = "MAX";
            KronSizeShop.Content = player.Money;
            Shop.Visibility = Visibility.Visible;
            C02.Visibility = Visibility.Hidden;
        }

        private void Button_Click_23(object sender, RoutedEventArgs e) // СОХРАНЕНИЕ
        {
            PlayerClick.Stop();
            PlayerClick.Play();
            CanvaLoadSave.Visibility = Visibility.Visible;
            C02.Visibility = Visibility.Hidden;
            SaveMode = true;
            MakeDelButAndSave();
        }

        private void Button_Click_24(object sender, RoutedEventArgs e) // В МЕНЮ
        {
            PlayerClick.Stop();
            PlayerClick.Play();
            C02.Visibility = Visibility.Hidden;
            C01.Visibility = Visibility.Visible;
        }
        //---------------------------------------------------1 стр---------------------------------------------------
        private void Button_Click_11(object sender, RoutedEventArgs e) // НАЧАТЬ
        {
            PlayerClick.Stop();
            PlayerClick.Play();
            ButHP.Content = "+100HP";
            ButEn.Content = "+20 Энергии";
            ButKinShell.Content = "+1 залп Кинетики";
            ButKinAcc.Content = "+5 точности Кинетики";
            ButKinDam.Content = "+30 урона Кинетики";
            ButKinEn.Content = "-5 энергии Кинетики";
            ButLaserAcc.Content = "+5 точности Лазера";
            ButLaserDam.Content = "+30 урона Лазера";
            ButLaserEn.Content = "-5 энергии Лазера";
            player = new Player();
            KronSize.Content = player.Money;
            Rider = new PlayerShip();
            Rider.W1 = new Weapon();
            Rider.W1.Accuracy = player.LaserAcc;
            Rider.W1.Damage = player.LaserDam;
            Rider.W1.Ener = player.LaserEner;
            Rider.W1.Name = "Laser";
            Rider.FW1 = new FiniteWeapon();
            Rider.FW1.Accuracy = player.KineticAcc;
            Rider.FW1.Damage = player.KineticDam;
            Rider.FW1.Ener = player.KineticEner;
            Rider.FW1.Name = "Kinetic";
            Rider.FW1.NumofShells = player.KineticShel;
            var uri = new Uri(Rider.UriRider);
            var bitmap = new BitmapImage(uri);
            Rider.bitmap = bitmap;
            Rider.uri = uri;
            C01.Visibility = Visibility.Hidden;
            Trainer.Visibility = Visibility.Visible;
            Music.Stop();
            IsTrainerMod = true;
            TrainerLabelName.Content = "Старпом: ";
            TrainerLabel.Content = "Капитан?";
            NumofPic++;
            TrainerAva.Visibility = Visibility.Hidden;
            var uripic = new Uri(BlackBack);
            var bitmap1 = new BitmapImage(uripic);
            TrainerImage.Source = bitmap1;
        }

        private void Button_Click12(object sender, RoutedEventArgs e) // ЗАГРУЗИТЬ
        {
            PlayerClick.Stop();
            PlayerClick.Play();
            SaveMode = false;
            CanvaLoadSave.Visibility = Visibility.Visible;
            C01.Visibility = Visibility.Hidden;
            MakeDelButAndSave();
        }

        private void Button_Click13(object sender, RoutedEventArgs e) // НАСТРОЙКИ
        {
            PlayerClick.Stop();
            PlayerClick.Play();
            C01.Visibility = Visibility.Hidden;
            Preferences.Visibility = Visibility.Visible;
        }

        private void Button_Click14(object sender, RoutedEventArgs e) // ЗАКРЫТЬ
        {
            PlayerClick.Stop();
            PlayerClick.Play();
            this.Close();
        }

        private void Music_Ret(object sender, EventArgs e) // событие для повтора музыки
        {
            Music.Position = TimeSpan.Zero;
            Music.Play();
        }
        //-------------------------------------------настройки------------------------------------------------------
        private void Slider_ValueChangedMusic(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ((Slider)sender).SelectionEnd = e.NewValue;
            pref.KofMusicVolume = e.NewValue / 100.0;
            Music.Volume = ClickVolume * KMusicVolume * pref.KofMusicVolume;
        }

        private void Slider_ValueChangedBattle(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ((Slider)sender).SelectionEnd = e.NewValue;
            pref.KofBattleVolume = e.NewValue / 100.0;
            BattleSound.Volume = ClickVolume * KMusicVolume * pref.KofBattleVolume;
            ExpSound.Volume = MisVoise.Volume = BattleSound.Volume;
        }

        private void Slider_ValueChangedClick(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ((Slider)sender).SelectionEnd = e.NewValue;
            pref.KofClickVolume = e.NewValue / 100.0;
            PlayerClick.Volume = ClickVolume * KMusicVolume * pref.KofClickVolume;
        }

        private void PreferSave(object sender, RoutedEventArgs e)
        {
            PlayerClick.Stop();
            PlayerClick.Play();
            BinaryFormatter bin = new BinaryFormatter();
            using (Stream fstream = new FileStream(PreSave, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                bin.Serialize(fstream, pref);
            }
            Preferences.Visibility = Visibility.Hidden;
            C01.Visibility = Visibility.Visible;
        }
        //--------------------------------------------SHOP----------------------------------------------------------
        private void Button_Shop_HP(object sender, RoutedEventArgs e)
        {
            if (player.Money >= 400 && player.hp < 2500)
            {
                PlayerClick.Stop();
                PlayerClick.Play();
                player.Money -= 400;
                player.hp += 100;
                KronSizeShop.Content = player.Money;
            }
            if (player.hp == 2500)
            {
                ButHP.Content = "MAX";
            }
        }

        private void Button_Shop_Shell_Kinetic(object sender, RoutedEventArgs e)
        {
            if (player.Money >= 1000 && player.KineticShel < 10)
            {
                PlayerClick.Stop();
                PlayerClick.Play();
                player.Money -= 1000;
                player.KineticShel++;
                KronSizeShop.Content = player.Money;
            }
            if (player.KineticShel == 10)
            {
                ButKinShell.Content = "MAX";
            }
        }

        private void Button_Shop_Acc_Kinetic(object sender, RoutedEventArgs e)
        {
            if (player.Money >= 1000 && player.KineticAcc < 80)
            {
                PlayerClick.Stop();
                PlayerClick.Play();
                player.Money -= 1000;
                player.KineticAcc += 5;
                KronSizeShop.Content = player.Money;
            }
            if (player.KineticAcc == 80)
            {
                ButKinAcc.Content = "MAX";
            }
        }

        private void Button_Shop_Dam_Kinetic(object sender, RoutedEventArgs e)
        {
            if (player.Money >= 1000 && player.KineticDam < 600)
            {
                PlayerClick.Stop();
                PlayerClick.Play();
                player.Money -= 1000;
                player.KineticDam += 30;
                KronSizeShop.Content = player.Money;
            }
            if (player.KineticDam == 600)
            {
                ButKinDam.Content = "MAX";
            }
        }

        private void Button_Shop_En_Kinetic(object sender, RoutedEventArgs e)
        {
            if (player.Money >= 1000 && player.KineticEner > 20)
            {
                PlayerClick.Stop();
                PlayerClick.Play();
                player.Money -= 1000;
                player.KineticEner -= 5;
                KronSizeShop.Content = player.Money;
            }
            if (player.KineticEner == 20)
            {
                ButKinEn.Content = "MAX";
            }
        }

        private void Button_Shop_Acc_Laser(object sender, RoutedEventArgs e)
        {
            if (player.Money >= 1000 && player.LaserAcc < 95)
            {
                PlayerClick.Stop();
                PlayerClick.Play();
                player.Money -= 1000;
                player.LaserAcc += 5;
                KronSizeShop.Content = player.Money;
            }
            if (player.LaserAcc == 95)
            {
                ButLaserAcc.Content = "MAX";
            }
        }

        private void Button_Shop_Dam_Laser(object sender, RoutedEventArgs e)
        {
            if (player.Money >= 1000 && player.LaserDam < 300)
            {
                PlayerClick.Stop();
                PlayerClick.Play();
                player.Money -= 1000;
                player.LaserDam += 30;
                KronSizeShop.Content = player.Money;
            }
            if (player.LaserDam == 300)
            {
                ButLaserDam.Content = "MAX";
            }
        }

        private void Button_Shop_En_Laser(object sender, RoutedEventArgs e)
        {
            if (player.Money >= 1000 && player.LaserEner > 30)
            {
                PlayerClick.Stop();
                PlayerClick.Play();
                player.Money -= 1000;
                player.LaserEner -= 5;
                KronSizeShop.Content = player.Money;
            }
            if (player.LaserEner == 30)
            {
                ButLaserEn.Content = "MAX";
            }
        }

        private void Button_Shop_En(object sender, RoutedEventArgs e)
        {
            if (player.Money >= 500 && player.energy < 200)
            {
                PlayerClick.Stop();
                PlayerClick.Play();
                player.Money -= 500;
                player.energy += 20;
                KronSizeShop.Content = player.Money;
            }
            if (player.energy == 200)
            {
                ButEn.Content = "MAX";
            }
        }

        private void Button_Shop_Grav_Core(object sender, RoutedEventArgs e)
        {
            if (player.Money >= 50000)
            {
                PlayerClick.Stop();
                PlayerClick.Play();
                player.DG = true;
                player.Money -= 50000;
                KronSizeShop.Content = player.Money;
                ButGravCore.Content = "SOLD";
            }
        }

        private void Shop_Back(object sender, RoutedEventArgs e)
        {
            PlayerClick.Stop();
            PlayerClick.Play();
            C02.Visibility = Visibility.Visible;
            Shop.Visibility = Visibility.Hidden;
            KronSize.Content = player.Money;
        }
        //---------------------------------Char Rider-----------------------------------------------
        private void Char_Back(object sender, RoutedEventArgs e)
        {
            PlayerClick.Stop();
            PlayerClick.Play();
            CharRider.Visibility = Visibility.Hidden;
            C02.Visibility = Visibility.Visible;
        }
        //-------------------------------------LVL--------------------------------------------------
        private void CLVL1(object sender, RoutedEventArgs e)
        {
            PlayerClick.Stop();
            PlayerClick.Play();
            MakeEnemy(1, 2);
        }

        private void CLVL2(object sender, RoutedEventArgs e)
        {
            PlayerClick.Stop();
            PlayerClick.Play();
            MakeEnemy(3, 1);
        }

        private void CLVL3(object sender, RoutedEventArgs e)
        {
            PlayerClick.Stop();
            PlayerClick.Play();
            MakeEnemy(5, 0);
        }

        private void BackLVL(object sender, RoutedEventArgs e)
        {
            PlayerClick.Stop();
            PlayerClick.Play();
            C02.Visibility = Visibility.Visible;
            CBattle.Visibility = Visibility.Hidden;
        }
        //------------------------------------Load/Save---------------------------------------------------
        private void LoSa1_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            PlayerClick.Stop();
            PlayerClick.Play();
            if (SaveMode)
            {
                DateTime date = DateTime.Now;
                player.DateSave = date.ToString();
                BinaryFormatter bin = new BinaryFormatter();
                using (Stream fstream = new FileStream(WSave1, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    bin.Serialize(fstream, player);
                }
                LoSa1.Content = player.Name + "\n" + player.DateSave;
                DelLoSa1.Visibility = Visibility.Visible;
            }
            else
            {
                player = new Player();
                BinaryFormatter bin = new BinaryFormatter();
                using (Stream fstream = new FileStream(WSave1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    player = (Player)bin.Deserialize(fstream);
                }
                KronSize.Content = player.Money;
                Rider = new PlayerShip();
                Rider.W1 = new Weapon();
                Rider.W1.Accuracy = player.LaserAcc;
                Rider.W1.Damage = player.LaserDam;
                Rider.W1.Ener = player.LaserEner;
                Rider.W1.Name = "Laser";
                Rider.FW1 = new FiniteWeapon();
                Rider.FW1.Accuracy = player.KineticAcc;
                Rider.FW1.Damage = player.KineticDam;
                Rider.FW1.Ener = player.KineticEner;
                Rider.FW1.Name = "Kinetic";
                Rider.FW1.NumofShells = player.KineticShel;
                var uri = new Uri(Rider.UriRider);
                var bitmap = new BitmapImage(uri);
                Rider.bitmap = bitmap;
                Rider.uri = uri;
                C02.Visibility = Visibility.Visible;
                CanvaLoadSave.Visibility = Visibility.Hidden;
                AdName.Content = player.Name;
            }
        }

        private void LoSa2_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            PlayerClick.Stop();
            PlayerClick.Play();
            if (SaveMode)
            {
                DateTime date = DateTime.Now;
                player.DateSave = date.ToString();
                BinaryFormatter bin = new BinaryFormatter();
                using (Stream fstream = new FileStream(WSave2, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    bin.Serialize(fstream, player);
                }
                LoSa2.Content = player.Name + "\n" + player.DateSave;
                DelLoSa2.Visibility = Visibility.Visible;
            }
            else
            {
                player = new Player();
                BinaryFormatter bin = new BinaryFormatter();
                using (Stream fstream = new FileStream(WSave2, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    player = (Player)bin.Deserialize(fstream);
                }
                KronSize.Content = player.Money;
                Rider = new PlayerShip();
                Rider.W1 = new Weapon();
                Rider.W1.Accuracy = player.LaserAcc;
                Rider.W1.Damage = player.LaserDam;
                Rider.W1.Ener = player.LaserEner;
                Rider.W1.Name = "Laser";
                Rider.FW1 = new FiniteWeapon();
                Rider.FW1.Accuracy = player.KineticAcc;
                Rider.FW1.Damage = player.KineticDam;
                Rider.FW1.Ener = player.KineticEner;
                Rider.FW1.Name = "Kinetic";
                Rider.FW1.NumofShells = player.KineticShel;
                var uri = new Uri(Rider.UriRider);
                var bitmap = new BitmapImage(uri);
                Rider.bitmap = bitmap;
                Rider.uri = uri;
                C02.Visibility = Visibility.Visible;
                CanvaLoadSave.Visibility = Visibility.Hidden;
                AdName.Content = player.Name;
            }
        }

        private void LoSa3_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            PlayerClick.Stop();
            PlayerClick.Play();
            if (SaveMode)
            {
                DateTime date = DateTime.Now;
                player.DateSave = date.ToString();
                BinaryFormatter bin = new BinaryFormatter();
                using (Stream fstream = new FileStream(WSave3, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    bin.Serialize(fstream, player);
                }
                LoSa3.Content = player.Name + "\n" + player.DateSave;
                DelLoSa3.Visibility = Visibility.Visible;
            }
            else
            {
                player = new Player();
                BinaryFormatter bin = new BinaryFormatter();
                using (Stream fstream = new FileStream(WSave3, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    player = (Player)bin.Deserialize(fstream);
                }
                KronSize.Content = player.Money;
                Rider = new PlayerShip();
                Rider.W1 = new Weapon();
                Rider.W1.Accuracy = player.LaserAcc;
                Rider.W1.Damage = player.LaserDam;
                Rider.W1.Ener = player.LaserEner;
                Rider.W1.Name = "Laser";
                Rider.FW1 = new FiniteWeapon();
                Rider.FW1.Accuracy = player.KineticAcc;
                Rider.FW1.Damage = player.KineticDam;
                Rider.FW1.Ener = player.KineticEner;
                Rider.FW1.Name = "Kinetic";
                Rider.FW1.NumofShells = player.KineticShel;
                var uri = new Uri(Rider.UriRider);
                var bitmap = new BitmapImage(uri);
                Rider.bitmap = bitmap;
                Rider.uri = uri;
                C02.Visibility = Visibility.Visible;
                CanvaLoadSave.Visibility = Visibility.Hidden;
                AdName.Content = player.Name;
            }
        }

        private void LoSa4_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            PlayerClick.Stop();
            PlayerClick.Play();
            if (SaveMode)
            {
                DateTime date = DateTime.Now;
                player.DateSave = date.ToString();
                BinaryFormatter bin = new BinaryFormatter();
                using (Stream fstream = new FileStream(WSave4, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    bin.Serialize(fstream, player);
                }
                LoSa4.Content = player.Name + "\n" + player.DateSave;
                DelLoSa4.Visibility = Visibility.Visible;
            }
            else
            {
                player = new Player();
                BinaryFormatter bin = new BinaryFormatter();
                using (Stream fstream = new FileStream(WSave4, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    player = (Player)bin.Deserialize(fstream);
                }
                KronSize.Content = player.Money;
                Rider = new PlayerShip();
                Rider.W1 = new Weapon();
                Rider.W1.Accuracy = player.LaserAcc;
                Rider.W1.Damage = player.LaserDam;
                Rider.W1.Ener = player.LaserEner;
                Rider.W1.Name = "Laser";
                Rider.FW1 = new FiniteWeapon();
                Rider.FW1.Accuracy = player.KineticAcc;
                Rider.FW1.Damage = player.KineticDam;
                Rider.FW1.Ener = player.KineticEner;
                Rider.FW1.Name = "Kinetic";
                Rider.FW1.NumofShells = player.KineticShel;
                var uri = new Uri(Rider.UriRider);
                var bitmap = new BitmapImage(uri);
                Rider.bitmap = bitmap;
                Rider.uri = uri;
                C02.Visibility = Visibility.Visible;
                CanvaLoadSave.Visibility = Visibility.Hidden;
                AdName.Content = player.Name;
            }
        }

        private void LoSa5_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            PlayerClick.Stop();
            PlayerClick.Play();
            if (SaveMode)
            {
                DateTime date = DateTime.Now;
                player.DateSave = date.ToString();
                BinaryFormatter bin = new BinaryFormatter();
                using (Stream fstream = new FileStream(WSave5, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    bin.Serialize(fstream, player);
                }
                LoSa5.Content = player.Name + "\n" + player.DateSave;
                DelLoSa5.Visibility = Visibility.Visible;
            }
            else
            {
                player = new Player();
                BinaryFormatter bin = new BinaryFormatter();
                using (Stream fstream = new FileStream(WSave5, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    player = (Player)bin.Deserialize(fstream);
                }
                KronSize.Content = player.Money;
                Rider = new PlayerShip();
                Rider.W1 = new Weapon();
                Rider.W1.Accuracy = player.LaserAcc;
                Rider.W1.Damage = player.LaserDam;
                Rider.W1.Ener = player.LaserEner;
                Rider.W1.Name = "Laser";
                Rider.FW1 = new FiniteWeapon();
                Rider.FW1.Accuracy = player.KineticAcc;
                Rider.FW1.Damage = player.KineticDam;
                Rider.FW1.Ener = player.KineticEner;
                Rider.FW1.Name = "Kinetic";
                Rider.FW1.NumofShells = player.KineticShel;
                var uri = new Uri(Rider.UriRider);
                var bitmap = new BitmapImage(uri);
                Rider.bitmap = bitmap;
                Rider.uri = uri;
                C02.Visibility = Visibility.Visible;
                CanvaLoadSave.Visibility = Visibility.Hidden;
                AdName.Content = player.Name;
            }
        }

        private void LoSa6_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            PlayerClick.Stop();
            PlayerClick.Play();
            if (SaveMode)
            {
                DateTime date = DateTime.Now;
                player.DateSave = date.ToString();
                BinaryFormatter bin = new BinaryFormatter();
                using (Stream fstream = new FileStream(WSave6, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    bin.Serialize(fstream, player);
                }
                DelLoSa6.Visibility = Visibility.Visible;
                LoSa6.Content = player.Name + "\n" + player.DateSave;
            }
            else
            {
                player = new Player();
                BinaryFormatter bin = new BinaryFormatter();
                using (Stream fstream = new FileStream(WSave6, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    player = (Player)bin.Deserialize(fstream);
                }
                KronSize.Content = player.Money;
                Rider = new PlayerShip();
                Rider.W1 = new Weapon();
                Rider.W1.Accuracy = player.LaserAcc;
                Rider.W1.Damage = player.LaserDam;
                Rider.W1.Ener = player.LaserEner;
                Rider.W1.Name = "Laser";
                Rider.FW1 = new FiniteWeapon();
                Rider.FW1.Accuracy = player.KineticAcc;
                Rider.FW1.Damage = player.KineticDam;
                Rider.FW1.Ener = player.KineticEner;
                Rider.FW1.Name = "Kinetic";
                Rider.FW1.NumofShells = player.KineticShel;
                var uri = new Uri(Rider.UriRider);
                var bitmap = new BitmapImage(uri);
                Rider.bitmap = bitmap;
                Rider.uri = uri;
                C02.Visibility = Visibility.Visible;
                CanvaLoadSave.Visibility = Visibility.Hidden;
                AdName.Content = player.Name;
            }
        }

        private void DelLoSa1_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            PlayerClick.Stop();
            PlayerClick.Play();
            File.Delete(WSave1);
            LoSa1.Content = "Сохранения нет";
            DelLoSa1.Visibility = Visibility.Hidden;
        }

        private void DelLoSa2_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            PlayerClick.Stop();
            PlayerClick.Play();
            File.Delete(WSave2);
            LoSa2.Content = "Сохранения нет";
            DelLoSa2.Visibility = Visibility.Hidden;
        }

        private void DelLoSa3_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            PlayerClick.Stop();
            PlayerClick.Play();
            File.Delete(WSave3);
            LoSa3.Content = "Сохранения нет";
            DelLoSa3.Visibility = Visibility.Hidden;
        }

        private void DelLoSa4_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            PlayerClick.Stop();
            PlayerClick.Play();
            File.Delete(WSave4);
            LoSa4.Content = "Сохранения нет";
            DelLoSa4.Visibility = Visibility.Hidden;
        }

        private void DelLoSa5_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            PlayerClick.Stop();
            PlayerClick.Play();
            File.Delete(WSave5);
            LoSa5.Content = "Сохранения нет";
            DelLoSa5.Visibility = Visibility.Hidden;
        }

        private void DelLoSa6_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            PlayerClick.Stop();
            PlayerClick.Play();
            File.Delete(WSave6);
            LoSa6.Content = "Сохранения нет";
            DelLoSa6.Visibility = Visibility.Hidden;
        }

        private void LoSaExit_Click(object sender, RoutedEventArgs e)
        {
            PlayerClick.Stop();
            PlayerClick.Play();
            if (SaveMode)
                C02.Visibility = Visibility.Visible;
            else
                C01.Visibility = Visibility.Visible;
            CanvaLoadSave.Visibility = Visibility.Hidden;
        }
        //-------------------------------------------------------------------------------------------
        private void MakeEnemy(int Cr, int Des)
        {
            C03.Visibility = Visibility.Visible;
            CBattle.Visibility = Visibility.Hidden;
            PBase.Visibility = Visibility.Hidden;
            TBase.Visibility = Visibility.Hidden;
            EBase.Visibility = Visibility.Hidden;
            C02.Visibility = Visibility.Hidden;
            But1.Visibility = Visibility.Visible;
            StertBattle.Visibility = Visibility.Visible;
            ButLaser.Visibility = Visibility.Hidden;
            Music.Open(new Uri(MainPreBattleMusic, UriKind.RelativeOrAbsolute));
            Music.Play();
            Rider.HP = player.hp;
            Rider.Energy = player.energy;
            Rider.FW1.NumofShells = player.KineticShel;
            Rider.FW1.Accuracy = player.KineticAcc;
            Rider.FW1.Damage = player.KineticDam;
            Rider.FW1.Ener = player.KineticEner;
            Rider.W1.Accuracy = player.LaserAcc;
            Rider.W1.Damage = player.LaserDam;
            Rider.W1.Ener = player.LaserEner;
            ShowEn.Content = "";
            ShowHP.Content = "";
            PreBattle = true;
            ChoiseRider = false;
            IsLaserActiv = false;
            IsWeaponActiv = false;
            EnemCruisers = new List<EnemCruiser>();
            for (int i = 0; i < Cr; i++)
                EnemCruisers.Add(new EnemCruiser(1000, 100, 20, false));
            for (int i = 0; i < Des; i++)
                EnemDestrs.Add(new EnemDestr(800, 100));
            Random random = new Random();
            int tempC;
            int tempS;
            for (int i = 0; i < EnemCruisers.Count; i++)
            {
                tempC = random.Next(WereCan + 1, Column);
                EnemCruisers[i].Col = tempC;
                tempS = random.Next(Strok);
                EnemCruisers[i].Str = tempS;
                if (!cells[tempS, tempC].IsEmpty)
                    --i;
                else
                {
                    cells[tempS, tempC].IsEmpty = false;
                    cells[tempS, tempC].IsEnemCruiser = true;
                }
            }
            for (int i = 0; i < EnemCruisers.Count; i++)
            {
                EnemCruisers[i].LHP = new Label();
                EnemCruisers[i].LHP.Content = EnemCruisers[i].HP;
                EnemCruisers[i].LHP.Width = LWid;
                EnemCruisers[i].LHP.Height = LHei;
                EnemCruisers[i].LHP.FontSize = LSize;
                EnemCruisers[i].LHP.Foreground = new SolidColorBrush(Colors.Gold);
                Thickness Pos = new Thickness();
                Pos.Left = cells[EnemCruisers[i].Str, EnemCruisers[i].Col].X + LDeltaXM;
                Pos.Top = cells[EnemCruisers[i].Str, EnemCruisers[i].Col].Y + LDeltaYM;
                EnemCruisers[i].LHP.Margin = Pos;
                C03.Children.Add(EnemCruisers[i].LHP);
                cells[EnemCruisers[i].Str, EnemCruisers[i].Col].AddEnemy(EnemCruisers[i]);
                cells[EnemCruisers[i].Str, EnemCruisers[i].Col].image.Source = EnemCruisers[i].bitmap;
            }
            for (int i = 0; i < EnemDestrs.Count; i++)
            {
                tempC = random.Next(WereCan + 1, Column);
                EnemDestrs[i].Col = tempC;
                tempS = random.Next(Strok);
                EnemDestrs[i].Str = tempS;
                if (!cells[tempS, tempC].IsEmpty)
                    --i;
                else
                {
                    cells[tempS, tempC].IsEmpty = false;
                    cells[tempS, tempC].IsEnemDestr = true;
                }
            }
            for (int i = 0; i < EnemDestrs.Count; i++)
            {
                EnemDestrs[i].LHP = new Label();
                EnemDestrs[i].LHP.Content = EnemDestrs[i].HP;
                EnemDestrs[i].LHP.Width = LWid;
                EnemDestrs[i].LHP.Height = LHei;
                EnemDestrs[i].LHP.FontSize = LSize;
                EnemDestrs[i].LHP.Foreground = new SolidColorBrush(Colors.Gold);
                Thickness Pos = new Thickness();
                Pos.Left = cells[EnemDestrs[i].Str, EnemDestrs[i].Col].X + LDeltaXL;
                Pos.Top = cells[EnemDestrs[i].Str, EnemDestrs[i].Col].Y + LDeltaYM;
                EnemDestrs[i].LHP.Margin = Pos;
                C03.Children.Add(EnemDestrs[i].LHP);
                cells[EnemDestrs[i].Str, EnemDestrs[i].Col].AddEnemy(EnemDestrs[i]);
                cells[EnemDestrs[i].Str, EnemDestrs[i].Col].image.Source = EnemDestrs[i].bitmap;
            }
        }

        private void ClearBattle()
        {
            for (int i1 = 0; i1 < Strok; i1++)
            {
                for (int j1 = 0; j1 < Column; j1++)
                {
                    cells[i1, j1].HideMove();
                }
            }
            for (int i = 0; i < EnemCruisers.Count; i++)
            {
                EnemCruisers[i].LHP.Visibility = Visibility.Hidden;
            }
            for (int i = 0; i < EnemDestrs.Count; i++)
            {
                EnemDestrs[i].LHP.Visibility = Visibility.Hidden;
            }
            Rider.Energy = player.energy;
            Rider.HP = player.hp;
            EnemCruisers.RemoveRange(0, EnemCruisers.Count);
            EnemDestrs.RemoveRange(0, EnemDestrs.Count);
            C02.Visibility = Visibility.Visible;
            C03.Visibility = Visibility.Hidden;
            ButKinetic.Visibility = Visibility.Hidden;
            ButLaser.Visibility = Visibility.Hidden;
            LabelEnerKin.Visibility = Visibility.Hidden;
            LabelEnerLaser.Visibility = Visibility.Hidden;
            LabelShellKin.Visibility = Visibility.Hidden;
            PreBattle = false;
            Battle = false;
            ChoiseRider = false;
            IsMovingRider = false;
            Rider.IsActive = false;
            IsTargetOn = false;
            IsLaserActiv = false;
            IsWeaponActiv = false;
            for (int i = 0; i < Strok; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    cells[i, j].image.Visibility = Visibility.Hidden;
                    cells[i, j].IsAlly = cells[i, j].IsRider = cells[i, j].IsEnemy = cells[i, j].IsCanMove = false;
                    cells[i, j].IsEmpty = true;
                    cells[i, j].image = new Image();
                    Thickness Pos = new Thickness();
                    Pos.Left = cells[i, j].X + cells[i, j].XLeft;
                    Pos.Top = cells[i, j].Y + cells[i, j].YTop;
                    cells[i, j].image.Margin = Pos;
                    cells[i, j].image.Width = cells[i, j].wid;
                    C03.Children.Add(cells[i, j].image);
                }
            }
        }

        private void KillCruiser(int i)
        {
            ExpSound.Stop();
            ExpSound.Play();
            player.Money += 400;
            IsTargetOn = false;
            KronSize.Content = player.Money;
            TBase.Visibility = Visibility.Hidden;
            ButLaser.Visibility = Visibility.Hidden;
            ButKinetic.Visibility = Visibility.Hidden;
            cells[EnemCruisers[i].Str, EnemCruisers[i].Col].Destroy();
            EnemCruisers[i].Destroy();
            C03.Children.Add(cells[EnemCruisers[i].Str, EnemCruisers[i].Col].image);
            X = EnemCruisers[i].Str;
            Y = EnemCruisers[i].Col;
            timer.Interval = new TimeSpan(0, 0, 1);
            EnemCruisers.Remove(EnemCruisers[i]);
            timer.Start();
        }

        private void KillDestr(int i)
        {
            ExpSound.Stop();
            ExpSound.Play();
            player.Money += 200;
            IsTargetOn = false;
            KronSize.Content = player.Money;
            TBase.Visibility = Visibility.Hidden;
            ButLaser.Visibility = Visibility.Hidden;
            ButKinetic.Visibility = Visibility.Hidden;
            cells[EnemDestrs[i].Str, EnemDestrs[i].Col].Destroy();
            EnemDestrs[i].Destroy();
            C03.Children.Add(cells[EnemDestrs[i].Str, EnemDestrs[i].Col].image);
            X = EnemDestrs[i].Str;
            Y = EnemDestrs[i].Col;
            timer.Interval = new TimeSpan(0, 0, 1);
            EnemDestrs.Remove(EnemDestrs[i]);
            timer.Start();
        }

        private void RepEnerEnemy()
        {
            for (int i = 0; i < EnemCruisers.Count; i++)
                EnemCruisers[i].Energy = 100;
            for (int i = 0; i < EnemDestrs.Count; i++)
                EnemDestrs[i].Energy = 100;
        }

        private void BadEndTick(object sender, EventArgs e)
        {
            this.IsEnabled = true;
            BadEndCanvas.Visibility = Visibility.Hidden;
            C01.Visibility = Visibility.Visible;
            TimerBadEnd.Stop();
        }

        private void MakeDelButAndSave()
        {
            Player tempplayer;
            try
            {
                BinaryFormatter bin1 = new BinaryFormatter();
                using (Stream fstream = new FileStream(WSave1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    tempplayer = (Player)bin1.Deserialize(fstream);
                }
                LoSa1.Content = tempplayer.Name + "\n" + tempplayer.DateSave;
                DelLoSa1.Visibility = Visibility.Visible;
            }
            catch (Exception)
            {
                DelLoSa1.Visibility = Visibility.Hidden;
                LoSa1.Content = "Сохранения нет";
            }

            try
            {
                BinaryFormatter bin1 = new BinaryFormatter();
                using (Stream fstream = new FileStream(WSave2, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    tempplayer = (Player)bin1.Deserialize(fstream);
                }
                DelLoSa2.Visibility = Visibility.Visible;
                LoSa2.Content = tempplayer.Name + "\n" + tempplayer.DateSave;
            }
            catch (Exception)
            {
                DelLoSa2.Visibility = Visibility.Hidden;
                LoSa2.Content = "Сохранения нет";
            }

            try
            {
                BinaryFormatter bin1 = new BinaryFormatter();
                using (Stream fstream = new FileStream(WSave3, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    tempplayer = (Player)bin1.Deserialize(fstream);
                }
                DelLoSa3.Visibility = Visibility.Visible;
                LoSa3.Content = tempplayer.Name + "\n" + tempplayer.DateSave;
            }
            catch (Exception)
            {
                DelLoSa3.Visibility = Visibility.Hidden;
                LoSa3.Content = "Сохранения нет";
            }

            try
            {
                BinaryFormatter bin1 = new BinaryFormatter();
                using (Stream fstream = new FileStream(WSave4, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    tempplayer = (Player)bin1.Deserialize(fstream);
                }
                DelLoSa4.Visibility = Visibility.Visible;
                LoSa4.Content = tempplayer.Name + "\n" + tempplayer.DateSave;
            }
            catch (Exception)
            {
                DelLoSa4.Visibility = Visibility.Hidden;
                LoSa4.Content = "Сохранения нет";
            }

            try
            {
                BinaryFormatter bin1 = new BinaryFormatter();
                using (Stream fstream = new FileStream(WSave5, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    tempplayer = (Player)bin1.Deserialize(fstream);
                }
                DelLoSa5.Visibility = Visibility.Visible;
                LoSa5.Content = tempplayer.Name + "\n" + tempplayer.DateSave;
            }
            catch (Exception)
            {
                DelLoSa5.Visibility = Visibility.Hidden;
                LoSa5.Content = "Сохранения нет";
            }

            try
            {
                BinaryFormatter bin1 = new BinaryFormatter();
                using (Stream fstream = new FileStream(WSave6, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    tempplayer = (Player)bin1.Deserialize(fstream);
                }
                DelLoSa6.Visibility = Visibility.Visible;
                LoSa6.Content = tempplayer.Name + "\n" + tempplayer.DateSave;
            }
            catch (Exception)
            {
                DelLoSa6.Visibility = Visibility.Hidden;
                LoSa6.Content = "Сохранения нет";
            }
        }
    }
}