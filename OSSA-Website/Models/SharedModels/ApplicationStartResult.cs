﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OSSA_Website.Models.SharedModels
{
    /// <summary>
    ///  Microservice start result class
    /// </summary>
    public class ApplicationStartResult
    {
        public bool Success { get; set; }
        public Exception Exception { get; set; }
    }
}
