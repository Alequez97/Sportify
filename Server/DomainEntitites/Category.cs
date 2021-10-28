﻿using System.ComponentModel.DataAnnotations;

namespace DomainEntities
{
    /// <summary>
    /// Class that represents Category model
    /// </summary>
    public class Category : EntityBase
    {
        [Required]
        public string Name { get; set; }
    }
}
