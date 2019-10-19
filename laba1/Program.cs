using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Терентьев Владислав ИУ5-33";
            double a, b, c = 0, D;
            string ext;
            int i = 0;
            if (args.Length == 0)
            {
                i = 1;
            }
            Console.WriteLine("Пример уравнения: a*x^4 + b*x^2 + c = 0, где a, b, c - коэффициенты.");
            do
            {
                if (i == 0)
                {
                    if (!double.TryParse(args[0], out a))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Проверьте правильность данных и попробуйте снова.");
                        Console.ResetColor();
                        a = ReadCoefficient("a");
                    }
                    if (args.Length > 1)
                    {
                        if (!double.TryParse(args[1], out b))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Проверьте правильность данных и попробуйте снова.");
                            Console.ResetColor();
                            b = ReadCoefficient("b");
                        }
                    }
                    else
                    {
                        b = ReadCoefficient("b");
                    }
                    if (args.Length > 2)
                    {
                        if (!double.TryParse(args[2], out c))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Проверьте правильность данных и попробуйте снова.");
                            Console.ResetColor();
                            c = ReadCoefficient("c");
                        }
                    }
                    else
                    {
                        c = ReadCoefficient("c");
                    }
                }
                else
                {
                    do
                    {
                        a = ReadCoefficient("a");
                        b = ReadCoefficient("b");
                        if (a == 0 && b == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Вы ввели не уравнение. Попробуйте снова.");
                            Console.ResetColor();
                        }
                    } while (a == 0 && b == 0);
                }
                if (i != 0)
                {
                    c = ReadCoefficient("c");
                }
                if (a == 0 && b == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Вы ввели не уравнение.");
                    Console.ResetColor();
                    ext = "нет";
                }
                else
                {
                    if (i == 0)
                    {
                        Console.WriteLine("a = " + a + " b = " + b + " c = " + c);
                    }
                    if (a == 0)
                    {
                        if ((-c / b) >= 0)
                        {
                            if (c == 0)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Корень уравнения: x = 0");
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.WriteLine("Корни уравнения:");
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("x1 = " + Math.Sqrt(-c / b));
                                Console.WriteLine("x2 = -" + Math.Sqrt(-c / b));
                                Console.ResetColor();
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Данное уравнение не имеет действительных корней.");
                            Console.ResetColor();
                        }
                    }
                    else
                    {
                        if ((D = b * b - 4 * a * c) < 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Данное уравнение не имеет действительных корней.");
                            Console.ResetColor();
                        }
                        else
                        {
                            if (D == 0)
                            {
                                if ((-b / (2 * a)) >= 0)
                                {
                                    if (b == 0)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine("Корень уравнения: x = 0");
                                        Console.ResetColor();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Корни уравнения:");
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine("x1 = " + Math.Sqrt(-b / (2 * a)));
                                        Console.WriteLine("x2 = -" + Math.Sqrt(-b / (2 * a)));
                                        Console.ResetColor();
                                    }
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Данное уравнение не имеет действительных корней.");
                                    Console.ResetColor();
                                }
                            }
                            else
                            {
                                D = Math.Sqrt(D);
                                if (((-b + D) / (2 * a) < 0) && ((-b - D) / (2 * a) < 0))
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Данное уравнение не имеет действительных корней.");
                                    Console.ResetColor();
                                }
                                else
                                {
                                    Console.WriteLine("Корни уравнения:");
                                    if ((-b + D) / (2 * a) >= 0)
                                    {
                                        if ((-b + D) == 0)
                                        {
                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.WriteLine("x = 0");
                                            Console.ResetColor();
                                        }
                                        else
                                        {
                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.WriteLine("x = " + Math.Sqrt((-b + D) / (2 * a)));
                                            Console.WriteLine("x = -" + Math.Sqrt((-b + D) / (2 * a)));
                                            Console.ResetColor();
                                        }
                                    }
                                    if ((-b - D) / (2 * a) >= 0)
                                    {
                                        if ((-b - D) == 0)
                                        {
                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.WriteLine("x = 0");
                                            Console.ResetColor();
                                        }
                                        else
                                        {
                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.WriteLine("x = " + Math.Sqrt((-b - D) / (2 * a)));
                                            Console.WriteLine("x = -" + Math.Sqrt((-b - D) / (2 * a)));
                                            Console.ResetColor();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    do
                    {
                        Console.Write("Выйти из программы ? (");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("да");
                        Console.ResetColor();
                        Console.Write("/");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("нет");
                        Console.ResetColor();
                        Console.Write("): ");
                        if (((ext = Console.ReadLine()) != "да") && (ext != "нет"))
                        {
                            Console.WriteLine("Проверьте правильность данных и попробуйте снова.");
                        }
                    } while (ext != "да" && ext != "нет");
                }
                i = 1;
            } while (ext != "да");
        }

        static double ReadCoefficient(string indx)
        {
            double coeff;
            Console.Write("Введите коэффициент: " + indx + " = ");
            while (!double.TryParse(Console.ReadLine(), out coeff))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Проверьте правильность данных и попробуйте снова.");
                Console.ResetColor();
                Console.Write("Введите коэффициент: " + indx + " = ");
            }
            return coeff;
        }
    }
}