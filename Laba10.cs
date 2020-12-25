using System;
using System.Linq;

namespace Game
{
    class Program
    {

        static int[] dims = new int[] { 4, 6, 8 };
        static int N = 4;

        static int WIDTH = 80;
        static int HEIGHT = 30;

        static bool[,] shown;
        //Item1 - index, Item2 - value
        static Tuple<int, int>[,] desk;

        static int TIME = 2000;

        static int STEPS = 0;

        static void Main(string[] args)
        {
            SetWindow();

            ShowTitle();

            PromptDimension();

            Console.Clear();
            ShowTitle();

            GenerateTable();
            ShowTable();
            //Console.Read();
            System.Threading.Thread.Sleep(TIME);

            MainLoop();

            ShowCongratz();
        }

        static void MainLoop()
        {
            bool[] open = new bool[2];
            int[] poss = new int[4];

            while (!AllOpen())
            {
                Console.Clear();
                ShowTitle();

                HideTable();
                if (open[0] && open[1])
                {
                    if (!CheckIfSame(poss))
                    {
                        shown[poss[0], poss[1]] = false;
                        shown[poss[2], poss[3]] = false;
                    }
                    open[0] = false;
                    open[1] = false;
                    System.Threading.Thread.Sleep(1000);
                    continue;
                }

                Console.WriteLine();
                Console.Write("Please choose card number: ");
                int num = Convert.ToInt32(Console.ReadLine());
                STEPS++;

                int ind = -1;
                int iv = -1, jv = -1;
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                        if (desk[i, j].Item1 == num)
                        {
                            iv = i;
                            jv = j;
                            ind = desk[i, j].Item2;
                        }
                }

                if (shown[iv, jv])
                {
                    continue;
                }

                if (!open[0])
                {
                    open[0] = true;
                    poss[0] = iv;
                    poss[1] = jv;
                }
                else if (!open[1])
                {
                    open[1] = true;
                    poss[2] = iv;
                    poss[3] = jv;
                }

                shown[iv, jv] = true;
            }

            Console.Clear();
            ShowTitle();

            HideTable();
        }

        static bool CheckIfSame(int[] poss)
        {
            if (desk[poss[0], poss[1]].Item2 == desk[poss[2], poss[3]].Item2)
                return true;
            return false;
        }

        static bool AllOpen()
        {
            bool allTrue = false;
            foreach (bool b in shown)
            {
                if (b)
                {
                    allTrue = true;
                }
                else
                {
                    allTrue = false;
                    break;
                }
            }
            return allTrue;
        }

        static void HideTable()
        {
            int counter = 1;
            string filling;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (shown[i, j])
                    {
                        filling = desk[i, j].Item2.ToString();
                        if (filling.Length == 1 && (counter.ToString()).Length == 1)
                            filling = " " + filling;
                        Console.Write("\t{0}. {1}", counter++, filling);
                        continue;
                    }

                    filling = " XX";
                    if ((counter.ToString()).Length != 1)
                        filling = "XX";
                    Console.Write("\t{0}. {1}", counter++, filling);
                }
                Console.WriteLine();
            }
        }

        static void ShowTable()
        {
            int counter = 1;
            string filling;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    filling = desk[i, j].Item2.ToString();
                    if (filling.Length == 1 && (counter.ToString()).Length == 1)
                        filling = " " + filling;

                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("\t{0}. ", counter++);
                     Console.BackgroundColor = ConsoleColor.Yellow;
                     Console.Write(filling);
                    Console.Write("\t{0}. {1}", counter++, filling);
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }

        static void GenerateTable()
        {
            int counter = 1;
            desk = new Tuple<int, int>[N, N];

            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    desk[i, j] = Tuple.Create(counter++, -1);

            int ir, jr;
            Random r = new Random();
            int val = 1;
            while (!AllFilled())
            {
                for (int t = 0; t < 2; t++)
                {
                    ir = r.Next(N);
                    jr = r.Next(N);
                    
                    while (desk[ir, jr].Item2 != -1)
                    {
                        ir = r.Next(N);
                        jr = r.Next(N);
                    }

                     desk[ir, jr] = new Tuple<int, int>(desk[ir, jr].Item1, val);
                }
                val++;
            }

            shown = new bool[N, N];
        }

        static bool AllFilled()
        {
            foreach (Tuple<int, int> t in desk)
            {
                if (t.Item2 == -1)
                    return false;
            }
            return true;
        }

        static void PromptDimension()
        {
            string result = string.Join(",", dims.Select(x => x.ToString()).ToArray());
            Console.Write("Please choose the dimension - {" + result + "}: ");

            N = Convert.ToInt32(Console.ReadLine());
        }

        static void ShowCongratz()
        {
            Console.WriteLine();
            for (int i = 0; i < WIDTH; i++)
                Console.Write("*");

            string title = "CONGRATULATIONS!!! YOU'VE OPENED ALL CARDS!!!";
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (title.Length / 2)) + "}", title));

            string steps = "AMOUNT OF STEPS IS " + STEPS;
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (steps.Length / 2)) + "}", steps));

            for (int i = 0; i < WIDTH; i++)
                Console.Write("*");
            Console.WriteLine();
        }

        static void ShowTitle()
        {
            for (  int i = 0; i < WIDTH; i++)
                Console.Write("*");

            string title = "TRAIN YOUR MEMORY";
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (title.Length / 2)) + "}", title));

            for (int i = 0; i < WIDTH; i++)
                Console.Write("*");
            Console.WriteLine();
        }

        static void SetWindow()
        {
            Console.Clear();
            Console.SetWindowSize(WIDTH, HEIGHT);
            Console.BufferWidth = WIDTH;
            Console.BufferHeight = HEIGHT;
        }

    }
}
