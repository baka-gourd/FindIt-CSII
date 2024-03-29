﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindIt.Domain
{
	public class CategoryIconAttribute : Attribute
	{
        public string Icon { get; set; }

        public CategoryIconAttribute(string icon)
        {
            Icon = icon;
        }
    }
}