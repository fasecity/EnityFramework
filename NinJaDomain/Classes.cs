using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinJaDomain
{
    public class Ninja
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
        ///--------------------->>> lazy loading make it virtual:EF gets prop in instance
        public virtual List<NinjaEquipment> EquipmentOwned { get; set; }
    }
    public class Clan
    {
        public Clan()
        {
            Ninjas = new List<Ninja>();
        }
        public int Id { get; set; }
        public string ClanName { get; set; }
        public List<Ninja> Ninjas { get; set; }

    }
    public class NinjaEquipment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //-------------------->>>this required tag makes Ninja a FK property
        //-------------------->>>It somehow infered this as a FK Ninja_ID in the DB
        [Required]
        public Ninja Ninja { get; set; }
        public string EquipmentType { get; set; }
    }
}
