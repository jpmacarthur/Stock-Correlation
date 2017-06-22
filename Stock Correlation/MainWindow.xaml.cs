using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using Calculator;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Stock_Correlation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        stockDisp tester = new stockDisp() { };
        stockDisp tester2 = new stockDisp() { };
        downloadJson dl = new downloadJson();
        parseJson parser = new parseJson();
        CSV_Parser parCSV = new CSV_Parser();
        corrDisp corre = new corrDisp();
        Correlation calc = new Correlation();
        Dictionary<string, string> names = new Dictionary<string, string>();
        searchBarinfo sear = new searchBarinfo();
        ViewModel list = new ViewModel();
        ViewModel list2 = new ViewModel();
        caldate wpfCAL = new caldate();
        popupDim pud = new popupDim();
        HashSet<string> hashednames = new HashSet<string>();
        int m_count = 0;
        double tempcor = 0;
        bool mysqlError = false;
        Task<List<double>> T_one = new Task<List<double>>(() =>
        {
            List<double> temp = new List<double>();
            return temp;
        });
        Task<List<double>> T_second = new Task<List<double>>(() =>
        {
            List<double> temp = new List<double>();
            return temp;
        });
        Task<double> T5 = new Task<double>(()=> {
            double temp = 0;
            return temp;
        });
        public MainWindow()
        {
            InitializeComponent();
            sqlRetrieve getter = new sqlRetrieve();
            List<string> csv = parCSV.getSymbols();
            List<stockname> l1 = new List<stockname>();
            List<stockname> l2 = new List<stockname>();
            Dictionary<string, string> names = parCSV.getSymbolName();
            foreach (string str in csv)
            {
                hashednames.Add(str);
            }
            foreach (KeyValuePair<string, string> pair in names)
            {
                if (pair.Value.Contains(@"&#39;"))
                {
                    string temp2 = pair.Value.Replace(@"&#39;", "'");
                    stockname temp3 = new stockname(pair.Key, temp2);
                    l1.Add(temp3);
                }
                else
                {
                    stockname temp = new stockname(pair.Key, pair.Value);
                    l1.Add(temp);
                }
            }
            list = new ViewModel(l1);
            list2 = new ViewModel(l1);
            sear = new searchBarinfo(names);
            Correl.DataContext = corre;
            sto1.DataContext = list;
            sto2.DataContext = list2;
            typedPrice.DataContext = tester;
            typedPrice2.DataContext = tester2;
            cale.DataContext = wpfCAL;
            pU2.DataContext = pud;
            Collector d = new Collector();
            Dictionary<string, string> test = new Dictionary<string, string>();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            load.Visibility = Visibility.Visible;
            sqlRetrieve getter = new sqlRetrieve();

            Task T_one_ex = T_one.ContinueWith((antecedent) =>
                {
                    if (mysqlError == true)
                    {
                        MessageBox.Show("Check your database settings, I was unable to connect");
                        load.Visibility = Visibility.Collapsed;
                        }
                    else {
                        try
                        {
                            tester.Price = T_one.Result.Average().ToString("0.##");
                        }
                        catch (InvalidOperationException s) { MessageBox.Show(String.Format("Error {0}", s.Message)); }
                    }


                    m_count--;
                    if (m_count <= 0) { load.Visibility = Visibility.Collapsed; }
                }, TaskScheduler.FromCurrentSynchronizationContext());
            Task T_second_ex = T_second.ContinueWith((antecedent) =>
            {
                if (mysqlError == false)
                {
                    try
                    {
                        tester2.Price = T_second.Result.Average().ToString("0.##");
                    }
                    catch (InvalidOperationException s) { MessageBox.Show(String.Format("Error {0}", s.Message)); }

                    m_count--;
                    if (m_count <= 0) { load.Visibility = Visibility.Collapsed; }
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
            Task T5_ex = T5.ContinueWith((antecedent) =>
            {
                corre.RVal = antecedent.Result;
                m_count--;
                if(m_count <= 0) load.Visibility = Visibility.Collapsed;
            }, TaskScheduler.FromCurrentSynchronizationContext());

        }
        private void cal_Test_Click_1(object sender, RoutedEventArgs e)
        {
            DateTime? date = wpfCAL.SelDate;
            MessageBox.Show(date.Value.ToString("yyyy-MM-dd"));

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Settings s = new Settings();
            s.Show();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            MainWindow ne = new MainWindow();
            ne.Show();
        }

        private void SETT_Click(object sender, RoutedEventArgs e)
        {
            pud.Height = (int)totG.ActualHeight;
            pud.Width = (int)totG.ActualWidth;
            pU2.Visibility = Visibility.Visible;
            mysqlError = false;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void sto1_LostFocus(object sender, RoutedEventArgs e)
        {
            if(list.SelectedName == null && hashednames.Contains(sto1.Text.ToUpper()))
            {
                var temp = list.Names.FirstOrDefault(o => o.symbol == sto1.Text.ToUpper());
                stockname tempitem = new stockname(temp.symbol, temp.name);
                list.SelectedName = tempitem;
            }


            }

        private void sto1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sqlRetrieve getter = new sqlRetrieve();
            if (list2.SelectedName!= null) { T5 = Task.Factory.StartNew(() =>
            {

                {
                    List<double> jsf = new List<double>();
                    List<double> jsf2 = new List<double>();
                    m_count++;
                    try
                    {
                        jsf = getter.retrievePrice(list.SelectedName.symbol, Properties.Settings.Default.server, Properties.Settings.Default.database, Properties.Settings.Default.password, Properties.Settings.Default.username);
                        jsf2 = getter.retrievePrice(list2.SelectedName.symbol, Properties.Settings.Default.server, Properties.Settings.Default.database, Properties.Settings.Default.password, Properties.Settings.Default.username);
                    }
                    catch (MySqlException ex) { mysqlError = true; }
                    if (mysqlError == false) tempcor = Math.Round(calc.ComputeCoeff(jsf, jsf2), 3);
                    return tempcor;
                }
            });
            }

            string selected1 = "";
            try { selected1 = list.SelectedName.symbol; }
            catch (NullReferenceException ed) { }
            T_one = Task.Factory.StartNew(() =>
            {
                m_count++;
                List<double> jsf = new List<double>();
                try
                {
                    jsf = getter.retrievePrice(selected1, Properties.Settings.Default.server, Properties.Settings.Default.database, Properties.Settings.Default.password, Properties.Settings.Default.username);
                }
                catch (MySqlException ex) { mysqlError = true; }
                return jsf;
            });
        }

        private void sto2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(list.SelectedName != null) { T5.Start(); }
            sqlRetrieve getter = new sqlRetrieve();
            string selected1 = list2.SelectedName.symbol;

            try
            {
                T_second = Task.Factory.StartNew(() =>
            {
            List<double> jsf = new List<double>();
                m_count++;
                try
                {
                    jsf = getter.retrievePrice(selected1, Properties.Settings.Default.server, Properties.Settings.Default.database, Properties.Settings.Default.password, Properties.Settings.Default.username);
                }
                catch (MySqlException) { mysqlError = true; }
                return jsf;
            });
            }
            catch(AggregateException ae) { if (ae.InnerException is MySqlException) { mysqlError = true; } }
        }
    }

    public class popupDim : INotifyPropertyChanged
        {
            private int height;
            public int Height
            {
                get { return height; }
                set
                {
                    height = value;
                    OnPropertyChanged();
                }
            }
            private int width;
            public int Width
            {
                get { return width; }
                set
                {
                    width = value;
                    OnPropertyChanged();
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            private void OnPropertyChanged([CallerMemberName] string caller = "")
            {
                if (PropertyChanged != null)
                {
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(caller));
                    };
                }
            }
        }
        public class ViewModel
        {
            public ObservableCollection<stockname> Names { get; set; }
            public stockname SelectedName { get; set; }
            public ViewModel(List<stockname> ha)
            {
                Names = new ObservableCollection<stockname>(ha);
            }
            public ViewModel()
            {
                Names = new ObservableCollection<stockname>();
            }
            public AutoCompleteFilterPredicate<object> PersonFilter
            {
                get
                {
                    return (searchText, obj) =>
                        (obj as stockname).symbol.ToLower().Contains(searchText.ToLower())
                        || (obj as stockname).name.ToLower().Contains(searchText.ToLower());
                }
            }
        }
        public class searchBarinfo
        {
            public ObservableCollection<string> list { get; private set; }
            public ObservableCollection<string> list2 { get; private set; }

            public searchBarinfo(Dictionary<string, string> ha)
            {
                list = new ObservableCollection<string>(ha.Keys.ToList());
                list2 = new ObservableCollection<string>(ha.Values.ToList());
            }
            public searchBarinfo()
            {
                list = new ObservableCollection<string>();
                list2 = new ObservableCollection<string>();
            }
        }
        public class caldate : INotifyPropertyChanged
        {
            private DateTime selDate;
            public DateTime SelDate
            {
                get { return selDate; }
                set
                {
                    selDate = value;
                    OnPropertyChanged();
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            public caldate()
            {
                selDate = DateTime.Now;
            }
            private void OnPropertyChanged([CallerMemberName] string caller = "")
            {
                if (PropertyChanged != null)
                {
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(caller));
                    };
                }
            }
        }
        public class corrDisp : INotifyPropertyChanged
        {
            private double rVal;
            public double RVal
            {
                get { return rVal; }
                set
                {
                    rVal = value;
                    OnPropertyChanged();
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            private void OnPropertyChanged([CallerMemberName] string caller = "")
            {
                if (PropertyChanged != null)
                {
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(caller));
                    };
                }
            }
        }
        public class stockDisp : INotifyPropertyChanged
        {
            private string symbol;
            public string Symbol
            {
                get { return symbol; }
                set
                {
                    symbol = value;
                    OnPropertyChanged();
                }
            }
            private string price;
            public string Price
            {
                get { return price; }
                set
                {
                    price = value;
                    OnPropertyChanged();
                }
            }


            public event PropertyChangedEventHandler PropertyChanged;
            private void OnPropertyChanged([CallerMemberName] string caller = "")
            {
                if (PropertyChanged != null)
                {
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(caller));
                    };
                }
            }
        }
    }

