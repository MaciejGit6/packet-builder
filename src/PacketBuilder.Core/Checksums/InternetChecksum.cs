
using System.Buffers.Binary;

namespace PacketBuilder.Core.Checksums;

/// <summary>The 16-bit one's-complement "Internet checksum" (RFC 1071), used by IPv4/ICMP/TCP/UDP.</summary>
public struct InternetChecksum
{
    private uint _sum;

    public void Add(ReadOnlySpan<byte> data)
    {
        int i = 0;
        for (; i + 1 < data.Length; i += 2)
            _sum += BinaryPrimitives.ReadUInt16BigEndian(data.Slice(i, 2));

        if (i < data.Length)
            _sum += (uint)(data[i] << 8);
    }

    public void Add(ushort value) => _sum += value;

    public readonly ushort Fold()
    {
        uint sum = _sum;
        while ((sum >> 16) != 0)
            sum = (sum & 0xFFFF) + (sum >> 16);
        return (ushort)~sum;
    }

    public static InternetChecksum operator +(InternetChecksum a, InternetChecksum b)
        => new() { _sum = a._sum + b._sum };
}