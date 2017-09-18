using NinJaDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace NinJaDomain
{
    public class Ninja : IModificationHistory
    {
        public Ninja()
        {
            //allways intantiate lists in ctor
            EquipmentOwned = new List<NinjaEquipment>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool ServedInObiOne { get; set; }
        public Clan Clan { get; set; }
        public int ClanId { get; set; }
        ///--------------------->>> lazy loading make it virtual:EF gets prop in instance(removed it tho)
        public List<NinjaEquipment> EquipmentOwned { get; set; }

        //interfaces
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDirty { get; set; }
    }

}
