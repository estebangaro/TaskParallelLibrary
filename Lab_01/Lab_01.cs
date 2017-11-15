using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

namespace Lab_01
{
    public partial class Lab_01 : Form
    {
        public Lab_01() => InitializeComponent();

        private CancellationTokenSource[] Cts;
        private CancellationToken[] Ct;
        private Task[] LongTask;
        private Task SecondTask;
        private CancellationTokenSource cts;
        private CancellationToken ct;

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

        private void RunTask(byte taskNumber)
        {
            WriteToOutput($"Iniciando tarea {taskNumber}");
            Thread.Sleep(10000);
            WriteToOutput($"Finalizando tarea {taskNumber}");
        }

        private void RunTaskGroup()
        {
            Task[] Tasks =
            {
                Task.Run(() => RunTask(1)),
                Task.Run(() => RunTask(2)),
                Task.Run(() => RunTask(3)),
                Task.Run(() => RunTask(4)),
                Task.Run(() => RunTask(5))
            };

            Task.Run(delegate
            {
                WriteToOutput("Esperando a que finalicen todas las tareas");
                int index = Task.WaitAny(Tasks);
                WriteToOutput($"La tarea {index} ha finalizado por lo tanto se reanuda la ejecución del hilo secundario.");
            });
        }

        private void ReturnTaskValue()
        {
            //Task<int> TaskInt = Task.Run(delegate ()
            //{
            //    WriteToOutput("Generando un valor entero aleatorio...");
            //    Thread.Sleep(10000);
            //    WriteToOutput("Valor de mi propiedad Result: ");
            //    WriteToOutput($"Result: {this.Result}");
            //    return new Random().Next(1000);
            //});
            Task<int> TaskInt = new Task<int>(delegate
            {
                WriteToOutput("Generando un valor entero aleatorio...");
                Thread.Sleep(10000);
                return new Random().Next(1000);
            });

            Task.Run(delegate
            {
                WriteToOutput($"Esperar el resultado de la tarea...");
                WriteToOutput($"Valor devuelto por la tarea: {TaskInt.Result}");
                WriteToOutput($"Fin de la ejecución del método ReturnTaskValue");
            });
        }

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

        private void ExecuteLongWork(int seconds, int[] ArrayInts, 
            CancellationToken Ct, int taskNumber)
        {
            for (int i = 0; i < ArrayInts.Length && !Ct.IsCancellationRequested; i++)
            {
                AddMessage($"Procesando entero en tarea [{taskNumber}] {ArrayInts[i]}");
                Thread.Sleep(seconds * 1000);
            }

            if (Ct.IsCancellationRequested)
            {
                // Lógica de solicitud de cancelación.
                AddMessage("Cancelando proceso...");

                if (taskNumber == 1)
                    throw new OperationCanceledException(Ct);
                else if (taskNumber == 2)
                    throw new DivideByZeroException();
                else
                    throw new NotImplementedException();
                //Ct.ThrowIfCancellationRequested();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cts = new CancellationTokenSource[] { new CancellationTokenSource(), new CancellationTokenSource(),
                new CancellationTokenSource() };
            Ct = new CancellationToken[] { Cts[0].Token, Cts[1].Token, Cts[2].Token };

            int[] ArrayInts;
            ArrayInts = Enumerable.Range(1, 10).ToArray();

            SecondTask = Task.Run(delegate {
                LongTask = new Task[] { Task.Run(() => ExecuteLongWork(3, ArrayInts, Ct[0], 1), Ct[0]),
                Task.Run(() => ExecuteLongWork(5, ArrayInts, Ct[1], 2), Ct[1]),
                Task.Run(() => ExecuteLongWork(6, ArrayInts, Ct[2], 3), Ct[2])
                };
                try
                {
                    Task.WaitAll(LongTask);
                }
                catch(AggregateException ae)
                {
                    AddMessage("Se han manejado las siguientes exepciones en Hilo Secundario");
                    foreach(var Inner in ae.InnerExceptions)
                    {
                        if (!(Inner is TaskCanceledException))
                            AddMessage($"Exepción: {Inner.Message}");
                        else
                            AddMessage("TaskCanceledExeption manejada");
                    }
                }
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Cts[0].Cancel();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int i = 1;
            foreach (var Task in LongTask)
                AddMessage($"Estatus LongTask {i++}: {Task.Status}");
            AddMessage($"Estatus LongTask SecondTask: {SecondTask.Status}");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Cts[1].Cancel();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Cts[2].Cancel();
        }

        private void continuacion_Click(object sender, EventArgs e)
        {
            // RunParallelTasks();
            AddMessage($"Ejecutando RunContinuationTasks. {Thread.CurrentThread.ManagedThreadId}");
            // ParallelLoopIterate();
            // RunLINQ();
            // RunPLINQ();
            Task.Run(delegate { RunContinuationTasks(); });
            AddMessage("Finalizando ejecución de RunContinuationTasks...");
        }

        static List<string> GetProductNames()
        {
            Thread.Sleep(3000);
            throw new NotImplementedException();
            return new List<string> { "PlayStation 4", "Tomb Raider", "PES", "The Last Of Us", "FIFA 18" };
        }

        void RunContinuationTasks()
        {
            Task<List<string>> GameNamesTask = new Task<List<string>>(
                    () => GetProductNames());
         
            Task<int> ProcessGameNames = GameNamesTask.ContinueWith(
                gameNameTask => {
                    AddMessage("Ejecutando tarea de continuación");
                    return ProcessData(gameNameTask.Result);
                });
            //try
            //{
                GameNamesTask.Start();
            //}
            //catch { AddMessage("Controlando exepcion"); }
            //try
            //{
            //    GameNamesTask.Wait();
            //}
            //try
            //{
                AddMessage($"El número de nombres de juegos procesados es: {ProcessGameNames.Result}");
            //}
            //catch (AggregateException ex)
            //{
            //    AddMessage($"Exepción controlada: {ex.Message}");
            //}
        }

        int ProcessData(List<string> GameNames)
        {
            int i = 0;
            if (GameNames != null)
            {
                foreach (string name in GameNames)
                {
                    AddMessage($"Nombre ({++i}): {name}");
                }

                return GameNames.Count;
            }
            else
            {
                AddMessage($"Sin información de nombres de juego");
                return i;
            }
        }

        int ProcessData(Task<List<string>> GameNames)
        {
            // List<string> gamesnames = GameNames.Result;
            int i = 0;
            if (GameNames.Status != TaskStatus.Faulted)
            {
                foreach (string name in GameNames.Result)
                {
                    AddMessage($"Nombre ({++i}): {name}");
                }

                return GameNames.Result.Count;
            }
            else
            {
                AddMessage($"Sin información de nombres de juego");
                return i;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AddMessage("Creando tareas...");
            ProductImporter productImporterObj = new ProductImporter();
            ProductInfo productImporterObj2 = new ProductInfo { Id = -75, Name = "Esteban GaRo"};
            cts = new CancellationTokenSource();
            ct = cts.Token;

            Task<ProductInfo> ProcessTask = Task.Factory.StartNew(product =>
            {
                AddMessage("Ejecutando Process...");
                ProductInfo p = productImporterObj.Process((ProductInfo)product, ct);
                AddMessage("Finalizando Process...");

                return p;
            }, productImporterObj2, ct);

            Task SaveTask = ProcessTask.ContinueWith(delegate (Task<ProductInfo> AntecendtTask)
            {
                if (AntecendtTask.Status != TaskStatus.Canceled)
                {
                    AddMessage($"Ejecutando Save con ProductInfo procesado...{AntecendtTask.Result}");
                    productImporterObj.Save(AntecendtTask);
                    AddMessage("Finalizando Save...");
                }
                else
                {
                    AddMessage("Se ha suspendido la ejecución de Save por cancelación bajo demanda de process" +
                        $" con ProductInfo procesado...");
                }
            });

            Task.Run(() =>
            {
                try
                {
                    ProcessTask.Wait();
                }
                catch (AggregateException ex)
                {
                    AddMessage("Procesando exepción: " + ex.Message);
                }
            });
        }

        private void button7_Click(object sender, EventArgs e)
        {
            cts.Cancel();
        }

        //private void Lab_01_Load(object sender, EventArgs e) => ReturnTaskValue(); //RunTaskGroup(); //CreateTask();
    }

    public class ProductImporter : IProductImporter
    {
        public ProductInfo Process(ProductInfo product, CancellationToken token)
        {
            ProductInfo Result = new ProductInfo();
            ProcessProductInfo(product, Result, token);
            return Result;
        }

        private void ProcessProductInfo(ProductInfo product, ProductInfo result, CancellationToken Token)
        {
            Thread.Sleep(2000);
            Token.ThrowIfCancellationRequested();
            result.Id = product.Id + 100;
            Thread.Sleep(3000);
            Token.ThrowIfCancellationRequested();
            result.Name = product.Name.ToUpper();
        }

        public void Save(Task<ProductInfo> t)
        {            
            Thread.Sleep(3000);
        }
    }
}