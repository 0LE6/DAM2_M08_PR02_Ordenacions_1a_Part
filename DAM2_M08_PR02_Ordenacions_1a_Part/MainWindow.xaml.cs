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

namespace DAM2_M08_PR02_Ordenacions_1a_Part
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

        private void btnOrdenar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnPosicionar_Click(object sender, RoutedEventArgs e)
        {
            // Limpia el Canvas antes de dibujar nuevas figuras
            cvCanvas.Children.Clear();

            int numeroDeElementos = iudElements.Value ?? 0; // Asumiendo que iudElements es un IntegerUpDown
            bool invertido = cb.IsChecked == true;
            bool aleatorio = AleatoriCheckBox.IsChecked == true;

            int[] elementos = Enumerable.Range(1, numeroDeElementos).ToArray();

            if (invertido)
            {
                Array.Reverse(elementos);
            }
            else if (aleatorio)
            {
                Random rng = new Random();
                elementos = elementos.OrderBy(x => rng.Next()).ToArray();
            }

            DibujarFiguras(elementos);
        }

        private void DibujarFiguras(int[] elementos)
        {
            // Determina si dibujar círculos o barras
            bool dibujarCirculos = cbFiguras.SelectedItem.ToString().Contains("Círculos");

            for (int i = 0; i < elementos.Length; i++)
            {
                if (dibujarCirculos)
                {
                    // Dibuja círculos
                    Ellipse ellipse = new Ellipse
                    {
                        Width = iudRadi.Value ?? 0,
                        Height = iudRadi.Value ?? 0,
                        Fill = new SolidColorBrush(Colors.Black) // Cambia esto por el color deseado
                    };
                    Canvas.SetLeft(ellipse, i * (ellipse.Width + 10)); // Ajusta la posición X según sea necesario
                    Canvas.SetTop(ellipse, cvCanvas.Height / 2 - ellipse.Height / 2); // Centra en Y
                    cvCanvas.Children.Add(ellipse);
                }
                else
                {
                    // Dibuja barras
                    Rectangle rectangle = new Rectangle
                    {
                        Width = iudGrosor.Value ?? 0,
                        Height = 100, // Altura fija para el ejemplo; cámbiala según necesites
                        Fill = new SolidColorBrush(Colors.Black) // Cambia esto por el color deseado
                    };
                    Canvas.SetLeft(rectangle, i * (rectangle.Width + 10)); // Ajusta la posición X según sea necesario
                    Canvas.SetTop(rectangle, cvCanvas.Height - rectangle.Height); // Alinea al fondo del Canvas
                    cvCanvas.Children.Add(rectangle);
                }
            }
        }

    }
}
