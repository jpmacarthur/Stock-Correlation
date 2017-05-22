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
        public MainWindow()
        {
            InitializeComponent();
            List<string> csv = parCSV.stockParse();
            ViewModel list = new ViewModel(csv);
            //stock1.DataContext = list;

            Correl.DataContext = corre;
            Symb.DataContext = tester;
            typedPrice.DataContext = tester;
            //s1Price.DataContext = tester2;
            Symb2.DataContext = tester2;
            typedPrice2.DataContext = tester2;
            actest.DataContext = list;
            Collector d = new Collector();
            Dictionary<string, string> test = new Dictionary<string, string>();
          //  test = d.main();
          //  sqlUpload up = new sqlUpload();
          //  up.upload(test);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            sqlRetrieve getter = new sqlRetrieve();
            List<double> jsf = getter.retrievePrice(tester.Symbol);
            List<double> jsf2 = getter.retrievePrice(tester2.Symbol);
            double average = jsf.Average();
            tester.Price = average.ToString("0.##");

            jsf2 = getter.retrievePrice(tester2.Symbol);
            double average2 = jsf2.Average();

            tester2.Price = average2.ToString("0.##");
            corre.RVal = calc.ComputeCoeff(jsf.ToArray(), jsf2.ToArray());
        }
    }
    public class ViewModel
    {
        public ObservableCollection<string> list { get; private set; }
        public ViewModel(List<string> ha)
        {
            list = new ObservableCollection<string>(ha);
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
