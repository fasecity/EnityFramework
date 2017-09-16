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
           // QIterateCollection();
        }

        private static void InsertNinja()
        {
            //ninja has a dependeny on the clan and 
            //wont work unless a clan is created the clan Id = 1:
            var ninja1 = new Ninja
            {                
                Name = "rolo",
                ServedInObiOne = false,
                ClanId = 1
            };
            var ninja2 = new Ninja
            {
                Name = "jumand",
                ServedInObiOne = true,
                ClanId = 1
            };

            //use using so you dont fuck around with the reasource:
            using (var context = new NinjaContext())
            {
                //Log the data from ef:
                context.Database.Log = Console.WriteLine;

                //Ninjacontext only adds one object at a time 
                //unless using addRange method 
                //takes in Ienumrable collection type:
                context.Ninjas.AddRange(new List<Ninja>{ninja1,ninja2 });
                
                //add and save methods for dbset:NinjaContext:
                //context.Ninjas.Add(ninja1); //----> regular single object add


                context.SaveChanges();
            }
        }

        private static void QIterateCollection()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;

                var ninjas = context.Ninjas.ToList();
                var query = context.Ninjas.Where(n => n.Name == "mosh").ToList();

                //first or default only returns one, not a collection- 3ms faster than foreach
                //retuns null if empty,Only brings back first found value on list,even if there is more
                //that match:
                //use a varible ef/linq always paramiterizes varibales by default(better 4 sqli)
                string name = "mosh";
                var query2 = context.Ninjas.Where(n => n.Name == name).FirstOrDefault();
                Console.WriteLine(query2.Name);


                //express querys using linq methods :----!!!!!!!!
                // foreach loop leaves connection to db open
                //(untill collection is iterated  through) if there is a large especially
                //foreach (var item in query)
                //{
                //    Console.WriteLine(item.Name);
                //}
            }
        }
    }
}
