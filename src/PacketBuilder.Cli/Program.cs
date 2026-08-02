using PacketBuilder.Core.Buffers;
using PacketBuilder.Core.Protocols;

Span<byte> frame = stackalloc byte[1518];
var writer = new PacketWriter(frame);

var eth = new EthernetLayer
{
    Destination = MacAddress.Parse("11:22:33:44:55:66"),
    Source = MacAddress.Parse("de:ad:be:ef:00:01"),
    EtherType = EtherType.Ipv4,
    Payload = new Ipv4Layer
    {
        Source = Ipv4Address.Parse("192.168.0.1"),
        Destination = Ipv4Address.Parse("192.168.0.199"),
        Protocol = IpProtocol.Udp,
        Payload = new UdpLayer
        {
            SourcePort = 40000,
            DestinationPort = 5683,                        // CoAP
            Payload = new RawPayload(new byte[] { 0x40, 0x01, 0x00, 0x00 }),
        },
    },
};
eth.Write(ref writer);

Console.WriteLine(Convert.ToHexString(writer.Written));