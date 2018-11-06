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

namespace Pattern_Observer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<ObserverList> allObservers = new List<ObserverList>();
        Stock stock = new Stock();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (rbBroker.IsChecked == false && rbBnak.IsChecked == false)
            {
                return;
            }

            // add observer to list, divide between listboxes

            ObserverList observer = new ObserverList();

            observer.name = txtName.Text.Trim();
            observer.minPrice = txtMinPrice.Text.Trim();
            observer.maxPrice = txtMaxPrice.Text.Trim();

            string item = observer.name + ": Min = " + observer.minPrice + "; Max = " + observer.maxPrice;
            if (rbBnak.IsChecked == true)
            {
                listBanks.Items.Add(item);
                observer.type = "Bank";
            }
            else
            {
                listBrokers.Items.Add(item);
                observer.type = "Broker";
            }

            allObservers.Add(observer);

            // clear fields for next element
            txtName.Clear();
            txtMinPrice.Text = "30";
            txtMaxPrice.Text = "40";
            rbBnak.IsChecked = false;
            rbBroker.IsChecked = false;

        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            // prepare list of banks and brokers based on the list
            // create observerver
            List<Bank> banks = new List<Bank>();
            List<Broker> brokers = new List<Broker>();

            foreach (ObserverList observer in allObservers)
            {
                if (observer.type == "Bank")
                {
                    Bank bank = new Bank(observer.name, observer.minPrice, observer.maxPrice, stock);
                    banks.Add(bank);
                }
                else
                {
                    Broker broker = new Broker(observer.name, observer.minPrice, observer.maxPrice, stock);
                    brokers.Add(broker);
                }
            }

            /*
             * Loop 10 times, each time currency price changes
             * On loop 4 = broker "Matt" stops trading
             * On loop 7 = bank "DNB" stops trading
             */
            for (int i = 1; i <= 10; i++)
            {
                txtOutput.AppendText(i + ":" + Environment.NewLine);
                stock.Market();

                foreach (var bank in banks)
                {
                    txtOutput.AppendText(bank.decision + Environment.NewLine);
                    if (i == 7 && bank.bankName.Contains("DNB"))
                    {
                        bank.StopTrade();
                        banks.Remove(bank);
                        break;
                    }
                }
                foreach (var broker in brokers)
                {
                    if (i == 4 && broker.brokerName.Contains("Matt"))
                    {
                        broker.StopTrade();
                        brokers.Remove(broker);
                        break;
                    }
                    txtOutput.AppendText(broker.decision + Environment.NewLine);
                }
            }
            //lblPrice.Content = stock.currency;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtOutput.Clear();
        }
    }

    // list to gather all observers and data
    class ObserverList
    {
        public string name { get; set; }
        public string minPrice { get; set; }
        public string maxPrice { get; set; }
        public string type { get; set; }
    }

    // ===== interfaces ===== //
    interface IObserver
    {
        void Update(Object obj);
    }

    interface IObservable
    {
        void RegisterObserver(IObserver o);
        void RemoveObserver(IObserver o);
        void NotifyObservers();
    }

    // ===== stock classes ===== //
    class StockInfo
    {
        public double Currency { get; set; }
    }

    class Stock : IObservable
    {
        public string currency { get; set; }

        StockInfo stockInfo;
        List<IObserver> observers;

        Random random = new Random();

        public Stock()
        {
            observers = new List<IObserver>();
            stockInfo = new StockInfo();
        }

        public void RegisterObserver(IObserver observer)
        {
            observers.Add(observer);
        }
        public void RemoveObserver(IObserver observer)
        {
            observers.Remove(observer);
        }
        public void NotifyObservers()
        {
            foreach (IObserver item in observers)
            {
                item.Update(stockInfo);
            }
        }

        public void Market()
        {
            // generate random number (price) return value, notify observers
            stockInfo.Currency = random.Next(20, 50);
            currency = stockInfo.Currency.ToString();
            NotifyObservers();
        }

    } // class stock

    // ===== observer classes ===== //

    // bank and broker classes basicaly are the same, possible to unite them in one class

    class Bank : IObserver
    {
        public string bankName { get; set; }
        public string minPrice { get; set; }
        public string maxPrice { get; set; }
        public string decision { get; set; }

        IObservable stock;

        public Bank(string name, string min, string max, IObservable observable)
        {
            this.bankName = name;
            this.minPrice = min;
            this.maxPrice = max;

            stock = observable;
            stock.RegisterObserver(this);
        }

        public void Update(object obj)
        {
            // on update decide, buy, sell or keep cash
            StockInfo stockInfo = (StockInfo)obj;
            decision = "Price: " + stockInfo.Currency + ": ";
            if (stockInfo.Currency > Convert.ToInt32(minPrice) && stockInfo.Currency < Convert.ToInt32(maxPrice))
            {
                decision += "Bank "+ bankName + " waits for better chance";
                return;
            }
            if (stockInfo.Currency <= Convert.ToInt32(minPrice))
            {
                decision += "Bank " + bankName + " buys currency for " + stockInfo.Currency;
            }
            if (stockInfo.Currency >= Convert.ToInt32(maxPrice))
            {
                decision += "Bank " + bankName + " sells currency for " + stockInfo.Currency;
            }
        }

        // stops current observer from further participation
        public void StopTrade()
        {
            stock.RemoveObserver(this);
            stock = null;
        }
    } // class bank

    class Broker : IObserver
    {
        public string brokerName { get; set; }
        public string minPrice { get; set; }
        public string maxPrice { get; set; }
        public string decision { get; set; }

        IObservable stock;

        public Broker(string name, string min, string max, IObservable observable)
        {
            this.brokerName = name;
            this.minPrice = min;
            this.maxPrice = max;

            stock = observable;
            stock.RegisterObserver(this);
        }

        public void Update(object obj)
        {
            // on update decide, buy, sell or keep cash
            StockInfo stockInfo = (StockInfo)obj;
            decision = "Price: " + stockInfo.Currency + ": ";

            switch (stockInfo.Currency)
            {
                case double price when stockInfo.Currency <= Convert.ToDouble(minPrice):
                    decision += "Broker " + brokerName + " buys currency for " + price;
                    break;
                case double price when stockInfo.Currency >= Convert.ToDouble(maxPrice):
                    decision += "Broker " + brokerName + " sells currency for " + price;
                    break;
                default:
                    decision += "Broker " + brokerName + " waits for better chance";
                    break;
            }
        } // update

        // stops current observer from further participation
        public void StopTrade()
        {
            stock.RemoveObserver(this);
            stock = null;
        }
    } // class broker 
}
