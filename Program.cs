using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPI;

namespace RVIP_5
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var env = new MPI.Environment(ref args))
            {
                if (MPI.Communicator.world.Rank == 0)
                {                   
                    Console.WriteLine("Исходный массив: ");
                    int[,] mass = new int[6, 6];
                    Random rand = new Random();
                    for (int i = 0; i < 6; i++)
                    {
                        for (int j = 0; j < 6; j++)
                        {
                            mass[i, j] = rand.Next(0, 9);
                            Console.Write(mass[i, j] + " ");
                        }
                        Console.WriteLine();
                    }
                    List<int> current_array_string = new List<int>();
                    List<int> to_multiply = new List<int>();
                    for (int i = 0; i < 6; i++)
                    {
                        for (int j = 0; j < 6; j++)
                        {
                            current_array_string.Add(mass[i, j]);
                        }
                        MPI.Communicator.world.Send(current_array_string, Communicator.world.Rank + i + 1, 0);
                        to_multiply.Add(MPI.Communicator.world.Receive<int>(Communicator.world.Rank + i + 1, 0));
                    }
                    int multiplications_sum = 0;
                    foreach (int j in to_multiply)
                    {
                        multiplications_sum += j;
                    }                    
                    Console.WriteLine("Сумма строк равна: " + multiplications_sum);
                }
                for (int i = 1; i < 7; i++)
                {
                    if (MPI.Communicator.world.Rank == i)
                    {
                        Console.WriteLine();
                        int multiplication = 1;
                        List<int> received_list = new List<int>();
                        received_list = MPI.Communicator.world.Receive<List<int>>(Communicator.world.Rank - i, 0);
                        for(int k = 0; k<received_list.Count(); k++)
                        {
                            multiplication = multiplication*received_list[k];
                        }
                        Communicator.world.Send(multiplication, 0, 0);
                    }
                }
            }
            Console.ReadLine();
        }
    }
}
