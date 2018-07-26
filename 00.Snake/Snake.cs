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
    public class Node
    {
        public Node previous = null;
        public Point value = new Point(0, 0);
        public Node next = null;
    }

    public class Snake//Збереження положення та рух змійки
    {
        public Node firstElement;
        public Node lastElement;
        public int length = 0;
        public int count;
        public bool alive = false;

        public Snake()
        {
            SnakeReincarnation();
            alive = false;
        }

        public void SnakeReincarnation()
        {
            firstElement = null;
            lastElement = null;
            length = 0;
            count = 0;
            alive = true;

            for (int i = 12; i < 18; i++)
            {
                AddToTheEnd(new Point(10, i));
            }
        }

        public void AddToTheEnd(Point value)//Додаємо елемент в кінець змійки(використовується при створенні нової змійки)
        {
            if (lastElement == null)
            {
                lastElement = new Node();
                lastElement.previous = null;
                lastElement.next = null;
                lastElement.value = value;
                firstElement = lastElement;
            }

            else
            {
                Node temp = new Node();
                temp.value = value;
                temp.next = null;
                temp.previous = lastElement;
                lastElement.next = temp;
                lastElement = temp;
            }
            count++;

        }

        public void AddToTheBegining(Point value)//Додаємо елемент в початок зміки(рух вперед)
        {
            if (firstElement == null)
            {
                firstElement = new Node();
                firstElement.previous = null;
                firstElement.next = null;
                firstElement.value = value;
                lastElement = firstElement;
            }
            else
            {
                Node temp = new Node();
                temp.value = value;
                temp.next = firstElement;
                temp.previous = null;
                firstElement.previous = temp;
                firstElement = temp;
            }
            count++;
        }

        public Node FindElementByIndex(int index)//Шукаємо елемент за індексом
        {
            Node temp;
            temp = firstElement;

            for (int i = 0; i < index; i++)
            {
                temp = temp.next;
                if (temp == null)
                {
                    throw new IndexOutOfRangeException();
                }
            }
            return temp;
        }

        public void Remove(int index)//Видаляємо елемент за індексом
        {

            Node elementToRemove = FindElementByIndex(index);
            if (elementToRemove.next != null)
            {
                FindElementByIndex(index + 1).previous = elementToRemove.previous;
            }
            else
            {
                lastElement = elementToRemove.previous;
            }

            if (index != 0)
            {
                FindElementByIndex(index - 1).next = elementToRemove.next;
            }
            else
            {
                firstElement = elementToRemove.next;
            }
            count--;
        }
        //------------------------------------------

        public void Move(map field)//Рух змійки
        {
            bool tasty = false;//На цьому кроці змійка ще не їла
            if (alive)
            {
                int X = this.firstElement.value.X;//Координати голови змійки
                int Y = this.firstElement.value.Y;//
                if (field.way == "U")                      //Нові координати голови, залежно від напрямку руху
                {                                          //
                    Y = Y - 1;                             //
                }                                          //
                                                           //
                if (field.way == "D")                      //
                {                                          //
                    Y = Y + 1;                             //
                }                                          //
                                                           //
                if (field.way == "L")                      //
                {                                          //
                    X = X - 1;                             //
                }                                          //
                                                           //
                if (field.way == "R")                      //
                {                                          //
                    X = X + 1;                             //
                }                                          //

                if (field.step[X, Y].value == -1 || field.step[X, Y].value == 1)//Якщо на шляху стіна аба змійка, то змійка вмирає
                {
                    field.step[X, Y].value = -10;
                    alive = false;
                }
                else
                {
                    if (field.step[X, Y].value == 10) { tasty = true; field.FoodGen(); length++; }//Якщо на шляху їжа, то змійка їсть
                    AddToTheBegining(new Point(X, Y));
                    field.step[X, Y].value = 1;
                }

                if (tasty == false)//Якщо зміка так і не поїла, то підбираємо хвіст
                {
                    Remove(count - 1);
                    X = this.lastElement.value.X;
                    Y = this.lastElement.value.Y;
                    field.step[X, Y].value = 0;
                }
            }
        }
    }
}
