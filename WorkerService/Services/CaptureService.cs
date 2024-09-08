using Shared.Models;
using SharpPcap;

namespace WorkerService.Services
{
    //public interface ICaptureService
    //{
    //    CaptureDeviceList GetDeviceToCapture();
    //    void CaptureDevice(string deviceName);
    //}

    public class CaptureService //: ICaptureService
    {

        public readonly IPacketManager _packetManager;

        public CaptureService(IPacketManager packetManager)
        {
            _packetManager = packetManager;
        }

        public static CaptureDeviceList GetDeviceToCapture()
        {
            var devices = CaptureDeviceList.Instance;
            return devices;
        }

        public static void CaptureDevice(string deviceName)
        {
            if(!string.IsNullOrEmpty(deviceName) && deviceName != null)
            {
                var devices = GetDeviceToCapture();
                //var deviceToCapture = devices.ToList().Where(d => d.Name.ToLower() == deviceName).FirstOrDefault();
                var deviceToCapture = devices[5];

                deviceToCapture.OnPacketArrival +=
                new PacketArrivalEventHandler(device_OnPacketArrival);

                // Open the device for capturing
                int readTimeoutMilliseconds = 1000;
                deviceToCapture.Open(DeviceModes.Promiscuous, readTimeoutMilliseconds);

                //tcpdump filter to capture only TCP/IP packets
                string filter = "ip and tcp";
                deviceToCapture.Filter = filter;

                // Start capture 'INFINTE' number of packets
                deviceToCapture.Capture();

            }
        }

        public static void device_OnPacketArrival(object sender, PacketCapture e)
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

                NetworkPacket newPacket = new NetworkPacket
                {
                    LastUpdate = DateTime.Now,
                    DestinationIp = dstIp.ToString(),
                    DestinationPort = dstPort,
                    SourceIp = srcIp.ToString(),
                    SourcePort = srcPort,
                    Location = string.Empty
                };

                //_packetManager

                Console.WriteLine("{0}:{1}:{2},{3} Len={4} {5}:{6} -> {7}:{8}",
                    time.Hour, time.Minute, time.Second, time.Millisecond, len,
                    srcIp, srcPort, dstIp, dstPort);
            }
        }
    }

}
