using NinJaDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinJaDomain
{
   
   
    public class NinjaEquipment : IModificationHistory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //-------------------->>>this required tag makes Ninja a FK property
        //-------------------->>>It somehow infered this as a FK Ninja_ID in the DB
        [Required]
        public Ninja Ninja { get; set; }
        public string EquipmentType { get; set; }

        //interfaces
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDirty { get; set; }
    }
}
