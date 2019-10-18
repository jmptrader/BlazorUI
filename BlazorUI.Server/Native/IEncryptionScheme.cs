﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorUI.Server.Native
{
    interface IEncryptionScheme
    {
        byte[] ReadData(string name);
        void WriteData(string name, byte[] data);
    }
}
