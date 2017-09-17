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
            //UpdateNinja();
            //UpdateNinjaDisconnectedModel();
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

        private static void UpdateNinja()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;

                var ninja = context.Ninjas.FirstOrDefault();
                
                //sets inverse of the boolean serverdInObiOne 
                //in the first ninja hit in db:
                ninja.ServedInObiOne = (!ninja.ServedInObiOne);

                context.SaveChanges();

            }
        }

        /// <summary>
        /// This is for disconnected apps like websites,api service.
        /// Take deep look at disconnected model this is just the surface
        /// Take EF enterpise course for patterns to solve data persistance
        /// </summary>
        private static void UpdateNinjaDisconnectedModel()
        {
            //----note:
            //object to be changed
            Ninja ninja;

            //-----note:
            //imaginge the server is retriving a client object and sending 
            //it back to the client

            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;

                ninja = context.Ninjas.FirstOrDefault();
            }

            //-------note:
            //change: imagine the client updates said object and sends it back

            ninja.ServedInObiOne = (!ninja.ServedInObiOne);

            //---------note:
            //updating = false : EF has no way of nowing the state of the object
            //All this does is reinsatiate the context and call save()
            //the resut is only the query gets executed but save changes doesnt:
            //has no clue of the history of ninja,

            using (var context = new NinjaContext())
            {
                //DBcontext Log:

                context.Database.Log = Console.WriteLine;

                //--------note:
                //lets make aware of ninja for argument sake:
                //lets add said ninja object: EF will add without being aware of the 
                //state of ninja.EF is not a human and will do what you tell it:

                //----------------code:-----------commeted out for reason above !!!!!!!!
                //context.Ninjas.Add(ninja);

                //-------note:
                //if we use the attach method (we say hey watch this data
                //But it still has no way of knowing the state of ninja object
                //and how it used to have a different value:
                //this would be usefull to use if the value being changed was underneath 
                //the attach statment:

                context.Ninjas.Attach(ninja);

                //-------note:Entery state
                //So we have to make EF context aware of the state of the object state
                //This is a great way to force entity framework to update all of the object
                //only downside is it goes through all the coloums:

                context.Entry(ninja).State = EntityState.Modified;

                context.SaveChanges();

                //reasources:--read more
                //--https://stackoverflow.com/questions/30987806/dbset-attachentity-vs-dbcontext-entryentity-state-entitystate-modified

            }
        }
    }
}
