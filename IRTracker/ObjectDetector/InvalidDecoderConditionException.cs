﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRTracker.ObjectDetector
{
    class InvalidDecoderConditionException : Exception
    {
        public InvalidDecoderConditionException(string error) : base(error) { }
    }
}
