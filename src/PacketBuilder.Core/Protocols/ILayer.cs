using PacketBuilder.Core.Buffers;

namespace PacketBuilder.Core.Protocols;

/// <summary>A protocol layer that writes its header (and any nested payload) into a packet.</summary>
public interface ILayer
{
    void Write(ref PacketWriter writer);
}