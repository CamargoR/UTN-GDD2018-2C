﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalcoNet.Classes.CustomException
{
    class AutoMappingException : Exception
    {
        public AutoMappingException(string message) : base(message) { }
    }
}
