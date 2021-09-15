using Esiur.Data;
using Esiur.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esiur.Examples.Monitor.Model
{

    public class ProcessInfo : IRecord
    {
        [Public] public int Id { get; set; }
        [Public] public string Name { get; set; }
        [Public] public string Path { get; set; }
        [Public] public long Memory { get; set; }
        [Public] public DateTime StartTime { get; set; }
        [Public] public string Title { get; set; }
    }
}
