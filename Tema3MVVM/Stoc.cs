//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Tema3MVVM
{
    using System;
    using System.Collections.Generic;
    
    public partial class Stoc
    {
        public long IDstoc { get; set; }
        public long IDprodus { get; set; }
        public long cantitate { get; set; }
        public string unitate_de_masura { get; set; }
        public System.DateTime data_aprovizionare { get; set; }
        public System.DateTime data_expirare { get; set; }
        public float pret_achizitie { get; set; }
        public float pret_vanzare { get; set; }
        public bool exista { get; set; }
    
        public virtual Produs Produs { get; set; }
    }
}