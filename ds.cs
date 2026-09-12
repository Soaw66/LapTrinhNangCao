using System;

class Program
{
    static void Main()
    {
        Console.Write("Nhap a: ");
        double a = double.Parse(Console.ReadLine()!);

        Console.Write("Nhap b: ");
        double b = double.Parse(Console.ReadLine()!);

        Console.Write("Nhap c: ");
        double c = double.Parse(Console.ReadLine()!);

        double sum = a + b + c;

        Console.WriteLine("Tong cua 3 so la: " + sum);
    }
}