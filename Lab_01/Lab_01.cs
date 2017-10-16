using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_01
{
    public partial class Lab_01 : Form
    {
        public Lab_01() => InitializeComponent();

        private void CreateTask()
        {
            Task Task;
            //Action Code = new Action(ShowMessage);
            Action Code2 = ShowMessage;
            Task = new Task(Code2);
            //Task Task2 = new Task(
            //    delegate ()
            //    {
            //        MessageBox.Show("Ejecutando una tarea cuyo delegado " +
            //        "hace referencia a un método anónimo");
            //    });
            Task Task2 = new Task(
                new Action(
                delegate ()
                {
                    MessageBox.Show("Ejecutando una tarea cuyo delegado " +
                    "hace referencia a un método anónimo");
                }));
            //Task Task3 = new Task(
            //    new Action(() => ShowMessage()));

            //Task Task3 = new Task(
            //    () => { ShowMessage(); });

            Task Task3 = new Task(
                () => ShowMessage());

            Task Task4 = new Task(
                () => MessageBox.Show("Ejecutando la Tarea 4"));

            Task Task5 = new Task(
                () =>
                {
                    DateTime CurrentDate = DateTime.Today;
                    DateTime StartDate = CurrentDate.AddDays(30);
                    MessageBox.Show($"Tarea 5. Fecha calculada: {StartDate}");
                });

            Task Task6 = new Task(
                message => MessageBox.Show(message.ToString()), "Tarea 6. Expresión Lambda con parámetros");

            Task Task7 = new Task(() => AddMessage("Tarea 7. Ejecutando método AddMessage(string)"));
            Task7.Start();

            AddMessage("Hilo interfaz de usuario. Ejecutando método AddMessage(string)");

            Task Task8 =
                System.Threading.Tasks.Task.Factory.StartNew(
                    () => AddMessage("Tarea 8. Tarea iniciada con TaskFactory.StartNew(Action)"));

            Task Task9 =
                System.Threading.Tasks.Task.Run(() =>
                    AddMessage("Tarea 9. Tarea iniciado con método Task.Run(Action)"));

            Task Task10 = Task.Run(
                // Proporcionando argumento Action, mediante la definición
                // de un método anónimo.
                delegate ()
                {
                    AddMessage("Tarea 10. Ejecutando Tarea 10");
                    Thread.Sleep(10000);
                    AddMessage("Tarea 10. La Tarea 10 ha finalizado");
                });
            Task Task11 = Task.Run(delegate
            {
                AddMessage("Tarea 11. Esperando a que se ejecute Tarea 10.");
                Task10.Wait();
                AddMessage("Tarea 11. La Tarea 10 finalizó su ejecución.");
            });
        }

        private void ShowMessage() => MessageBox.Show("Ejecutando método ShowMessage");

        delegate void AddMessageDelegate();

        private void AddMessage(string message)
        {
            int ManagedThreadId = Thread.CurrentThread.ManagedThreadId;
            //Message.Invoke(new Action(delegate {
            //    Message.Text += $"Mensaje: {message}, " +
            //        $"Hilo Actual: {ManagedThreadId} \n";
            //}));
            this.Invoke(new AddMessageDelegate(() =>
                Message.Text += $"Mensaje: {message}, " +
                    $"Hilo Actual: {ManagedThreadId} \n"));
        }

        private void WriteToOutput(string message) =>
                System.Diagnostics.Debug.WriteLine($"Mensaje: {message}, " +
                    $"Hilo Actual: {Thread.CurrentThread.ManagedThreadId} \n");

        private void Lab_01_Load(object sender, EventArgs e) => CreateTask();
    }
}
