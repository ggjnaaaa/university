using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB1
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleKeyInfo kl;

            do
            {
                display();
                kl = Console.ReadKey(false);
                switch (kl.KeyChar.ToString())
                {
                    case "1":
                        tab();
                        break;
                    case "2":
                        test();
                        break;
                    case "3":
                        doubl();
                        break;
                    case "4":
                        arr();
                        break;
                    case "5":
                        prost();
                        break;
                    case "6":
                        mes();
                        break;
                    case "7":
                        stud();
                        break;
                    default:
                        break;
                }
            }
            while (kl.Key != ConsoleKey.Escape);
        }
        static void display()
        {
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1) таблица умножения;");
            Console.WriteLine("2) является ли сивол цифрой;");
            Console.WriteLine("3) обмен значениями;");
            Console.WriteLine("4) массив со случайными цифрами в выбранном диапазоне;");
            Console.WriteLine("5) простые числа от 0 до введенного числа;");
            Console.WriteLine("6) время года по введенному номеру месяца;");
            Console.WriteLine("7) студенты.");
            Console.WriteLine("Esc чтобы выйти");
        }
        static void tab() //1 приложение
        {
            Console.Clear();
            for (int x = 1; x < 11; x++)
            {
                Console.WriteLine("~~~~~~~~~~~~~~~~~");
                for (int y = 1; y < 11; y++)
                {
                    int t = x * y;
                    Console.WriteLine(x + "*" + y + "=" + t);
                }
            }
            Console.WriteLine("~~~~~~~~~~~~~~~~~");
            Console.ReadLine();
        }
        static void test() //2 приложение
        {
            Console.Clear();
            Console.WriteLine("Введите символ: ");
            string c = Console.ReadLine();
            switch (c)
            {
                case "1":
                    Console.WriteLine("Введённый символ является цифрой");
                    break;
                case "2":
                    Console.WriteLine("Введённый символ является цифрой");
                    break;
                case "3":
                    Console.WriteLine("Введённый символ является цифрой");
                    break;
                case "4":
                    Console.WriteLine("Введённый символ является цифрой");
                    break;
                case "5":
                    Console.WriteLine("Введённый символ является цифрой");
                    break;
                case "6":
                    Console.WriteLine("Введённый символ является цифрой");
                    break;
                case "7":
                    Console.WriteLine("Введённый символ является цифрой");
                    break;
                case "8":
                    Console.WriteLine("Введённый символ является цифрой");
                    break;
                case "9":
                    Console.WriteLine("Введённый символ является цифрой");
                    break;
                case "0":
                    Console.WriteLine("Введённый символ является цифрой");
                    break;
                default: Console.WriteLine("Введённый символ не является цифрой");
                    break;
            }
            Console.ReadLine();
        }
        static void doubl() //3 приложение
        {
            Console.Clear();
            double x = 84.1585;
            double y = -9.3647;
            Console.WriteLine("До обмена значениями x = " + x + " y = " + y);
            double z = x;
            x = y;
            y = z;
            Console.WriteLine("После обмена значениями x = " + x + " y = " + y);
            Console.ReadLine();
        }
        static void arr() //4 приложение
        {
            Console.Clear();
            Random rnd = new Random();
            Console.WriteLine("Введите минимальное значение: ");
            int min = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите максимально значение: ");
            int max = int.Parse(Console.ReadLine());
            int[,] a = new int[7, 5];
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    a[i, j] = rnd.Next(min, max);
                }
            }
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Console.Write(a[i, j] + "  ");
                }
                Console.Write("\n");
            }
            Console.ReadLine();
        }
        static void prost() //5 приложение
        {
            Console.Clear();
            Console.WriteLine("Введите число: ");
            int a = int.Parse(Console.ReadLine());
            int k = 0;
            for (int i = 1; i <= a; i++)
            {
                if ((a % i) == 0)
                {
                    k += 1;
                }
            }
            if (k == 2)
                Console.WriteLine("Число является простым");
            else
                Console.WriteLine("Число не является простым");
            for (int i = 0; i <= a; i++)
            {
                bool b = true;
                for (int j = 2; j < i; j++)
                {
                    if (i % j == 0)
                    {
                        b = false;
                    }
                }
                if (b == true)
                {
                    Console.Write("{0} ", i);
                }
            }
            Console.ReadLine();
        }
        static void mes() //6 приложение
        {
            Console.Clear();
            Console.WriteLine("Введите номер месяца: ");
            int m = int.Parse(Console.ReadLine());
            switch (m)
            {
                case 12: Console.Write("Зима"); break;
                case 1: Console.Write("Зима"); break;
                case 2: Console.Write("Зима"); break;
                case 3: Console.Write("Весна"); break;
                case 4: Console.Write("Весна"); break;
                case 5: Console.Write("Весна"); break;
                case 6: Console.Write("Лето"); break;
                case 7: Console.Write("Лето"); break;
                case 8: Console.Write("Лето"); break;
                case 9: Console.Write("Осень"); break;
                case 10: Console.Write("Осень"); break;
                case 11: Console.Write("Осень"); break;
                default: Console.WriteLine("Проверьте ввод");
                    break;
            }
            Console.ReadLine();
        }
        struct pubStud
        {
            public string FIO;
            public byte mark;
        }
        static void stud() //7 приложение
        {
            pubStud[] stude = new pubStud[10];
            int k = 0;
            int j = 0;
            do
            {
                start1:
                Console.Clear();
                Console.WriteLine("Добавить нового студента? Осталось место на {0} студентов", (10 - k));
                Console.WriteLine("0 - нет ~~~~~ 1 - да");
                ConsoleKeyInfo cl;
                cl = Console.ReadKey(false);
                switch (cl.KeyChar.ToString())
                {
                    case "0":
                        k = 10;
                        break;
                    case "1":
                        dobStud(ref stude[k].FIO, ref stude[k].mark);
                        j = k + 1;
                        break;
                    default:
                        goto start1;
                }
                k++;
            }
            while (k < 10) ;
            start2:
            Console.Clear();
            Console.WriteLine("Показать список студентов?");
            Console.WriteLine("0 - нет ~~~~~ 1 - показать худшего ~~~~~ 2 - показать лучшего ~~~~~ 3 - показать всех");
            ConsoleKeyInfo kl;
            kl = Console.ReadKey(false);
            switch (kl.KeyChar.ToString())
            {
                case "0":
                    break;
                case "1":
                    int a = 0;
                    int d = 2147483647;  
                    for (int i = 0; i != j; i++)
                    {
                        if (stude[i].mark < d)
                            a = i;
                            d = stude[i].mark;
                    }
                    listStud(stude[a].FIO, stude[a].mark);
                    Console.ReadKey();
                    break;
                case "2":
                    int b = 0;
                    int c = 0;
                    for (int i = 0; i != j; i++)
                    {
                        if (stude[i].mark > c)
                            b = i;
                            c = stude[i].mark;
                    }
                    listStud(stude[b].FIO, stude[b].mark);
                    Console.ReadKey();
                    break;
                case "3":
                    for (int l = 0; l != j; l++)
                    {
                        listStud(stude[l].FIO, stude[l].mark);
                    }
                    Console.ReadKey();
                    break;
                default:
                    goto start2;
            }
            Console.ReadKey();
        }
        static void dobStud(ref string FIO, ref byte mark)
        {
            Console.WriteLine("Введите ФИО студента: ");
            FIO = Console.ReadLine();
            Console.WriteLine("Введите оценки студента: ");
            mark = byte.Parse(Console.ReadLine());
        }
        static void listStud(string FIO, byte mark)
        {
            Console.WriteLine("ФИО студента: {0}", FIO);
            Console.WriteLine("Оценки студента: {0}", mark);
        }
    }
}