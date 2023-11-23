using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SubjectApplication.Models
{
    public class TopicMetadata
    {
    }
    [MetadataType(typeof(TopicMetadata))]
    public partial class Topic
    {
        public bool IsHere { get; set; }
    }
}