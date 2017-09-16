using NinjaDataAccess;
using NinJaDomain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //this stops ef from going through its database initialization process
            //use for production, to not lose data, for development its okay
            Database.SetInitializer(new NullDatabaseInitializer<NinjaContext>());
           // InsertNinja();
           

        }

        private static void InsertNinja()
        {
            //ninja has a dependeny on the clan and wont work unless a clan is created the clan Id = 1
            var ninja1 = new Ninja
            {
                Id = 1,
                Name = "Mosh",
                ServedInObiOne = false,
                ClanId = 1
            };
            //use using so you dont fuck around with the reasource
            using (var context = new NinjaContext())
            {
                //Log the data from ef
                context.Database.Log = Console.WriteLine;

                //add and save methods for dbset:NinjaContext
                context.Ninjas.Add(ninja1);
                context.SaveChanges();
            }
        }
    }
}
