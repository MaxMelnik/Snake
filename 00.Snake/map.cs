using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace _00.Snake
{
    public class map//Ігрове поле, яке буде виводитися на екран
    {
        public int N = 20;                                //Розміри мапи
        public int M = 20;                                //
        public Node[,] step = new Node[20, 20];
        public string way = "U";

        public class Node                                 //Комірка мапи
        {
            public int value = 0;

            public Node()
            {
                this.value = 0;
            }
        }

        Random rnd = new Random();

        public void FoodGen()//Генеруємо їжу
        {
            int X = rnd.Next(1, 19);
            int Y = rnd.Next(1, 19);
            while (step[X, Y].value != 0)
            {
                X = rnd.Next(1, 19);
                Y = rnd.Next(1, 19);
            }

            step[X, Y].value = 10;
        }

        public map()
        {
            GenMap();
        }

        public void GenMap()//Генеруємо нову мапу
        {
            this.way = "U";//Початковий шлях - вгору
            for (int i = 0; i < N; i++)
                for (int j = 0; j < M; j++)
                {
                    step[i, j] = new Node();
                    step[i, j].value = 0;
                    if (i == 0 || j == 0 || i == N - 1 || j == M - 1)
                    {
                        step[i, j].value = -1;
                    }
                }
            int wallCount = 0;
            while (wallCount < rnd.Next(7, 11))//Кількіть перешкод - від семи до одинадцяти 
            {
                int X = rnd.Next(1, 19);
                int Y = rnd.Next(1, 19);
                if (step[X, Y].value == 0 && step[X, Y + 1].value != 1)
                {
                    step[X, Y].value = -1;
                    wallCount++;
                }
            }


            for (int i = 12; i < 17; i++)//Початкове положення змійки 
            {
                step[10, i].value = 1;
            }

            FoodGen();
        }


    }
}
