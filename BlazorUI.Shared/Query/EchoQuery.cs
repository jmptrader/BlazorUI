﻿using System;
using System.Collections.Generic;
using System.Text;
using Totem.Timeline;

namespace BlazorUI.Shared.Query
{
    public class EchoQuery : Totem.Timeline.Query
    {
        public int Count;

        public DateTime? TimeOfLastEcho;

        public TimeSpan? DelaySinceLastEcho;

        void Given(Echoed e)
        {
            Count++;
            DelaySinceLastEcho = DateTime.Now - (TimeOfLastEcho ?? DateTime.Now);
            TimeOfLastEcho = DateTime.Now;
        }
    }
}
