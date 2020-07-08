using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Both
{
    public delegate void EventByte(byte[] array);
    public delegate void EventServer(object sender, byte[] array);
    public delegate void EventSender(object sender);
}
