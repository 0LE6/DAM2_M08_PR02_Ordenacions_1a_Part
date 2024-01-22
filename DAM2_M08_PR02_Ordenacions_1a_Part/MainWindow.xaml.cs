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

            // Establecer colores por defecto para los ColorPickers
            colorCorrecte.SelectedColor = Colors.Green;
            colorIncorrecter.SelectedColor = Colors.Red;

            // Si deseas configurar el color de fondo por defecto del Canvas:
            colorFons.SelectedColor = Colors.White; // O el color que prefieras
            cvCanvas.Background = new SolidColorBrush(colorFons.SelectedColor.Value);
        }


        private void btnOrdenar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnPosicionar_Click(object sender, RoutedEventArgs e)
        {
            // Limpia el Canvas antes de dibujar nuevas figuras
            cvCanvas.Children.Clear();

            int numeroDeElementos = iudElements.Value ?? 0; // Asumiendo que iudElements es un IntegerUpDown
            bool invertido = checkInvertit.IsChecked == true;
            bool aleatorio = checkAleatori.IsChecked == true;

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
            bool dibujarCirculos = (cbFiguras.SelectedItem as ComboBoxItem)?.Content.ToString() == "Círculos";

            // Obtenim les dimensions del Canvas
            double alturaCanvas = cvCanvas.ActualHeight;
            double ampladaCanvas = cvCanvas.ActualWidth;

            // Calculem l'ample de cada barra basant-nos en el número d'elements
            double ampladaBarra = ampladaCanvas / elementos.Length;

            // Trobem l'element màxim per a escalar les alçades de les barres respecte aquest valor
            int valorMaximo = elementos.Any() ? elementos.Max() : 0;

            // Obtenim els colors dels ColorPickers
            Color colorCorrecte = this.colorCorrecte.SelectedColor ?? Colors.Green; // Color verd per defecte
            Color colorIncorrecte = this.colorIncorrecter.SelectedColor ?? Colors.Red;

            // Creem un array ordenat per a comparar
            int[] elementosOrdenados = (int[])elementos.Clone();
            Array.Sort(elementosOrdenados);

            for (int i = 0; i < elementos.Length; i++)
            {
                if (dibujarCirculos)
                {
                    // Dibuja círculos si es necesario
                }
                else
                {
                    // Dibuja barras
                    double alturaBarra = (elementos[i] / (double)valorMaximo) * alturaCanvas;
                    Rectangle rectangle = new Rectangle
                    {
                        Width = ampladaBarra - (iudGrosor.Value ?? 0) * 2, // Restem el grosor del borde per cada costat
                        Height = alturaBarra,
                        Stroke = new SolidColorBrush(Colors.Black), // Color del borde
                        StrokeThickness = iudGrosor.Value ?? 0,
                        Fill = new SolidColorBrush(elementos[i] == elementosOrdenados[i] ? colorCorrecte : colorIncorrecte) // Color interior
                    };
                    Canvas.SetLeft(rectangle, i * ampladaBarra); // Ajusta la posición X
                    Canvas.SetTop(rectangle, alturaCanvas - alturaBarra); // Ajusta la posición Y des de la part inferior del Canvas
                    cvCanvas.Children.Add(rectangle);
                }
            }
        }




    }
}
