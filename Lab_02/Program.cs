using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
// Tarea 1,2,3 completadas.
namespace Lab_02
{
    class Program
    {
        static void Main(string[] args)
        {
            // RunParallelTasks();
            Console.WriteLine($"Ejecutando ciclos de iteración en paralelo. {Thread.CurrentThread.ManagedThreadId}");
            ParallelLoopIterate();
            Console.WriteLine("Finalizando ejecución de tareas en paralelo...");
            Console.WriteLine("Presione <enter> para finalizar");
            Console.ReadLine();
        }

        static void RunParallelTasks()
        {
            Console.WriteLine($"Ejecutando tareas en paralelo. {Thread.CurrentThread.ManagedThreadId}");
            try
            {
                Parallel.Invoke(
                    () => WriteToConsole("Tarea 1."),
                    () => WriteToConsole("Tarea 2."),
                    () => WriteToConsole("Tarea 3."));
            }catch(AggregateException ex)
            {
                Console.WriteLine("Por procesar " + ex.InnerExceptions.Count + " exepciones.");
                foreach(var exe in ex.InnerExceptions)
                {
                    Console.WriteLine("Exepción: " + exe.Message);
                }
            }
        }

        static void ParallelLoopIterate()
        {
            int[] ArrayOfInts = new int[5];
            Parallel.For(0, 5, delegate (int index)
            {
                ArrayOfInts[index] = index * index;
                WriteToConsole($"Calculando el cuadrado de {index}");
            });

            WriteToConsole("Mostrando los cuadrados calculados...");
            Parallel.ForEach(ArrayOfInts, intNumber =>
                WriteToConsole($"Cuadrado de {Array.IndexOf(ArrayOfInts, intNumber)}: {intNumber}"));
        }

        static void WriteToConsole(string message)
        {
            if (message == "Tarea 2.")
                throw new NotImplementedException();
            Console.WriteLine($"{message}. {Thread.CurrentThread.ManagedThreadId}");
            // Thread.Sleep(5000);
            // Console.WriteLine($"Fin de la tarea. {Thread.CurrentThread.ManagedThreadId}");
        }
    }
}
