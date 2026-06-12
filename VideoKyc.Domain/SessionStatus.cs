using System;
using System.Collections.Generic;
using System.Text;

namespace VideoKyc.Domain
{
    public enum SessionStatus
    {
        Waiting = 1,

        Assigned = 2,

        Connecting = 3,

        Live = 4,

        Recording = 5,

        DocumentVerification = 6,

        FaceVerification = 7,

        Review = 8,

        Approved = 9,

        Rejected = 10,

        Completed = 11,

        Cancelled = 12
    }
}
