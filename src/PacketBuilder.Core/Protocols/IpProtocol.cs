namespace PacketBuilder.Core.Protocols;

/// <summary>IANA IP protocol numbers (IPv4 header, offset 9).</summary>
public enum IpProtocol : byte
{
    Icmp = 1,
    Tcp = 6,
    Udp = 17,
}