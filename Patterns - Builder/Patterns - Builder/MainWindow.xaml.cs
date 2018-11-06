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

namespace Patterns___Builder
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

        private void btnBuild_Click(object sender, RoutedEventArgs e)
        {
            if (cmbBudget.Text.Length < 1)
            {
                return;
            }

            Builder builder = new Builder();
            CarMaker maker;

            if (cmbBudget.Text == "I have tonns of cash")
            {
                maker = new RichCar();
            }
            else
            {
                maker = new PoorCar();
            }

            Car car = builder.build(maker);

            txtOutput.AppendText(car.ToString() + Environment.NewLine);
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtOutput.Clear();
        }
    } // main class

    class CarModel
    {
        public string modelName { get; set; }
    }

    class Color
    {
        public string colorName { get; set; }
    }

    class Extras
    {
        public bool install;
    }

    class Car
    {
        public CarModel model { get; set; }

        public Color color { get; set; }

        public Extras extras { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (model != null)
            {
                sb.Append(model.modelName + Environment.NewLine);
            }
            if (color != null)
            {
                sb.Append(color.colorName + Environment.NewLine);
            }
            if (extras.install == true)
            {
                sb.Append("With Extras" + Environment.NewLine);
            }
            else
            {
                sb.Append("Without Extras" + Environment.NewLine);
            }

            return sb.ToString();
        }
    }

    abstract class CarMaker
    {
        public Car car { get; private set; }

        public void CreateCar()
        {
            car = new Car();
        }

        public abstract void SetModel();
        public abstract void SetColor();
        public abstract void SetExtras();

    }

    class Builder
    {
        public Car build(CarMaker carMaker)
        {
            carMaker.CreateCar();

            carMaker.SetModel();
            carMaker.SetColor();
            carMaker.SetExtras();

            return carMaker.car;
        }
    }

    class RichCar : CarMaker
    {
        public override void SetModel()
        {
            this.car.model = new CarModel { modelName = "Lamborghini Gallardo" };
        }
        public override void SetColor()
        {
            this.car.color = new Color { colorName = "Electric Orange" };
        }
        public override void SetExtras()
        {
            this.car.extras = new Extras { install = true };
        }
    }

    class PoorCar : CarMaker
    {
        public override void SetModel()
        {
            this.car.model = new CarModel { modelName = "Fiat Punto" };
        }
        public override void SetColor()
        {
            this.car.color = new Color { colorName = "Brown dirt" };
        }
        public override void SetExtras()
        {
            this.car.extras = new Extras { install = false };
        }
    }



}
