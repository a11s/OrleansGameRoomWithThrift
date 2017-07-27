using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerInterface;

namespace AppGateway
{
    public class G
    {
        public static IGateway DefaultGateway { get; internal set; }
        public static ClientProcessor DefaultClientProcessor { get; internal set; }
    }
}
