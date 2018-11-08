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

namespace Patterns___Adapter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            Driver driver = new Driver();
            Bus bus = new Bus();

            driver.Move(bus);
            txtOutut.AppendText(bus.action + Environment.NewLine);

            Husky husky = new Husky();
            ITransport transport = new WalkToTravelAdapter(husky);
            driver.Move(transport);
            txtOutut.AppendText(husky.action + Environment.NewLine);
        }
    }

    // interfaces
    interface ITransport
    {
        void Travel();
    }

    interface IAnimal
    {
        void Walk();
    }
    
    class Driver
    {
        public void Move(ITransport transport)
        {
            transport.Travel();
        }
    }

    // transoprt types 
    class Bus : ITransport
    {
        public string action { get; set; }
        public void Travel()
        {
            action = "Bus travels on the roads";
        }
    }

    class Husky : IAnimal
    {
        public string action { get; set; }
        public void Walk()
        {
            action = "Husky travels on snow";
        }
    }

    // adapter
    class WalkToTravelAdapter : ITransport
    {
        Husky husky;
        public WalkToTravelAdapter(Husky h)
        {
            husky = h;
        }

        public void Travel()
        {
            husky.Walk();
        }
    }
}
