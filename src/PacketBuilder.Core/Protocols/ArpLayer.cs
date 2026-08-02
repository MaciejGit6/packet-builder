using PacketBuilder.Core.Buffers;

namespace PacketBuilder.Core.Protocols;

public enum ArpOperation : ushort
{
    Request = 1,
    Reply = 2,
}

/// <summary>An ARP-over-Ethernet message for IPv4 (28 bytes).</summary>
public sealed class ArpLayer : ILayer
{
    public ArpOperation Operation { get; init; } = ArpOperation.Request;
    public required MacAddress SenderMac { get; init; }
    public required Ipv4Address SenderIp { get; init; }
    public MacAddress TargetMac { get; init; }          // left as 0 for a request
    public required Ipv4Address TargetIp { get; init; }

    public void Write(ref PacketWriter writer)
    {
        writer.WriteUInt16(1);                        // hardware type: Ethernet
        writer.WriteUInt16((ushort)EtherType.Ipv4);  //protocol type: IPv4
        writer.WriteUInt8(6);                         //hardware address length
        writer.WriteUInt8(4);                         // protocol address length
        writer.WriteUInt16((ushort)Operation);
        SenderMac.WriteTo(writer.Reserve(6));
        SenderIp.WriteTo(writer.Reserve(4));
        TargetMac.WriteTo(writer.Reserve(6));
        TargetIp.WriteTo(writer.Reserve(4));
    }
}