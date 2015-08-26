﻿using System;

namespace Locafi.Client.Data
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
        public string OrganisationName { get; set; }
        public string ImageUrl { get; set; }
        public string UserName { get; set; }

        public DateTime DateCreated { set; get; } // date item was created
        public string CreatedByUserId { set; get; } // id of the user who created it
        public DateTime DateLastModified { set; get; }  // datetime fo creation
        public string LastModifiedByUserId { set; get; }    // id of last user who modified it

        public bool? IsDeleted { get; set; }    // if item is deleted
        public bool? IsActive { get; set; }     // if item is active

        public override bool Equals(object obj)
        {
            var user = obj as UserDto;
            return user != null && user.Id.Equals(Id);
        }
    }
}
