﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngameScript
{
    public interface IEnvironment
    {
        string Get(string name, char separator = ';');
    }
}
