//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVC_Pekara
{
    using System;
    using System.Collections.Generic;
    
    public partial class Artikli
    {
        public int ArtikalID { get; set; }
        public int KategorijaID { get; set; }
        public string NazivArtikla { get; set; }
        public Nullable<int> Masa { get; set; }
        public string TipBrasna { get; set; }
        public Nullable<int> ProizvodjackaCenaBezPDV { get; set; }
        public Nullable<int> ProizvodjackaCenaSaPDV { get; set; }
    }
}