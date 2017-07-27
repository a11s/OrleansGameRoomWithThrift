using Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGateway
{
    public class EventBuffer
    {
        public string Name;


        byte CurrentEventsBuffIdx = 0;
        public List<game_event> CurrentEventsBuff
        {
            get
            {
                if (EventsBuffList == null) { return null; }
                var c = EventsBuffList[CurrentEventsBuffIdx % EventsBuffList.Count];
                return c;

            }
        }
        public void SwapEventBuffList()
        {
            var c = CurrentEventsBuff;
            unchecked { CurrentEventsBuffIdx++; }
            c.Clear();

        }

        List<List<game_event>> EventsBuffList = null;

        public EventBuffer(string name)
        {
            this.Name = name;
            EventsBuffList = new List<List<game_event>>();
            for (int i = 0; i < 2; i++)
            {
                EventsBuffList.Add(new List<game_event>());
            }
        }
    }
}
