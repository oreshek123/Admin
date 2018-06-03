using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.Lib;

namespace Admin
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceProvider service = new ServiceProvider();
            start:
            Console.WriteLine("1 - Создание нового провайдера\n2 - Поиск провайдера в списке существующих");
            int.TryParse(Console.ReadLine(), out int choice);
            switch (choice)
            {
                case 1:
                    {
                        service.AddProvider();
                        Console.WriteLine("Для выхода в меню нажмите ENTER");
                        if (Console.ReadKey().Key == ConsoleKey.Enter)
                        {
                            goto start;
                        }
                        break;
                    }
                case 2:
                    {
                        service.EditProvider();
                        Console.WriteLine("Для выхода в меню нажмите ENTER");
                        if (Console.ReadKey().Key == ConsoleKey.Enter)
                        {
                            goto start;
                        }
                        break;
                    }
            }



            Console.ReadLine();
        }
    }
}
