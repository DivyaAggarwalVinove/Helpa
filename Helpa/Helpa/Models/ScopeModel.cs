﻿using SQLite;

namespace Helpa.Models
{
    public class ScopeModel
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string ScopeName { get; set; }
        public bool isSelected { get; set; }
    }
}