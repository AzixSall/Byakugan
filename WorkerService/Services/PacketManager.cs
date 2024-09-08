using Shared.Models;
using WorkerService.Repositories;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace WorkerService.Services
{
    public interface IPacketManager
    {
        //Task AddEditPacket(NetworkPacket packet);
        Task DeleteAllPackets();
        Task<IpDetails?> GetConnectionDetailsAsync(string ip);
        Task<List<IpDetails>?> GetConnectionDetailsBatchAsync(HashSet<string> list);
        //HashSet<String> ListOfIpsToTrack();
    }
    public class PacketManager : IPacketManager
    {

        private readonly IPacketRepository _packetRepo;

        public PacketManager(IPacketRepository packetRepo)
        {
            _packetRepo = packetRepo;
        }

        //public async Task AddEditPacket(NetworkPacket packet)
        //{
        //    bool isPacketAlreadyExistant = _packetRepo.IsPacketAlreadyRegistered(packet.DestinationIp);

        //    if (isPacketAlreadyExistant)
        //    {
        //        _packetRepo.UpdatePacket(packet.DestinationIp);
        //    }
        //    else
        //    {
        //        packet.Location = await GetIpLatAndLongAsync(packet.DestinationIp);
        //        await _packetRepo.SavePacketAsync(packet);
        //    }
        //}

        public async Task DeleteAllPackets()
        {
            await _packetRepo.DeleteAllPacketsAsync();
        }

        public async Task<IpDetails?> GetConnectionDetailsAsync(string ip)
        {
            if(!string.IsNullOrEmpty(ip) && ip != null)
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("http://ip-api.com/json/");
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var responseMessage = await httpClient
                            .GetAsync($"{ip}");

                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = await responseMessage.Content.ReadAsStringAsync();

                        var details = JsonConvert.DeserializeObject<IpDetails>(responseData);
                        if (details != null)
                        {
                            return details;
                        }
                        else
                        {
                            return null;
                        }

                    }
                    else
                    {
                        Console.WriteLine($"Failed to retrieve data. Status code: {responseMessage.StatusCode}");
                        return null;
                    }
                }
            }

            return null;
        }

        public async Task<List<IpDetails>?> GetConnectionDetailsBatchAsync(HashSet<string> list)
        {
            if(list != null && list.Any())
            {
                string jsonArray = JsonConvert.SerializeObject(list);

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var content = new StringContent(jsonArray, System.Text.Encoding.UTF8, "application/json");

                    var responseMessage = await httpClient.PostAsync("http://ip-api.com/batch", content);

                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = await responseMessage.Content.ReadAsStringAsync();

                        var details = JsonConvert.DeserializeObject<IEnumerable<IpDetails>>(responseData);
                        if (details != null)
                        {
                            _packetRepo.SavePacketBatch(details.ToList());
                            return null;
                        }
                        else
                        {
                            return null;
                        }

                    }
                    else
                    {
                        Console.WriteLine($"Failed to retrieve data. Status code: {responseMessage.StatusCode}");
                        return null;
                    }
                }
            }
            else
            {
                return null;
            }
        }

        //public HashSet<string> ListOfIpsToTrack()
        //{
            
        //}
    }
}
