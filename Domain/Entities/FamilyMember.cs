using System;
using System.Collections.Generic;
using Domain.Enums;

namespace Domain.Entities
{


    public partial class FamilyMember
    {
        public int Id { get; set; }

        public int MafiaFamilyId { get; set; }

        public string FirstName { get; set; } = null!;

        public string SecondName { get; set; } = null!;

        public int Age { get; set; }

        public int RankId { get; set; }

        public int Money { get; set; }

        public virtual Inventory? Inventory { get; set; }

        public virtual MafiaFamily MafiaFamily { get; set; } = null!;

        public virtual RankType Rank { get; set; } = null!;
    }
}
