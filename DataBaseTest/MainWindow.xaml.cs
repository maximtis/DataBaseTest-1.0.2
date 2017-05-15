using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Windows.Media.Animation;
using System.Diagnostics;
using System.Threading;
namespace DataBaseTest
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    
    public partial class MainWindow : Window
    {
        NewsFeeder MyNews;
        Member MyMember;
        MembersEntry MyEntry = new MembersEntry();
        System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            this.Closing += MainWindow_Closing;

            MyMember = new Member();
            MyNews = new NewsFeeder();
            MyMember.TryAuthorization += Button_EnterToPersonalSpace_Click;
            if (MyMember.CanLoadFromFile(@"bin/storage.bin"))
            {
                MyMember = MyMember.LoadFromFile(@"bin/storage.bin");
                InitializeNewsFeed();
                TextBox_Login.Text = MyMember.Person.Login;
                PasswordBox_Password.Password = MyMember.Person.Password;
            }

            //Grid_Menu.MouseEnter += (s, e) =>
            //{
            //    Grid_Menu.BeginAnimation(OpacityProperty, 
            //        new DoubleAnimation(Grid_Menu.Opacity, 1, TimeSpan.FromSeconds(0.5)));
            //    Grid_Menu.BeginAnimation(MarginProperty, 
            //        new ThicknessAnimation(Grid_Menu.Margin, new Thickness(0, 50, 0, 0), TimeSpan.FromSeconds(0.5)));
            //}; 
            //Grid_Menu.Opacity = 0;
            //Grid_Menu.MouseLeave += (s, e) =>
            //{
            //    Grid_Menu.BeginAnimation(OpacityProperty, 
            //        new DoubleAnimation(Grid_Menu.Opacity, 0, TimeSpan.FromSeconds(0.5)));
            //    Grid_Menu.BeginAnimation(MarginProperty, 
            //        new ThicknessAnimation(Grid_Menu.Margin, new Thickness(-131, 50, 0, 0), TimeSpan.FromSeconds(0.5)));
            //};
        }     

        #region Grid_Test


        #endregion
        
        #region Grid_Authorization
        private void AuthorizationProcessing()
        {
            PasswordBox_Password.IsEnabled = false;
            TextBox_Login.IsEnabled = false;
            MyMember.Person.Login = TextBox_Login.Text;
            MyMember.Person.Password = PasswordBox_Password.Password;
            Label_ConnectionInfo.Content = "Connecting to server...";
            Border_GrayWallTransparent.Visibility = Visibility.Visible;

            Border_GrayWallTransparent.BeginAnimation(OpacityProperty, 
                new DoubleAnimation(0, 0.2, TimeSpan.FromSeconds(0.5)));
            Border_Connecting_and_login.BeginAnimation(MarginProperty, 
                new ThicknessAnimation(new Thickness(350, 0, 350, -100), 
                    new Thickness(350, 0, 350, 0), TimeSpan.FromSeconds(0.5)));

        }
        private void Button_EnterToPersonalSpace_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationProcessing();
                        
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += (s, ev) =>
            {
                if (MyMember.Autorization())
                {
                    MyMember.LoadFromBase();
                    Grid_Authorization.BeginAnimation(OpacityProperty, new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.2)));
                    Grid_Authorization.Visibility = Visibility.Hidden;
                    Grid_PersonalSpace.Visibility = Visibility.Visible;
                    Grid_PersonalSpace.Opacity = 0;
                    Grid_PersonalSpace.BeginAnimation(OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.2)));
                }
                else
                {
                    Label_ConnectionInfo.Content = "Error.";
                    Border_Connecting_and_login.BeginAnimation(MarginProperty, new ThicknessAnimation(new Thickness(350, 0, 350, 0), new Thickness(350, 0, 350, -100), TimeSpan.FromSeconds(1)));
                    Border_GrayWallTransparent.Visibility = Visibility.Hidden;
                    PasswordBox_Password.IsEnabled = true;
                    TextBox_Login.IsEnabled = true;
                    
                }
                Timer.Stop();
            };
            Timer.Start();
        }        
        #endregion

        #region Grid_PersonalSpace
        private void Show_StatusBar()
        {
            FNBox.Visibility = Visibility.Visible;
            LNBox.Visibility = Visibility.Visible;
            EmailBox.Visibility = Visibility.Visible;
            PhoneBox.Visibility = Visibility.Visible;
            DateTimeBox.Visibility = Visibility.Visible;
            FNBox.Text = MyMember.Person.FirstName;
            LNBox.Text = MyMember.Person.LastName;
            EmailBox.Text = MyMember.Person.Email;
            PhoneBox.Text = MyMember.Person.Mobile_Number;
            DateTimeBox.Text = DateTime.Now.ToShortDateString();
        }
        private void Button_SendMessage_Click(object sender, RoutedEventArgs e)
        {
                SendMessage MyMessage = new SendMessage();
                MyMessage.ShowDialog();
                if (MyMessage.Message.Text!="")
                MyMember.SendMessage(MyMessage.Message.Text);
        }
        private void Button_ShowInfo_Click(object sender, RoutedEventArgs e)
        {
            PersonInfo Info = new PersonInfo(MyMember);
            Info.MyMember = MyMember;
            Info.ShowDialog();
            if (Info.UpdatedMember!=null)
            MyMember = Info.UpdatedMember.DeepCopy();
            Show_StatusBar();
        }
        private void Button_SendReport_Click(object sender, RoutedEventArgs e)
        {
            Report SenderReport = new Report(MyMember);
            SenderReport.ShowDialog();
        }
        private void Button_News_Updater_Click(object sender, RoutedEventArgs e)
        {
            InitializeNewsFeed();
        }
        #endregion

        #region Grid_Menu
        private void Button_Menu_Login_Click(object sender, RoutedEventArgs e)
        {
            if (MyMember.Authorized)
            {
                Grid_News.Visibility = Visibility.Hidden;
                Grid_Authorization.Visibility = Visibility.Hidden;
                Grid_PersonalSpace.Visibility = Visibility.Visible;
                Grid_Admin.Visibility = Visibility.Hidden;
            }
            else
            {
                Grid_News.Visibility = Visibility.Hidden;
                Grid_Admin.Visibility = Visibility.Hidden;
                Grid_PersonalSpace.Visibility = Visibility.Hidden;
                Grid_Authorization.Visibility = Visibility.Visible;

            }
        }
        private void Button_Menu_General_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void Button_Menu_Chat_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("В Разработке... ");
        }
        private void Button_Menu_Admin_Panel_Click(object sender, RoutedEventArgs e)
        {
            Grid_News.Visibility = Visibility.Hidden;
            Grid_PersonalSpace.Visibility = Visibility.Hidden;
            Grid_Admin.Visibility = Visibility.Visible;
        }
        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Window
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MyMember.SaveToFile(@"bin/storage.bin");
        }

        #endregion

        #region Grid_Admin

        private void InitializeEntry_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)RadioButton_FromBase.IsChecked)
                MyEntry.InitializeFromBase();
            else
                MyEntry.InitializeFromFile(18);//Количество студентов

            for (int i = 0; i < MyEntry.Count; i++)
            {
                ListBox_MembersVisualizator.Items.Add(MyEntry[i].Person.FirstName + " " + MyEntry[i].Person.LastName);
            }
            Button_InitializeEntry.IsEnabled = false;
                
        }
        private void Button_SelectPerson_Click(object sender, RoutedEventArgs e)
        {
            PersonInfo Redactor = new PersonInfo(MyEntry[ListBox_MembersVisualizator.SelectedIndex]);
            Redactor.Button_Update_PersonalInfo.Visibility = Visibility.Visible;
            Redactor.TextBlock_Informator.IsReadOnly = false;
            Redactor.ShowDialog();
        }
        private void Button_ClearEntry_Click(object sender, RoutedEventArgs e)
        {
            MyEntry.SaveMembersToFile(18);
            Button_InitializeEntry.IsEnabled = true;
            MyEntry.ClearMembers();
            ListBox_MembersVisualizator.Items.Clear();
        }

        private void Button_LookMessage_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < MyEntry.Count; i++)
            {
                ListBox_MessageVisualizator.Items.Add(MyEntry[i].Person.Password);
            }
            
        }
        private void Button_PostNews_Click(object sender, RoutedEventArgs e)
        {
            NewsFeeder.PostNews(new NewsNode(TextBox_NewsName.Text,
                                            TextBox_NewsContent.Text,
                                            TextBox_NewsAuthor.Text,
                                            DateTime.Now.ToShortDateString(),
                                            TextBox_ImgSource.Text));
        }
        #endregion

        #region Grid_News
        private void InitializeNewsFeed()
        {
            ListBox_NewsFeed.Items.Clear();
            MyNews.UpdateInfo();

            for (int i = 0; i < MyNews.NewsFeed.Count; i++)
                ListBox_NewsFeed.Items.Add(MyNews.NewsFeed[i].Name);

            if (MyNews.NewsFeed.Count>0)
            {
                ListBox_NewsFeed.SelectedIndex = 0;
            }

        }
        private void Button_Menu_News_Click(object sender, RoutedEventArgs e)
        {
            Grid_Admin.Visibility = Visibility.Hidden;
            Grid_PersonalSpace.Visibility = Visibility.Hidden;
            Grid_Authorization.Visibility = Visibility.Hidden;
            Grid_News.Visibility = Visibility.Visible;
        }

        private void ListBox_NewsFeed_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBox_NewsFeed.SelectedIndex > -1)
            {
                /*new BitmapImage(new Uri("http://weingame.clan.su/NewsImg/community.jpg"));*/
                SourceImg.Source = MyNews.NewsFeed[ListBox_NewsFeed.SelectedIndex].ImgSource;
                ContentField.Text = MyNews.NewsFeed[ListBox_NewsFeed.SelectedIndex].NewsContent;
                Author.Text = MyNews.NewsFeed[ListBox_NewsFeed.SelectedIndex].Author;
                Date.Text = MyNews.NewsFeed[ListBox_NewsFeed.SelectedIndex].Date;
            }
        }
        #endregion

        
    }
}
