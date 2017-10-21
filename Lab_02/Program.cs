using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab_02
{
    class Program
    {
        static void Main(string[] args)
        {
            RunParallelTasks();
            Console.WriteLine("Finalizando ejecución de tareas en paralelo...");
            Console.WriteLine("Presione <enter> para finalizar");
            Console.ReadLine();
        }

        static void RunParallelTasks()
        {
            WriteToConsole("Ejecutando tareas en paralelo.");
            Parallel.Invoke(
                () => WriteToConsole("Tarea 1."),
                () => WriteToConsole("Tarea 2."),
                () => WriteToConsole("Tarea 3."));
        }

        static void WriteToConsole(string message)
        {
            Console.WriteLine($"{message}. {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(5000);
            Console.WriteLine($"Fin de la tarea. {Thread.CurrentThread.ManagedThreadId}");
        }
    }
}
