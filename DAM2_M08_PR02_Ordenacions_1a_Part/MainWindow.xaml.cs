﻿using System;
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
        private int[] elementos;
        private int delay;

        public MainWindow()
        {
            InitializeComponent();

            // Establecer colores por defecto para los ColorPickers
            colorCorrecte.SelectedColor = Colors.Green;
            colorIncorrecter.SelectedColor = Colors.Red;
            colorIntercanvi.SelectedColor = Colors.Yellow;

            // Si deseas configurar el color de fondo por defecto del Canvas:
            colorFons.SelectedColor = Colors.White; // O el color que prefieras
            cvCanvas.Background = new SolidColorBrush(colorFons.SelectedColor.Value);


            iudPausa.ValueChanged += iudPausa_ValueChanged;
        }

        private void iudPausa_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            delay = (int)e.NewValue; // Actualiza el valor de delay
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

        private void ActualizarColorFigura(int index)
        {
            // Comprobamos si la figura está en la posición correcta comparándola con la array ordenada
            int[] elementosOrdenados = elementos.OrderBy(x => x).ToArray();
            Color colorFigura = 
                elementos[index] == elementosOrdenados[index] 
                ? colorCorrecte.SelectedColor.Value 
                : colorIncorrecter.SelectedColor.Value;

            // Actualizamos el color de la figura
            if (cvCanvas.Children[index] is Shape figura)
            {
                figura.Fill = new SolidColorBrush(colorFigura);
            }
        }

        private async void btnOrdenar_Click(object sender, RoutedEventArgs e)
        {
            string metodoSeleccionado = (cbOrdenacion.SelectedItem as ComboBoxItem)?.Content.ToString();
            switch (metodoSeleccionado)
            {
                case "Bubble sort":
                    await BubbleSort();
                    break;
                case "Selection sort":
                    await SelectionSort();
                    break;
                case "Insertion sort":
                    await InsertionSort();
                    break;
                case "Counting sort":
                    await CountingSort();
                    break;
            }
        }

        private async Task IntercambiarFiguras(int index1, int index2)
        {
            // primero intercambiamos los valores en la array de elementos
            int temp = elementos[index1];
            elementos[index1] = elementos[index2];
            elementos[index2] = temp;

            // luego intercambiamos las figuras en el Canvas
            UIElement figura1 = cvCanvas.Children[index1];
            UIElement figura2 = cvCanvas.Children[index2];

            // Actualiza la posición de las figuras intercambiadas en el Canvas
            ActualizarPosicionFigura(figura1, index2);
            ActualizarPosicionFigura(figura2, index1);

            // Remueve los elementos de la colección de hijos de Canvas antes de intercambiar
            cvCanvas.Children.Remove(figura1);
            cvCanvas.Children.Remove(figura2);

            // Intercambia las figuras en la colección de hijos de Canvas
            cvCanvas.Children.Insert(index1, figura2);
            cvCanvas.Children.Insert(index2, figura1);

            // Temporalmente cambiamos el color de las figuras que se están intercambiando
            CambiarColorFiguraTemporal(index1, colorIntercanvi.SelectedColor.Value);
            CambiarColorFiguraTemporal(index2, colorIntercanvi.SelectedColor.Value);

            // Esperamos un poco para visualizar el intercambio
            await Task.Delay(delay);

            // Actualizamos el color de las figuras intercambiadas
            ActualizarColorFigura(index1);
            ActualizarColorFigura(index2);
        }

        private void CambiarColorFiguraTemporal(int index, Color color)
        {
            if (cvCanvas.Children[index] is Shape figura)
            {
                figura.Fill = new SolidColorBrush(color);
            }
        }

        private void ActualizarPosicionFigura(UIElement figura, int nuevoIndex)
        {
            double espacioEntreFiguras = cvCanvas.ActualWidth / elementos.Length;
            double nuevaPosX = nuevoIndex * espacioEntreFiguras;

            Canvas.SetLeft(figura, nuevaPosX);
        }

        ////////////////////// POSICIONAR /////////////////////////

        private void btnPosicionar_Click(object sender, RoutedEventArgs e)
        {
            // siempre limpiamos el canvas antes de pintar de nuevo
            cvCanvas.Children.Clear();

            int numeroDeElementos = iudElements.Value ?? 0; 
            bool invertido = checkInvertit.IsChecked == true;
            bool aleatorio = checkAleatori.IsChecked == true;

            elementos = Enumerable.Range(1, numeroDeElementos).ToArray();

            if (invertido)
            {
                Array.Reverse(elementos);
            }
            else if (aleatorio)
            {
                Random rng = new Random();
                // ordenamos de forma aleatoria y pasamos a arrauy
                elementos = elementos.OrderBy(x => rng.Next()).ToArray();
            }

            DibujarFiguras(elementos);
        }

        private void DibujarFiguras(int[] elementos)
        {
            // Obtenemos las dimensiones del Canvas
            double alturaCanvas = cvCanvas.ActualHeight;
            double anchoCanvas = cvCanvas.ActualWidth;

            double tamañoFijo = 30; // aqui ajusto el tamaño del circulito

            // calculo del espacio entre figuras
            double espacioEntreFiguras = anchoCanvas / elementos.Length;

            // buscamos el elemento máximo 
            int valorMaximo = elementos.Any() ? elementos.Max() : 1; // Evitamos la división por cero

            // cogemos los colores de los ColorPickels
            Color colorCorrecto = this.colorCorrecte.SelectedColor ?? Colors.Green;
            Color colorIncorrecto = this.colorIncorrecter.SelectedColor ?? Colors.Red;

            // solucion burra: crear un array ordenado apra comparar
            int[] elementosOrdenados = (int[])elementos.Clone();
            Array.Sort(elementosOrdenados);

            for (int i = 0; i < elementos.Length; i++)
            {
                double alturaFigura = (elementos[i] / (double)valorMaximo) * alturaCanvas;

                if ((cbFiguras.SelectedItem as ComboBoxItem)?.Content.ToString() == "Círculos")
                {
                    // pintamos círculos (elipses)
                    Ellipse circulo = new Ellipse
                    {
                        Width = tamañoFijo,
                        Height = tamañoFijo,
                        Fill = new SolidColorBrush(elementos[i] == elementosOrdenados[i] 
                        ? colorCorrecto 
                        : colorIncorrecto) // con esto estara de color correcto o incorrectto
                    };

                    // lo situamos en el Canvas
                    Canvas.SetLeft(circulo, i * espacioEntreFiguras + (espacioEntreFiguras - tamañoFijo) / 2);
                    Canvas.SetTop(circulo, alturaCanvas - alturaFigura - tamañoFijo / 2); // alineacion desde la parte superior
                    cvCanvas.Children.Add(circulo);
                }
                else
                {
                    // pintamos rectángulos
                    Rectangle rectangulo = new Rectangle
                    {
                        Width = espacioEntreFiguras - (iudGrosor.Value ?? 0) * 2, // grosor del borde
                        Height = alturaFigura,
                        Stroke = new SolidColorBrush(Colors.Black), 
                        StrokeThickness = iudGrosor.Value ?? 0,
                        Fill = new SolidColorBrush(elementos[i] == elementosOrdenados[i]
                        ? colorCorrecto 
                        : colorIncorrecto) 
                    };

                    // lo situamos en el Canvas y ajustamos la posicion
                    Canvas.SetLeft(rectangulo, i * espacioEntreFiguras); 
                    Canvas.SetTop(rectangulo, alturaCanvas - alturaFigura); 
                    cvCanvas.Children.Add(rectangulo);
                }
            }
        }

        ////////////////////// SORT ALGORITHMS /////////////////////////  

        private async Task BubbleSort()
        {
            int n = elementos.Length;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (elementos[j] > elementos[j + 1])
                    {
                        IntercambiarFiguras(j, j + 1);
                        cvCanvas.UpdateLayout();
                        
                        // Este es mi primer intento con mi primer sorting algorithm,
                        // probamos dandole un delay segun selecciones el usuario
                        await Task.Delay(delay);
                    }
                }
            }
        }

        private async Task SelectionSort()
        {
            int n = elementos.Length;
            for (int i = 0; i < n - 1; i++)
            {
                int minIndex = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (elementos[j] < elementos[minIndex])
                    {
                        minIndex = j;
                    }
                }
                if (minIndex != i)
                {
                    await IntercambiarFiguras(i, minIndex);
                }
            }
        }

        private async Task InsertionSort()
        {
            int n = elementos.Length;
            for (int i = 1; i < n; i++)
            {
                int key = elementos[i];
                int j = i - 1;

                while (j >= 0 && elementos[j] > key)
                {
                    elementos[j + 1] = elementos[j];
                    await IntercambiarFiguras(j, j + 1);
                    j = j - 1;
                }
                elementos[j + 1] = key;
            }
        }

        private async Task CountingSort()
        {
            int RANGE = elementos.Max() + 1; // Asegúrate de que el rango cubra todos los valores posibles
            int[] count = new int[RANGE];
            int[] output = new int[elementos.Length];

            // Inicializa el array de conteo
            for (int i = 0; i < RANGE; ++i)
            {
                count[i] = 0;
            }

            // Almacena el conteo de cada elemento
            for (int i = 0; i < elementos.Length; ++i)
            {
                ++count[elementos[i]];
            }

            // Cambia count[i] para que contenga la posición actual en output de este elemento
            for (int i = 1; i < RANGE; ++i)
            {
                count[i] += count[i - 1];
            }

            // Construye el array de salida
            for (int i = elementos.Length - 1; i >= 0; i--)
            {
                output[count[elementos[i]] - 1] = elementos[i];
                --count[elementos[i]];
            }

            // Copia los elementos a elementos[]
            for (int i = 0; i < elementos.Length; ++i)
            {
                int indexAnterior = Array.IndexOf(elementos, output[i]);
                elementos[i] = output[i];
                await IntercambiarFiguras(i, indexAnterior);
            }
        }



    }
}
