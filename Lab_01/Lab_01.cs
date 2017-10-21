﻿using System;
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
                LongTask = new Task[] { Task.Run(() => ExecuteLongWork(3, ArrayInts, Ct[0], 1), Ct[1]),
                Task.Run(() => ExecuteLongWork(5, ArrayInts, Ct[0], 2), Ct[0]),
                Task.Run(() => ExecuteLongWork(6, ArrayInts, Ct[0], 3), Ct[0])
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

        //private void Lab_01_Load(object sender, EventArgs e) => ReturnTaskValue(); //RunTaskGroup(); //CreateTask();
    }
}