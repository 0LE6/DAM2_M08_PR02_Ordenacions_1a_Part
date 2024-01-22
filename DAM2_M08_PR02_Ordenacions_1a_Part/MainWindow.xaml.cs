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

        // Método auxiliar para controlar los checked y unchecked entre los 2 checkbox
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;

            if (checkBox != null)
            {
                if (checkBox.Name == "checkInvertit" && checkInvertit.IsChecked == true)
                {
                    checkAleatori.IsChecked = false;
                }
                else if (checkBox.Name == "checkAleatori" && checkAleatori.IsChecked == true)
                {
                    checkInvertit.IsChecked = false;
                }
            }
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
            // Obtenemos las dimensiones del Canvas
            double alturaCanvas = cvCanvas.ActualHeight;
            double anchoCanvas = cvCanvas.ActualWidth;

            double tamañoFijo = 30; // Este será el diámetro de los círculos

            // Calculamos el espacio entre las figuras basándonos en el número de elementos
            double espacioEntreFiguras = anchoCanvas / elementos.Length;

            // Encontramos el elemento máximo para escalar las alturas de las figuras con respecto a este valor
            int valorMaximo = elementos.Any() ? elementos.Max() : 1; // Evitamos la división por cero

            // Obtenemos los colores de los ColorPickers
            Color colorCorrecto = this.colorCorrecte.SelectedColor ?? Colors.Green;
            Color colorIncorrecto = this.colorIncorrecter.SelectedColor ?? Colors.Red;

            // Creamos un array ordenado para comparar
            int[] elementosOrdenados = (int[])elementos.Clone();
            Array.Sort(elementosOrdenados);

            for (int i = 0; i < elementos.Length; i++)
            {
                // Calculamos la posición y altura basándonos en el valor del elemento
                double alturaFigura = (elementos[i] / (double)valorMaximo) * alturaCanvas;

                if ((cbFiguras.SelectedItem as ComboBoxItem)?.Content.ToString() == "Círculos")
                {
                    // Dibujamos círculos (elipses)
                    Ellipse circulo = new Ellipse
                    {
                        Width = tamañoFijo,
                        Height = tamañoFijo,
                        Fill = new SolidColorBrush(elementos[i] == elementosOrdenados[i] ? colorCorrecto : colorIncorrecto)
                    };

                    // Posicionamos el círculo en el Canvas
                    Canvas.SetLeft(circulo, i * espacioEntreFiguras + (espacioEntreFiguras - tamañoFijo) / 2);
                    Canvas.SetTop(circulo, alturaCanvas - alturaFigura - tamañoFijo / 2); // Alineamos desde la parte superior
                    cvCanvas.Children.Add(circulo);
                }
                else
                {
                    // Dibujamos rectángulos
                    Rectangle rectangulo = new Rectangle
                    {
                        Width = espacioEntreFiguras - (iudGrosor.Value ?? 0) * 2, // Restamos el grosor del borde por cada lado
                        Height = alturaFigura,
                        Stroke = new SolidColorBrush(Colors.Black), // Color del borde
                        StrokeThickness = iudGrosor.Value ?? 0,
                        Fill = new SolidColorBrush(elementos[i] == elementosOrdenados[i] ? colorCorrecto : colorIncorrecto) // Color interior
                    };

                    // Posicionamos el rectángulo en el Canvas
                    Canvas.SetLeft(rectangulo, i * espacioEntreFiguras); // Ajustamos la posición X
                    Canvas.SetTop(rectangulo, alturaCanvas - alturaFigura); // Ajustamos la posición Y desde la parte inferior del Canvas
                    cvCanvas.Children.Add(rectangulo);
                }
            }
        }





    }
}
