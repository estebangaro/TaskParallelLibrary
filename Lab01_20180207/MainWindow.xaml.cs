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
            // CreateTask();
            // RunTaskGroup();
            ReturnTaskValue();

            // Continuar con Ejercicio 4 Cancelando tareas de larga duración.
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
            //Task T9_0 = Task.Run(() =>
            //{
            //    Task T9_1 = new Task(() =>
            //    {
            //        Thread.Sleep(5000);
            //        MessageBox.Show($"Ejecutando tarea T9_1, Hilo: {Thread.CurrentThread.ManagedThreadId}");
            //    });
            //    AddMessage($"Ejecutando trabajo de tarea creada y encolada por Task.Run, Id T9_1: {T9_1.Id}");
            //    T9_1.Start();

            //    return T9_1;
            //});
            //Task.Run(() =>
            //{
            //    T9_0.Wait();
            //    AddMessage($"En hilo principal después de encolar a T9_0 con estatus {T9_0.Status}");
            //});
            // Fin ejercicio propuesto
            // Tarea 3.
            var T10 = Task.Run(() =>
            {
                AddMessage("Iniciando tarea 10");
                Thread.Sleep(10000);
                AddMessage("Finalizando tarea 10");
            });
            Task.Run(delegate ()
            {
                AddMessage("Esperando a la tarea 10.");
                T10.Wait();
                AddMessage("La tarea 10 ha finalizado.");
            });
        }

        // Ejercicio 2, Tarea 3.
        public void WriteToOutput(string message)
        {
            System.Diagnostics.Debug.WriteLine($"Mensaje: {message}{Environment.NewLine}" +
                $"Hilo actual: {Thread.CurrentThread.ManagedThreadId}");
        }

        // Ejercicio 2, Tarea 3.
        public void RunTask(byte taskNumber)
        {
            WriteToOutput($"Ejecutando tarea {taskNumber}");
            Thread.Sleep(10000);
            WriteToOutput($"Finalizando tarea {taskNumber}");
        }

        // Ejercicio 2, Tarea 3.
        public void RunTaskGroup()
        {
            Task[] TaskGroup = new Task[]
            {
                Task.Run(() => RunTask(1)),
                Task.Run(() => RunTask(2)),
                Task.Run(() => RunTask(3)),
                Task.Run(() => RunTask(4)),
                Task.Run(() => RunTask(5))
            };

            //WriteToOutput("Esperando a que todas las tareas finalicen");
            //Task.WaitAll(TaskGroup);
            //WriteToOutput("Todas las tareas han finalizado");

            WriteToOutput("Esperando a que al menos una tarea finalice");
            int i = Task.WaitAny(TaskGroup);
            WriteToOutput($"Al menos una tarea finalizó ({(i + 1)})");
        }

        // Ejercicio 3, Tarea 1.
        public void ReturnTaskValue()
        {
            Task<int> T;
            T = Task<int>.Run(() => new Random().Next(1000));
            WriteToOutput($"Valor devuelto por la tarea 1: {T.Result}");

            WriteToOutput("Esperar el resultado de la tarea 2");
            Task<int> T2 = Task<int>.Run(delegate ()
            {
                WriteToOutput("Obteniendo el número aleatorio...");
                Thread.Sleep(10000);
                return new Random().Next(1000);
            });
            WriteToOutput($"La tarea devolvió el valor: {T2.Result}");
            WriteToOutput("Fin de la ejecución del método ReturnTaskValue");
        }

        // Ejercicio 1
        public void ShowMessage()
        {
            MessageBox.Show("Ejecutando método ShowMessage");
        }
    }
}
