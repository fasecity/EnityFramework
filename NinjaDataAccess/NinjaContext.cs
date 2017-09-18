using NinJaDomain;
using NinJaDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaDataAccess
{
    public class NinjaContext:DbContext
    {
        public DbSet<Ninja> Ninjas { get; set; }
        public DbSet<Clan> Clans { get; set; }
        public DbSet<NinjaEquipment> Equipment { get; set; }

        //using fluent api to overide property
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Types().
                Configure(c => c.Ignore("IsDirty"));
            base.OnModelCreating(modelBuilder);
        }
        /// <summary>
        /// forces ef to persist date/time data accordingly so I dont
        /// have to do it all the time. overided on builder.
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            foreach (var History in this.ChangeTracker.Entries()
                .Where(e => e.Entity is 
                IModificationHistory &&(e.State == EntityState.Added || e.State == EntityState.Modified)).Select
                (e=> e.Entity as IModificationHistory))
            {
                History.DateModified = DateTime.Now;
                if (History.DateCreated == DateTime.MinValue)
                {
                    History.DateCreated = DateTime.Now;
                }

            }
            //intenal result call reps unit of work
            int result = base.SaveChanges();
            // gets
            foreach (var History in this.ChangeTracker.Entries()
                                        .Where(e => e.Entity is IModificationHistory)
                                        .Select(e => e.Entity as IModificationHistory))
            {
                //is set for connected apps like wpf/console 
                History.IsDirty = false;
            }
           return result;
        }


    }
}
