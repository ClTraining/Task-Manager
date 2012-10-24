﻿using System;
using System.ComponentModel;
using EntitiesLibrary.Converter;

namespace EntitiesLibrary.CommandArguments
{
    [TypeConverter(typeof(ListArgsConverter))]
    public class ListArgs
    {
        public int? Id { get; set; }

        public DateTime Date { get; set; }
    }
}