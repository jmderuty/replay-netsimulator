using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace replay_netsimul
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.AddCommandLine(args);
            var config = builder.Build().Get<AppConfig>();

            using (var reader = File.OpenText(config.InputPath))
            {

                var lastTimeSampled = 0;
                var rand = new Random((int)config.Seed);

                foreach (var entry in GetEntries(reader))
                {
                    if (entry.Time - lastTimeSampled > config.Sampling)
                    {
                        lastTimeSampled = entry.Time;
                        if (1 - rand.NextDouble() > config.PacketLoss)
                        {
                            var newTime = entry.Time + config.MinLatency + rand.Next(config.MaxLatency - config.MinLatency + 1);

                            Console.Write(newTime + "\t");
                            Console.WriteLine(entry.Line);
                        }
                    }
                }
            }

        }

        private static IEnumerable<ReplayEntry> GetEntries(StreamReader reader)
        {
            reader.ReadLine();//Skip first line
            var line = reader.ReadLine();

            while (line != null)
            {
                yield return ReplayEntry.Parse(line);
                line = reader.ReadLine();
            }
        }

    }

    public class ReplayEntry
    {
        public static ReplayEntry Parse(string line)
        {
            var timeSeparator = line.IndexOf('\t');
            var entry = new ReplayEntry
            {
                Time = int.Parse(line.Substring(0, timeSeparator)),
                Line = line
            };
            return entry;
        }
        /// <summary>
        /// Time in ms since the start of the game
        /// </summary>
        public int Time { get; set; }

        /// <summary>
        /// Line corresponding to the entry
        /// </summary>
        public string Line { get; set; }
    }

    public class AppConfig
    {
        /// <summary>
        /// Path to the input replay file
        /// </summary>
        public string InputPath { get; set; }

        /// <summary>
        /// Mini
        /// </summary>
        public int MinLatency { get; set; } = 50;
        /// <summary>
        /// Maximum latency introduced on each packet (ms)
        /// </summary>
        public int MaxLatency { get; set; } = 50;
        /// <summary>
        /// Packet loss (0-1)
        /// </summary>
        public float PacketLoss { get; set; } = 0;
        /// <summary>
        /// Sampling interval in ms done by the network engine from the source data
        /// </summary>
        public int Sampling { get; set; } = 50;

        public long Seed { get; set; } = DateTime.UtcNow.Ticks;
    }
}
