using Shared.Models;
using WorkerService.Data;

namespace WorkerService.Repositories
{
    public interface IPacketRepository
    {
        void SavePacketBatch(List<IpDetails> packet);
        void UpdatePacket(string ip);
        bool IsPacketAlreadyRegistered(string ip);
        Task DeleteAllPacketsAsync();
    }

    public class PacketRepository : IPacketRepository
    {
        private readonly ApplicationDbContext _appDbContext;

        public PacketRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task DeleteAllPacketsAsync()
        {
            var data = _appDbContext.IpDetails.AsEnumerable();
            if (data.Any())
            {
                foreach (var item in data)
                {
                    _appDbContext.IpDetails.Remove(item);
                }

                await _appDbContext.SaveChangesAsync();
            }
        }

        public bool IsPacketAlreadyRegistered(string ip)
        {
            return _appDbContext.NetworkPackets.Any(x => x.DestinationIp == ip);
        }

        public void SavePacketBatch(List<IpDetails> packet)
        {
            _appDbContext.IpDetails.AddRange(packet);
            _appDbContext.SaveChanges();
        }

        public void UpdatePacket(string ip)
        {
            NetworkPacket? packet = _appDbContext.NetworkPackets.Where(p => p.DestinationIp == ip).FirstOrDefault();

            if (packet != null)
            {
                packet.LastUpdate = DateTime.Now;
            }
        }
    }
}
