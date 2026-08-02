using System.Buffers.Binary;
using PacketBuilder.Core.Buffers;
using PacketBuilder.Core.Checksums;

namespace PacketBuilder.Core.Protocols;

/// <summary>An IPv4 header (20 bytes, no options) wrapping a payload.</summary>
public sealed class Ipv4Layer : ILayer
{
    public required Ipv4Address Source { get; init; }
    public required Ipv4Address Destination { get; init; }
    public required IpProtocol Protocol { get; init; }
    public byte Ttl { get; init; } = 64;
    public ushort Identification { get; init; }
    public ILayer? Payload { get; init; }

    public void Write(ref PacketWriter writer)
    {
        int start = writer.Length;

        writer.WriteUInt8(0x45);              // version 4, header length 5 words (20 bytes)
        writer.WriteUInt8(0x00);              // DSCP / ECN
        Span<byte> totalLength = writer.Reserve(2);   // filled after payload
        writer.WriteUInt16(Identification);
        writer.WriteUInt16(0x4000);           // flags: Don't Fragment
        writer.WriteUInt8(Ttl);
        writer.WriteUInt8((byte)Protocol);
        Span<byte> checksum = writer.Reserve(2);      // stays 0 while we compute it
        Source.WriteTo(writer.Reserve(4));
        Destination.WriteTo(writer.Reserve(4));

        Payload?.Write(ref writer);

        // Back-patch the two fields that depend on what came after them.
        BinaryPrimitives.WriteUInt16BigEndian(totalLength, (ushort)(writer.Length - start));

        var sum = new InternetChecksum();
        sum.Add(writer.Written.Slice(start, 20));     // checksum over the 20-byte header
        BinaryPrimitives.WriteUInt16BigEndian(checksum, sum.Fold());
    }
}