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
        public MainWindow()
        {
            InitializeComponent();
            List<string> csv = parCSV.getSymbols();
            List<stockname> l1 = new List<stockname>();
            List<stockname> l2 = new List<stockname>();
            Dictionary<string, string> names = parCSV.getSymbolName();
            foreach(KeyValuePair<string, string> pair in names)
            {
                stockname temp = new stockname(pair.Key, pair.Value);
                l1.Add(temp);
            }
            list = new ViewModel(l1);
            list2 = new ViewModel(l1);
            sear = new searchBarinfo(names);
            //stock1.DataContext = list;
            Correl.DataContext = corre;
            sto1.DataContext = list;
            sto2.DataContext = list2;
            typedPrice.DataContext = tester;
            //s1Price.DataContext = tester2;
            typedPrice2.DataContext = tester2;
          //  actest.DataContext = list;
            Collector d = new Collector();
            Dictionary<string, string> test = new Dictionary<string, string>();
          //  test = d.main();
          //  sqlUpload up = new sqlUpload();
          //  up.upload(test);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            sqlRetrieve getter = new sqlRetrieve();
            string selected1 = list.SelectedName.symbol;
            string selected2 = list2.SelectedName.symbol;
            List<double> jsf = getter.retrievePrice(selected1);
            List<double> jsf2 = getter.retrievePrice(selected2);
            double average = jsf.Average();
            tester.Price = average.ToString("0.##");

            double average2 = jsf2.Average();

            tester2.Price = average2.ToString("0.##");
            corre.RVal = Math.Round(calc.ComputeCoeff(jsf.ToArray(), jsf2.ToArray()),3);
        }
    }
    public interface ImyViewModel
    {
        IEnumerable<string> Names { get; }
        string SelectedName { get; set; }
    }
    public interface IPeopleViewModel
    {
        IEnumerable<Person> People { get; }
        Person SelectedPerson { get; set; }
    }
    public class ViewModel { 
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
        public string Symbol { get { return symbol;}
            set { symbol = value;
                OnPropertyChanged();
            }
        }
        private string price;
        public string Price
        {
            get { return price; }
            set { price = value;
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
    } }
