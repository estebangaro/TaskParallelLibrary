using System;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
namespace Lab01_20180207
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CreateTask();
        }

        //Ejercicio 2
        public void AddMessage(string message)
        {
            int ManageThreadId = Thread.CurrentThread.ManagedThreadId;
            Messages.Dispatcher.Invoke(() =>
            Messages.Content +=
                $"Mensaje: {message}, " +
                $"Hilo actual: {ManageThreadId}{Environment.NewLine}");
        }

        // Ejercicio 1
        public void CreateTask()
        {
            // Tarea 2
            Task T;
            var Code = new Action(ShowMessage);
            T = new Task(Code);
            Task T2 = new Task(delegate
            {
                MessageBox.Show("Ejecutando una tarea en un delegado anónimo");
            });
            // Tarea 3
            // Task T3 = new Task(delegate { ShowMessage(); });
            // Creando una tarea a partir de un delegado que invoca
            // a un método anónimo utilizando las sintaxis abreaviada
            // expresiones LAMBDA para la creación de métodos anónimos.
            Task T3 = new Task(() => ShowMessage());
            Task T4 = new Task(() => MessageBox.Show("Ejecutando la tarea 4."));
            Task T5 = new Task(() =>
            {
                DateTime CurrentDate = DateTime.Today;
                DateTime StartDate = CurrentDate.AddDays(30);
                MessageBox.Show($"Tarea 5. Fecha calculada:{StartDate}");
            });
            Task T6 = new Task((message) => MessageBox.Show(message.ToString()),
                "Expresión lambda con parámetros.");
            // Ejercicio 2
            Task T7 = new Task(() => AddMessage("Ejecutando la tarea 7"));
            T7.Start();
            AddMessage("En el hilo principal");
            Task T8 = Task.Factory.StartNew(() => AddMessage("Tarea 8 iniciada con TaskFactory"));
            Task T9 = Task.Run(() => AddMessage("Tarea 9 ejecutada con Task.Run"));
            // Ejercicio propuesto
            Task T9_0 = Task.Run(() =>
            {
                Task T9_1 = new Task(() =>
                {
                    Thread.Sleep(5000);
                    MessageBox.Show($"Ejecutando tarea T9_1, Hilo: {Thread.CurrentThread.ManagedThreadId}");
                });
                AddMessage($"Ejecutando trabajo de tarea creada y encolada por Task.Run, Id T9_1: {T9_1.Id}");
                T9_1.Start();

                return T9_1;
            });
            Task.Run(() =>
            {
                T9_0.Wait();
                AddMessage($"En hilo principal después de encolar a T9_0 con estatus {T9_0.Status}");
            });
            // Fin ejercicio propuesto


            // Tarea 3 Esperar la ejecución de las tareas (Continuar).
        }

        // Ejercicio 1
        public void ShowMessage()
        {
            MessageBox.Show("Ejecutando método ShowMessage");
        }
    }
}
