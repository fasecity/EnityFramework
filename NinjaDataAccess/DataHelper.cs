using NinJaDomain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaDataAccess
{
  public class DataHelper
    {
        public static void NewDbWithSeed()
        {
            //makes new database data
            Database.SetInitializer(new DropCreateDatabaseAlways<NinjaContext>());
            using (var context = new NinjaContext())
            {
                //determines if sequence has elements eg ninjas
                if (context.Ninjas.Any())
                {
                    return;
                }
                var vtClan = context.Clans.Add(new Clan { ClanName = "vtBoys" });
                var tdotClan = context.Clans.Add(new Clan { ClanName = "TdotClan" });
                var scarbzClan = context.Clans.Add(new Clan { ClanName = "scarbzClan" });


            }
        }
    }
}
