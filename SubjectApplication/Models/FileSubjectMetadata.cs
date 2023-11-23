using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SubjectApplication.Models
{
    public class FileSubjectMetadata
    {
    }
    [MetadataType(typeof(FileSubjectMetadata))]
    public partial class FileSubject
    {
        public bool IsHidden { get; set; }
        //public HttpPostedFileBase HttpFile { get; set; }
        //public List<HttpPostedFileBase> ListFile { get; set; }
    }
}