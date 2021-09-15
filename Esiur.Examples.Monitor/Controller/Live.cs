using Esiur.Examples.Monitor.Model;
using Esiur.Core;
using Esiur.Net.IIP;
using Esiur.Resource;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Esiur.Examples.Monitor.Controller
{
    [Resource]
    public partial class Live
    {
        PerformanceCounter processorCounter = new("Processor", "% Processor Time", "_Total");
        PerformanceCounter memoryCounter = new("Memory", "Available MBytes");


        [Public] double processor;
        [Public] double memory;

        [Public] double storage;

        [Public] public event ResourceEventHandler<ProcessInfo> ProcessStarted;
        [Public] public event ResourceEventHandler<ProcessInfo> ProcessEnded;

        Timer timer = new(5000);

        ProcessInfo[] processes;


        [Public] public ProcessInfo GetProcessById(int id) => processes.FirstOrDefault(x => x.Id == id);
        

        [Public] public ProcessInfo[] GetProcesses() => Process.GetProcesses()
                .Select(x =>
                {
                    try
                    {
                        return new ProcessInfo()
                        {
                            Id = x.Id,
                            Name = x.ProcessName,
                            Memory = x.PeakWorkingSet64,
                            Path = x.MainModule?.ModuleName,
                            StartTime = x.StartTime,
                            Title = x.MainWindowTitle
                        };
                    }
                    catch { return null; }
                })
                .Where(x => x != null)
             .ToArray();

        [Public] public ProcessInfo[] GetUIProcesses() => GetProcesses()
                                                            .Where(x => !string.IsNullOrEmpty(x.Title))
                                                            .ToArray();

        [Public] public void Kill(int pid) => throw new NotImplementedException();

        public Live()
        {

            var p = Process.GetProcesses();

            processes = GetProcesses();


            timer.Elapsed += (sender, e) =>
            {
                // properties updated in server will automatically get updated in the client
                Processor = Math.Round(processorCounter.NextValue() * 100) / 100;
                Memory = Math.Round(memoryCounter.NextValue()) ;

                // check processs for new ones
                var ps = GetProcesses();
                var newProcesses = ps.Where(x => !processes.Any(y=>y.Id == x.Id));

                foreach (var p in newProcesses)
                    ProcessStarted?.Invoke(p);

                var closedProcesses = processes.Where(x => !ps.Any(y => y.Id == x.Id));

                foreach (var p in closedProcesses)
                    ProcessEnded?.Invoke(p);

                processes = ps;
            };

            timer.Start();

        }
    }
}
