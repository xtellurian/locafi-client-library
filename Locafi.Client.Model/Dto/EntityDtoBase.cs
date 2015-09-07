﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto
{
    public abstract class EntityDtoBase
    {
        protected EntityDtoBase()
        {
            
        }

        protected EntityDtoBase(EntityDtoBase entityDtoBase)
        {
            this.Id = entityDtoBase.Id;
            this.CreatedByUserId = entityDtoBase.CreatedByUserId;
            this.CreatedByUserFullName = entityDtoBase.CreatedByUserFullName;
            this.DateCreated = entityDtoBase.DateCreated;
            this.LastModifiedByUserId = entityDtoBase.LastModifiedByUserId;
            this.DateLastModified = entityDtoBase.DateLastModified;
            this.LastModifiedByUserFullName = entityDtoBase.LastModifiedByUserFullName;
        }

        public Guid Id { get; set; }

        public Guid? CreatedByUserId { get; set; }

        public string CreatedByUserFullName { get; set; }

        public DateTimeOffset DateCreated { get; set; }

        public Guid? LastModifiedByUserId { get; set; }

        public string LastModifiedByUserFullName { get; set; }

        public DateTimeOffset? DateLastModified { get; set; }

        public override bool Equals(object obj)
        {
            var entity = obj as EntityDtoBase;
            return entity != null && entity.Id == this.Id;
        }

        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return this.Id.GetHashCode();
        }
    }
}
