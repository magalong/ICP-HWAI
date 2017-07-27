using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RainVisitor.Models;

namespace RainVisitor
{
    class Program
    {
        static void Main(string[] args)
        {
            DataTable data = RainVisitor.Repository.LoginRepository.Check();
            Console.WriteLine(data.ToString());
            Console.ReadKey();
        }
    }
}
