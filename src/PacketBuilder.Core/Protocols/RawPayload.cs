using PacketBuilder.Core.Buffers;

namespace PacketBuilder.Core.Protocols;

/// <summary>A leaf layer that writes a fixed block of bytes</summary>
public sealed class RawPayload : ILayer
{
    private readonly byte[] _data;

    public RawPayload(byte[] data) => _data = data;

    public void Write(ref PacketWriter writer) => writer.WriteBytes(_data);
}