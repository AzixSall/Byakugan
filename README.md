# Byakugan - A Realtime IP Detection Radar System

![Screenshot 2024-09-05 173041](https://github.com/user-attachments/assets/f14383dd-a328-4f90-a3cb-e1d8abeb24a8)

A real-time network monitoring tool that captures and visualizes IP traffic on an interactive 3D map.

![image](https://github.com/user-attachments/assets/47b6b6ef-31cb-4ecc-8ca5-daf59e28cc20)

The frontend is relying on the Bing Maps Service to display the map, you can create your own API key here : https://www.bingmapsportal.com/
![image](https://github.com/user-attachments/assets/bc9cf310-0ded-4ad4-b5d0-00ca0b0d5aa8)

## Features

- **Real-time packet capture** from network interfaces using SharpPcap
- **IP geolocation** with GPS coordinate mapping
- **Interactive 3D map** visualization with location pinpoints
- **Device identification** showing IP addresses and resolved hostnames

## Requirements

- .NET Framework/Core
- SharpPcap library
- Administrative privileges (for packet capture)

## Usage

1. Run the application with administrator privileges
2. Select your network interface
3. Monitor real-time IP traffic on the 3D map
4. Click on map pins to view detailed connection information

## Installation

```bash
git clone [your-repo-url]
cd ip-detection-radar
dotnet restore
dotnet build
update-database
```

## Dependencies

- SharpPcap - Network packet capture
- Bing Maps - 3D map rendering
- IP Table - IP to GPS coordinate conversion

## Legal Notice

This tool is for educational and authorized network monitoring purposes only. Ensure you have proper authorization before monitoring network traffic.

## License

MIT - Feel Free to improve

