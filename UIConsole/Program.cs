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
            //FindNinjas();
            //RetriveWithStoredProcedure();
            // DeleteNinja();
            // InsertGraphNinjaWithEqiup();
            // GraphQueryRetrive();
          //  ProjectedQuery();
        }

        private static void InsertNinja()
        {
            //ninja has a dependeny on the clan and 
            //wont work unless a clan is created the clan Id = 1:
            var ninja1 = new Ninja
            {                
                Name = "joiud",
                ServedInObiOne = false,
                ClanId = 1
            };
            var ninja2 = new Ninja
            {
                Name = "joe",
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
                context.Ninjas.AddRange(new List<Ninja>{ninja1,ninja2 }); //-------->adds more than 1 object
                
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
        
        private static void FindNinjas()
        {
            using (var context = new NinjaContext())
            {

                context.Database.Log = Console.WriteLine;

                //keyval reps the PKey value
                var keyVal = 4;


                //find method searched for the element with said value
                //uses single or default and serches top 2 
                var ninja = context.Ninjas.Find(keyVal);

                Console.WriteLine("afterFind-1: {0}",ninja.Name);

                //------------------------------------------
                //for some cool reason the find method doest search the database twice
                //if the keyval has been already searched b4,
                //it doesnt make a second trip to db even if b
                var someninja = context.Ninjas.Find(keyVal);

                Console.WriteLine("afterFind-2: {0}", someninja.Name);






                //even tho they are diffrent local variables they refrence the same
                //index in the object []array in the find(param obj[] keyVal)
                //DBset is just an Ienumrable of Objects :
               
                //Console.WriteLine(ninja.Equals(someninja));
                //Console.WriteLine(ReferenceEquals(ninja,someninja));
                ///Console.WriteLine(ninja == someninja);



            }
        }
        /// <summary>
        /// Not working need to study stored procedures
        /// simple tho make stored proc and iterate
        /// </summary>
        private static void RetriveWithStoredProcedure()
        {
            using (var context = new NinjaContext())
            {
                //context.Database.Log = Console.WriteLine;

                
                //var ninjas = context.Ninjas.SqlQuery("exec dbo.Procedure_2");
                //foreach (var item in ninjas)
                //{
                //    Console.WriteLine(item.Name);
                //}
            }
        }

        private static void DeleteNinja()
        {
            Ninja ninja;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;

                 ninja = context.Ninjas.FirstOrDefault();

                //context.Ninjas.Remove(ninja);

                //context.SaveChanges();

            }
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;

                context.Entry(ninja).State = EntityState.Deleted;
                //context.Ninjas.Remove(ninja);

                context.SaveChanges();

            }

        }
        /// <summary>
        /// this method is okay but makes 2 trips to db
        /// </summary>
        private static void DeleteNinjaByID()
        {
            var keyVal = 6;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;

                var delNinja = context.Ninjas.Find(keyVal);

                context.Ninjas.Remove(delNinja);
                context.SaveChanges();

            }
        }
        /// <summary>
        /// Makes only one trip to DB, most efficient
        /// way to delete object
        /// (Learn stored procs asap)
        /// </summary>
        private static void DeleteNinjaStoredProc()
        {
            using (var context = new NinjaContext())
            {
                var keyVal = 3;
                context.Database.Log = Console.WriteLine;

                context.Database.ExecuteSqlCommand("exec DeleteNinjaByID {0}",keyVal);
                context.SaveChanges();

            }
        }

        private static void InsertGraphNinjaWithEqiup()
        {
            var ninja1 = new Ninja
            {
                Name = "Huboy",
                ServedInObiOne = false,
                ClanId = 1
            };
            var eqipment1 = new NinjaEquipment
            {
                Name = "bulldozer",
                EquipmentType ="vehicle"
                
            };
            var eqipment2 = new NinjaEquipment
            {
                Name = "sword",
                EquipmentType = "knife"

            };


            using (var context = new NinjaContext())
            {
                //Log the data from ef:
                context.Database.Log = Console.WriteLine;

                //One path is adding the ninja to the context first
                //then adding the equipment.When this is done EF is
                //smart enough to add the equipment to the context implicitly
                //thus linking it with the ninja1 object-- lil magic in code first convention
                //the untracked(not added to context) obj(eqipment) inherits the state of ninja1.
                //thus equipment keeps it's state when saved in the contxt
                context.Ninjas.Add(ninja1);
                ninja1.EquipmentOwned.Add(eqipment1);
                ninja1.EquipmentOwned.Add(eqipment2);

                context.SaveChanges();
            }
        }

        private static void GraphQueryRetrive()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;

                //No loading:------------------------
                //all this returns is ninja object---no weapons/equiip
                var ninja = context.Ninjas.FirstOrDefault(x => x.Name.StartsWith("Huboy"));


                //Eager loading:---------------------------------------------------------:
                
                //if you want equipment you gotta include it,with eagerloading DbSet 
                //has method called include. Dont abuse eagerloading slows down db

                 ninja = context.Ninjas.Include(x => x.EquipmentOwned)
                  .FirstOrDefault(x => x.Name.StartsWith("Huboy"));


                //--------------------------------------------------------------------------------////
                //explict loading :----------------------------------------------------------------
                
                //if state is note specified eqipmnet dont load
                  ninja = context.Ninjas.FirstOrDefault(x => x.Name.StartsWith("Huboy"));
                 Console.WriteLine("ninja retrived : {0} , Equipment {1}",ninja.Name ,ninja.EquipmentOwned.Count);//count 0

                //eager loads collection explicitly
                context.Entry(ninja).Collection(x => x.EquipmentOwned).Load();//has to be specified without lazy loading
                Console.WriteLine("ninja retrived : {0} , Equipment {1}", ninja.Name, ninja.EquipmentOwned.Count);//2
                //-------------------------------------------------------------------------------------------------------//
               

                //Lazy loading ------------------------------------------------------------------------:
                
                // make equipment prop virtual EF does some magic and gets count
                //magic comes with a performence hit if your loading alot of stuff since EF
                //in a foreach it would multiply the trips to the database by 2 for each row
                //since the equipment coloum is being lazy loaded in advance
                //use sparengly:
                Console.WriteLine("ninja retrived : {0} , Equipment {1}", ninja.Name, ninja.EquipmentOwned.Count);//count 2







            }

        }

        private static void ProjectedQuery()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;

                //select gets scalar data in an annonymus object
                var ninja = context.Ninjas
                    .Select(x => new { x.Name, x.ServedInObiOne, x.EquipmentOwned }).ToList();

            }
        }
    }
}