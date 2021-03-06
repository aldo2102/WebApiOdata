//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApi2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Perfil
    {
        public Perfil()
        {
            this.GrupoUsuario = new HashSet<GrupoUsuario>();
            this.PerfilUsuario = new HashSet<PerfilUsuario>();
            this.Grupo = new HashSet<Grupo>();
        }

        [Key]
        public long IDN_PERFIL { get; set; }
        public long IDN_SISTEMA { get; set; }
        public string DSC_NOME { get; set; }
        public string DSC_DESCRICAO { get; set; }
        public Nullable<System.DateTime> Dta_Inclusao { get; set; }
    
        public virtual ICollection<GrupoUsuario> GrupoUsuario { get; set; }
        public virtual ICollection<PerfilUsuario> PerfilUsuario { get; set; }
        public virtual ICollection<Grupo> Grupo { get; set; }
    }
}
