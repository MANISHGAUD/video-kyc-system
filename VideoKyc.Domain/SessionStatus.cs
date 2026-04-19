using System;
using System.Collections.Generic;
using System.Text;

namespace VideoKyc.Domain
{
    public enum SessionStatus
    {
        Waiting,
        Connecting,
        Connected,
        Ended
    }
}
