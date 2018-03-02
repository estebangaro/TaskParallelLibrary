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

namespace Lab03_20180302
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Task tarea;
        public MainWindow()
        {
            InitializeComponent();
            tarea =
            Task.Run(() =>
            MuestraMensajeAsync());

            // Ejercicio Propuesto.
            // CreateUIControlInnerTask();
        }

        // Ejercicio 1
        private void ObtenerResultado_Click(object sender, RoutedEventArgs e)
        {
            // ObtenerResultado_ClickEjercicio1();
            ObtenerResultado_ClickEjercicio2();
        }

        // Ejercicio 1
        private void ObtenerResultado_ClickEjercicio1()
        {
            Delegate MostrarMensajeDelegado =
                // new Action<string>(MostrarMensaje);
                new MostrarDelegado(MostrarMensaje);
            Task.Run(delegate ()
            {
                string Resultado = "Resultado obtenido";
                this.Resultado.Dispatcher
                    .BeginInvoke(MostrarMensajeDelegado, Resultado);
            });
        }

        // Ejercicio 2
        private async void ObtenerResultado_ClickEjercicio2()
        {
            MostrarEnConsolaDepuracion($"Hilo que ejecuta el manejo de evento " +
                $"{System.Threading.Thread.CurrentThread.ManagedThreadId}");
            Resultado.Content = "Obteniendo un número entero aleatorio menor a 5000...";
            Task<int> T = ObtenerNumeroAleatorio();
            // Queda a la espera del RESULTADO de la tarea.
            // EDITADO: Queda a la espera de la TAREA.
            MostrarEnConsolaDepuracion($"Hilo que ejecuta antes de await " +
                $"{System.Threading.Thread.CurrentThread.ManagedThreadId}");
            int entero = await T;
            MostrarEnConsolaDepuracion($"Hilo que ejecuta después de await " +
                $"{System.Threading.Thread.CurrentThread.ManagedThreadId}");
            Resultado.Content = $"El número entero aleatorio menor a 5000 es: {entero}";
        }

        private async Task<int> ObtenerNumeroAleatorio()
        {
            int T = await Task<int>.Run(delegate ()
            {
                MostrarEnConsolaDepuracion($"Hilo que ejecuta la tarea {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                // Simula una operación de larga duración no relacionada a la CPU.
                System.Threading.Thread.Sleep(10000);

                return new Random().Next(5000);
            });

            return T;
        }

        // Ejercicio 1
        private void MostrarMensaje(string mensaje)
        {
            Resultado.Content = mensaje;
        }

        public delegate void MostrarDelegado(string Resultado);

        // Ejercicio Propuesto, Ejercicio 1.
        // ERROR: 
        // El subproceso de llamada debe ser STA, ya que muchos componentes de la interfaz de usuario lo requieren.
        private void CreateUIControlInnerTask()
        {
            MostrarEnConsolaDepuracion("Inicio de ejecución de tarea");
            Task.Run(delegate ()
            {
                // Se crea control UI en tarea.
                Label UIControl = new Label();
                UIControl.Content = "Estado Inicial";
                // Se modifica el estado de la propiedad "Content" del control UI
                // en una tarea distinta (tarea anidada - enlazado de tareas).
                Task InnerTask = Task.Run(delegate ()
                {
                    MostrarEnConsolaDepuracion($"Estado de control UI, " +
                        $"antes de modificación en tarea anidada: {UIControl.Content}");
                    UIControl.Content = "Resultado obtenido";
                    MostrarEnConsolaDepuracion($"Estado de control UI, " +
                        $"después de modificación en tarea anidada: {UIControl.Content}");
                });
            }).Wait();
            MostrarEnConsolaDepuracion("Fin de ejecución de tarea");
        }

        // Ejercicio Propuesto, Ejercicio 1.
        private void MostrarEnConsolaDepuracion(string mensaje)
        {
            System.Diagnostics.Debug.WriteLine(mensaje);
        }

        private void MuestraSaludo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Dsifrutando de la TPL :D");
        }

        private async Task MuestraMensajeAsync()
        {
            MostrarEnConsolaDepuracion($"Ejecutando método asíncrono, HILO: " +
                $"{System.Threading.Thread.CurrentThread.ManagedThreadId}");
            await Resultado.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                Resultado.Content = $"Calculando número entero..., " +
                    $"HILO {System.Threading.Thread.CurrentThread.ManagedThreadId}";
            }));
            Task<int> t = Task<int>.Run(() =>
             {
                 MostrarEnConsolaDepuracion($"Ejecutando operación de larga duración, HILO: " +
                     $"{System.Threading.Thread.CurrentThread.ManagedThreadId}");
                 System.Threading.Thread.Sleep(10000);
                 return 2509;
             });
            MostrarEnConsolaDepuracion($"Ejecutando método asíncrono antes de await, HILO: " +
                $"{System.Threading.Thread.CurrentThread.ManagedThreadId}");
            int entero = await t;
            MostrarEnConsolaDepuracion($"Ejecutando método asíncrono después de await, HILO: " +
                $"{System.Threading.Thread.CurrentThread.ManagedThreadId}");
            await Resultado.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                Resultado.Content = $"Número entero calculado: {entero}, " +
                $"HILO {System.Threading.Thread.CurrentThread.ManagedThreadId}";
            }));
            MostrarEnConsolaDepuracion($"Ejecutando método asíncrono después de await X2, HILO: " +
                $"{System.Threading.Thread.CurrentThread.ManagedThreadId}");
        }

        private void MuestraSaludo_Copy_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Estatus: {tarea.Status}");
        }
    }
}
