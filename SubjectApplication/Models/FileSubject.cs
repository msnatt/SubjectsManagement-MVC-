//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SubjectApplication.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class FileSubject
    {
        public int ID { get; set; }
        public int SubjectID { get; set; }
        public string NameFile { get; set; }
        public string PathFile { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual Subject Subject { get; set; }
    }
}
