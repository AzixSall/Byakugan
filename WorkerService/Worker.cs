using PacketDotNet;
using Shared.Models;
using SharpPcap;
using WorkerService.Services;
using static System.Formats.Asn1.AsnWriter;

namespace WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        public HashSet<String> ipsToTrack = new HashSet<String>();
        private bool stopCapture = false;
        IServiceProvider _serviceProvider;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                await scope.ServiceProvider.GetRequiredService<IPacketManager>().DeleteAllPackets();
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }

                Capture();

                using (var scope = _serviceProvider.CreateScope())
                {
                    if (ipsToTrack != null)
                    {
                        await scope.ServiceProvider.GetRequiredService<IPacketManager>().GetConnectionDetailsBatchAsync(ipsToTrack);
                    }
                    else
                    {
                        _logger.LogWarning("Empty List");
                    }

                }

                await Task.Delay(10000, stoppingToken);
            }
        }

        public void device_OnPacketArrival(object sender, PacketCapture e)
        {
            var time = e.Header.Timeval.Date;
            var len = e.Data.Length;
            var rawPacket = e.GetPacket();

            var packet = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);

            var tcpPacket = packet.Extract<PacketDotNet.TcpPacket>();
            if (tcpPacket != null)
            {
                var ipPacket = (PacketDotNet.IPPacket)tcpPacket.ParentPacket;
                System.Net.IPAddress srcIp = ipPacket.SourceAddress;
                System.Net.IPAddress dstIp = ipPacket.DestinationAddress;
                int srcPort = tcpPacket.SourcePort;
                int dstPort = tcpPacket.DestinationPort;


                Console.WriteLine("{0}:{1}:{2},{3} Len={4} {5}:{6} -> {7}:{8}",
                    time.Hour, time.Minute, time.Second, time.Millisecond, len,
                    srcIp, srcPort, dstIp, dstPort);

                NetworkPacket newPacket = new NetworkPacket
                {
                    LastUpdate = DateTime.Now,
                    DestinationIp = dstIp.ToString(),
                    DestinationPort = dstPort,
                    SourceIp = srcIp.ToString(),
                    SourcePort = srcPort,
                    Location = string.Empty
                };


                ipsToTrack.Add(dstIp.ToString());
                ipsToTrack.Add(srcIp.ToString());
            }
        }


        public void Capture()
        {
            Console.WriteLine("Byakugan - An IP Tracking Radar System");

            // Retrieve the device list
            var devices = CaptureDeviceList.Instance;

            // If no devices were found print an error
            if (devices.Count < 1)
            {
                Console.WriteLine("No devices were found on this machine");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("The following devices are available on this machine:");
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine();

            int i = 0;

            // Print out the devices
            foreach (var dev in devices)
            {
                Console.WriteLine("{0}) {1} {2}", i, dev.Name, dev.Description);
                i++;
            }

            Console.WriteLine();
            Console.Write("-- Please choose a device to capture: ");
            i = int.Parse(Console.ReadLine());

            using var device = devices[i];

            // Open the device for capturing
            int readTimeoutMilliseconds = 1000;
            device.Open(DeviceModes.Promiscuous, readTimeoutMilliseconds);

            Console.WriteLine();
            Console.WriteLine("-- Listening on {0}...",
                device.Description);

            RawCapture packet;

            // Capture packets using GetNextPacket()
            PacketCapture e;
            GetPacketStatus retval;
            while ((retval = device.GetNextPacket(out e)) == GetPacketStatus.PacketRead)
            {
                packet = e.GetPacket();

                var processedPacket = PacketDotNet.Packet.ParsePacket(packet.LinkLayerType, packet.Data);
                var tcpPacket = processedPacket.Extract<PacketDotNet.TcpPacket>();

                // Prints the time and length of each received packet
                var time = packet.Timeval.Date;
                var len = packet.Data.Length;

                if (tcpPacket != null)
                {
                    var ipPacket = (PacketDotNet.IPPacket)tcpPacket.ParentPacket;
                    System.Net.IPAddress srcIp = ipPacket.SourceAddress;
                    System.Net.IPAddress dstIp = ipPacket.DestinationAddress;
                    int srcPort = tcpPacket.SourcePort;
                    int dstPort = tcpPacket.DestinationPort;


                    Console.WriteLine("{0}:{1}:{2},{3} Len={4} {5}:{6} -> {7}:{8}",
                        time.Hour, time.Minute, time.Second, time.Millisecond, len,
                        srcIp, srcPort, dstIp, dstPort);

                    NetworkPacket newPacket = new NetworkPacket
                    {
                        LastUpdate = DateTime.Now,
                        DestinationIp = dstIp.ToString(),
                        DestinationPort = dstPort,
                        SourceIp = srcIp.ToString(),
                        SourcePort = srcPort,
                        Location = string.Empty
                    };


                    ipsToTrack.Add(dstIp.ToString());
                    ipsToTrack.Add(srcIp.ToString());
                }
            }

            // Print out the device statistics
            Console.WriteLine(device.Statistics.ToString());

            Console.WriteLine("-- Timeout elapsed, capture stopped, device closed.");
        }
    }
}
