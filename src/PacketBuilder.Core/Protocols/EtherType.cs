namespace PacketBuilder.Core.Protocols;

/// <summary>EtherType values (Ethernet II header, offset 12).</summary>
public enum EtherType : ushort
{
    Ipv4 = 0x0800,
    Arp = 0x0806,
    Ipv6 = 0x86DD,
}