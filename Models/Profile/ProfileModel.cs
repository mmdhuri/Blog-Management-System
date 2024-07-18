using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Student_Management_System.Models.Profile
{
    public class ProfileModel
    {
        public string FirstName {get;set;}
        public string LastName {get;set;}
        public string ProfileImage {get;set;}
        public string Emailid {get;set;}
        public string Phonenumber {get;set;}
        public string ProfileCoverImagePath { get;set;}
        public HttpPostedFileBase ProfileImageFile {get;set;}
        public HttpPostedFileBase ProfileCoverImageFile { get;set;}
    }
}