using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SubjectApplication.Models
{
    public class SubjectMetadata
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> StudyTimeStart { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> StudyTimeEnd { get; set; }

    }
    [MetadataType(typeof(SubjectMetadata))]
    public partial class Subject
    {
        public string StartAndEndSubjectDate
        {
            get
            {
                var startdt = string.Format("{0:dd/MM/yyyy}", StudyTimeStart);
                var enddt = string.Format("{0:dd/MM/yyyy}", StudyTimeEnd);
                var StartAndEndDate = startdt + " - " + enddt;
                return StartAndEndDate;
            }

        }
        public List<HttpPostedFileBase> ListFile { get; set; }
        
    }
}