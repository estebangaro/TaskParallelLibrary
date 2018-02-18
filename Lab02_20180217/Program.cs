using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Lab02_20180217
{
    class Program
    {
        static void Main(string[] args)
        {
            RunParallelTasks();
            Console.WriteLine("Presione enter para finalizar");
            Console.ReadLine();
            // Continuar con tarea 3, ejercicio 1.
        }

        // Tarea 2
        private static void RunParallelTasks()
        {
            Console.WriteLine($"Thread: {Thread.CurrentThread.ManagedThreadId}, ejecutar tareas en paralelo");
            Parallel.Invoke(
                () => { WriteToConsole("Tarea 1"); },
                () => { WriteToConsole("Tarea 2", 10000); },
                () => { WriteToConsole("Tarea 3"); });
            // Ejercicio propuesto - Generación de excepciones no controladas.
            /* Console.WriteLine("Por lanzar tarea");
            Task<int> demo =
                Task<int>.Run(() => { WriteToConsole("Tarea demo"); throw new Exception("Excepción demo"); return 25; });
            int resultado = demo.Result; */

        }
        // Tarea 2
        private static void WriteToConsole(string message, int mseconds = 5000)
        {
            Console.WriteLine($"{message} - Thread:{Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(mseconds);
            Console.WriteLine($"Fin de la tarea - Thread:{Thread.CurrentThread.ManagedThreadId}");
        }
    }
}
