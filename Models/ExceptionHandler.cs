//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Student_Management_System.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ExceptionHandler
    {
        public int ExceptionID { get; set; }
        public string ReffrenceID { get; set; }
        public string Controller { get; set; }
        public string Method { get; set; }
        public string StackStrace { get; set; }
        public string Message { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
    }
}