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

namespace Patterns___Factory
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

        private void btnWooden_Click(object sender, RoutedEventArgs e)
        {
            Builder builder = new WoodBuilder(cmbBuilder.Text);
            House house = builder.Create();
            Output(builder.Name, house.houseBuilt);
        }

        private void btnPanel_Click(object sender, RoutedEventArgs e)
        {
            Builder builder = new PanelBuilder(cmbBuilder.Text);
            House house = builder.Create();
            Output(builder.Name, house.houseBuilt);
        }

        private void btnBrick_Click(object sender, RoutedEventArgs e)
        {
            Builder builder = new BrickBuilder(cmbBuilder.Text);
            House house = builder.Create();
            Output(builder.Name, house.houseBuilt);
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtOutput.Text = "";
        }

        private void Output(string builder, string house)
        {
            txtOutput.AppendText(builder + " : " + house);
            txtOutput.AppendText(Environment.NewLine);
        }
    } // main class


    // ========== Houses ========== //

    abstract class House
    {
        public string houseBuilt { get; set; }
    }

    class PanelHouse : House
    {
        public PanelHouse()
        {
            houseBuilt = "Panel house is built";
        }
    }

    class WoodenHouse : House
    {
        public WoodenHouse()
        {
            houseBuilt = "Wooden house is built";
        }
    }

    class BrickHouse : House
    {
        public BrickHouse()
        {
            houseBuilt = "Brick house is built";
        }
    }

    // ========== Builder ========== //

    abstract class Builder
    {
        public string Name { get; set; }

        public Builder(string b_name)
        {
            Name = b_name;

            // Name;
        }

        abstract public House Create();
    }

    class PanelBuilder : Builder
    {
        public PanelBuilder(string b_name) : base(b_name) { }

        public override House Create()
        {
            return new PanelHouse();
        }
    }

    class WoodBuilder : Builder
    {
        public WoodBuilder(string name) : base(name) { }
        public override House Create()
        {
            return new WoodenHouse();
        }
    }

    class BrickBuilder : Builder
    {
        public BrickBuilder(string name) : base(name) { }
        public override House Create()
        {
            return new BrickHouse();
        }
    }
}
