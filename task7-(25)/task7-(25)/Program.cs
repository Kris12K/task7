using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task7__25_
{
    class Program
    {
        //выделение памяти в рваном массиве под кодовые слова заданных длин
        static int[][] MakeJaggedArray(int[][] jagArr, int[] arrayOfLengths)
        {
            if (jagArr != null)
            {
                for (int i = 0; i < jagArr.GetLength(0); i++)
                {
                    jagArr[i] = new int[arrayOfLengths[i]];
                }
                return jagArr;
            }
            else
                throw
                     new NullReferenceException();
        }

        //проверка, существует ли однозначно декодируемый код для заданных длин по неравенству Крафта-Макмиллана
        static bool KraftMcMillanInequalityForBinaryCode(int[] lengths)
        {
            double x = 0;
            foreach (int a in lengths)
                x += Math.Pow(2, -a);
            if (x <= 1)
                return true;
            else
                return false;
        }

        //построение префиксного кода
        //первая строка заполняется нулями
        //i-ая строка копируется в (i+1)-ую строку (если длина (i+1)-ой строки больше, то оставшаяся часть заполняется нулями)
        //в (i+1)-ой строке двоичного число, соответствующее скопированной строке, увеличивается на единицу 
        //если к двоичному числу последней строки невозможно прибавить единицу, то построение префиксного кода невозможно
        static int[][] MakePreffixCodeWords(int[][] jagArr)
        {
            if (jagArr != null)
            {
                if (jagArr[0] == null)
                    throw new NullReferenceException();
                
                //первая строка уже заполнена нулями, поэтому цикл начинается с i=1 
                for (int i = 1; i < jagArr.GetLength(0); i++)
                {
                    if (jagArr[i] != null)
                    {
                        jagArr[i - 1].CopyTo(jagArr[i], 0);//копировать предыдущую строку в текущую строку

                        int count = jagArr[i - 1].Length - 1;//счетчик для прохода по массиву справа налево, 
                                                             //начиная с индекса, соответсвующего концу скопированной строки

                        do//увеличение скопированного двоичного числа на единицу
                          //если увеличить невозможно, то число будет равно нулю
                        {
                            if (jagArr[i][count] == 0)
                            {
                                jagArr[i][count] = 1;
                            }
                            else
                            {
                                jagArr[i][count] = 0;
                            }
                            count--;
                        } while (jagArr[i][count+1] == 0 && count >= 0);
                        
                        if (jagArr[i][0] == 0)
                        {
                            int length = 0;
                            foreach (int a in jagArr[i])
                                if (a == 1)
                                    break;
                                else
                                    length++;
                            if (length == jagArr[i].Length)//проверка, не является ли двоичное число равным нулю (кроме числа первой строки)
                            {
                                Console.WriteLine("С такими длинами невозможно построить суффиксный двоичный код");
                                return null;
                            }
                        }
                    }
                }
                return jagArr;
            }
            else
                throw new NullReferenceException();
        }

        //перевернуть строки рваного массива (префиксный код->суффиксный)
        static int[][] ReverseAllStrings(int[][] jagArr)
        {
            if (jagArr != null)
            {
                for (int i = 0; i < jagArr.GetLength(0); i++)
                {
                    if (jagArr[i] != null)
                        Array.Reverse(jagArr[i]);
                }
                return jagArr;
            }
            else
                throw new NullReferenceException();
        }

        //печать рваного массива
        static void PrintJaggedArr(int[][] jagArr)
        {
            if (jagArr != null)
            {
                for (int i = 0; i < jagArr.GetLength(0); i++)
                {
                    Console.Write(i + 1 + ") ");
                    for (int j = 0; j < jagArr[i].Length; j++)
                    {
                        Console.Write(jagArr[i][j]);
                    }
                    Console.WriteLine();
                }
            }
            else
                throw new NullReferenceException();
        }

        
        //функция проверки ввода целого числа
        public static int CheckInputInt(string message, int minValue, int maxValue)
        //(сообщение, мин вводимое значение, макс вводимое значение)
        {
            int input; //переменная, которой будет присвоено значение, введенное с клавиатуры
            do
            {
                input = maxValue + 1;  //переменной присваивается значение, выходящее за макс значение
                Console.WriteLine(message); //печать сообщения
                try
                {
                    string buf = Console.ReadLine();
                    input = Convert.ToInt16(buf);
                }
                catch (FormatException)
                {
                }
                catch (OverflowException)
                {
                }
            } while ((input < minValue) || (input > maxValue)); //пока значение больше макс/меньше мин
            return input;
        }

        //Ввод длин слов и запись длин в массив
        static int[] InputInArray(int n,int minValue,int maxValue)
        {
            int[] lenghts = new int[n];
            for (int i = 0; i < n; i++)
                lenghts[i]= CheckInputInt($"Введите длину кодового слова {i + 1} (от {minValue} до {maxValue})", minValue, maxValue);
            return lenghts;
        }

        static void Main(string[] args)
        {
            //программа строит суффиксный код с заданными длинами слов

            int n;//количество кодовых слов
            int[] lengths;// массив длин кодовых слов
            int[][] codeWords;//рваный массив, строки которого представляют кодовые слова

            n = CheckInputInt("Введите количество кодовых слов (от 2 до 100)", 2, 100);//ввод количества кодовых слов
            lengths = InputInArray(n, 1, 100);//ввод длин кодовых слов
            Array.Sort(lengths);//сортировка длин по возрастанию

            codeWords = new int[lengths.Length][];//выделение памяти под массив кодовых слов
            codeWords = MakeJaggedArray(codeWords, lengths);//выделение памяти под массив кодовых слов

            if (KraftMcMillanInequalityForBinaryCode(lengths))//проверка, существует ли однозначнодекодируемый код для заданных длин
                codeWords = MakePreffixCodeWords(codeWords);//вызов функции построения префиксного кода
            else
                Console.WriteLine("Для заданных длин кодовых слов невозможно построить суффиксный код");

            if (codeWords.SelectMany(f=>f).Sum()!=0) //значит префиксный код построен
            {
                codeWords = ReverseAllStrings(codeWords);//перевернуть строки массива, чтобы получить суффиксный код
                Console.WriteLine("Суффиксный код:");
                PrintJaggedArr(codeWords);
            }

        }
    }
}
