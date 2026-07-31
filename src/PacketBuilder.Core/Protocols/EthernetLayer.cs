using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacketBuilder.Core.Buffers;

namespace PacketBuilder.Core.Protocols;
public sealed class EthernetLayer : ILayer
{
    public required MacAddress Destination { get; init; }
    public required MacAddress Source { get; init; }
    public required EtherType EtherType { get; init; }
    public ILayer? Payload { get; init; }

    public void Write(ref PacketWriter writer)
    {
        Destination.WriteTo(writer.Reserve(6));
        Source.WriteTo(writer.Reserve(6));
        writer.WriteUInt16((ushort)EtherType);
        Payload?.Write(ref writer);
    }
}
