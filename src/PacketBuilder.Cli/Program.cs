using PacketBuilder.Core.Buffers;
using PacketBuilder.Core.Protocols;

Span<byte> frame = stackalloc byte[1518];
var writer = new PacketWriter(frame);

var eth = new EthernetLayer
{
    Destination = MacAddress.Broadcast,
    Source = MacAddress.Parse("de:ad:be:ef:00:01"),
    EtherType = EtherType.Arp,
};
eth.Write(ref writer);

Console.WriteLine(Convert.ToHexString(writer.Written));
// prints: FFFFFFFFFFFFDEADBEEF00010806