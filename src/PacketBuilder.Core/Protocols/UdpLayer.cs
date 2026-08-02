using System.Buffers.Binary;
using PacketBuilder.Core.Buffers;

namespace PacketBuilder.Core.Protocols;

/// <summary>A UDP header (8 bytes) wrapping a payload. Checksum left 0 (allowed on IPv4).</summary>
public sealed class UdpLayer : ILayer
{
    public required ushort SourcePort { get; init; }
    public required ushort DestinationPort { get; init; }
    public ILayer? Payload { get; init; }

    public void Write(ref PacketWriter writer)
    {
        int start = writer.Length;

        writer.WriteUInt16(SourcePort);
        writer.WriteUInt16(DestinationPort);
        Span<byte> length = writer.Reserve(2);   // header + payload, filled below
        writer.WriteUInt16(0);                  //checksum: 0 = not computed

        Payload?.Write(ref writer);

        BinaryPrimitives.WriteUInt16BigEndian(length, (ushort)(writer.Length - start));
    }
}