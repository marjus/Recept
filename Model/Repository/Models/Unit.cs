﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Recept.Models
{
    public class Unit
    {
        [Key]   
        public string Name { get; set; }
    }
}