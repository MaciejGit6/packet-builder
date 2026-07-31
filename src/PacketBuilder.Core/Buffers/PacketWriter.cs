using System.Buffers.Binary;

namespace PacketBuilder.Core.Buffers;

/// <summary>Forward-only writer over a span, emitting fields big-endian (network order).</summary>
public ref struct PacketWriter
{
    private readonly Span<byte> _buffer;
    private int _position;

    public PacketWriter(Span<byte> buffer)
    {
        _buffer = buffer;
        _position = 0;
    }

    public readonly int Length => _position;
    public readonly ReadOnlySpan<byte> Written => _buffer[.._position];

    public void WriteUInt8(byte value) => _buffer[_position++] = value;

    public void WriteUInt16(ushort value)
    {
        BinaryPrimitives.WriteUInt16BigEndian(_buffer.Slice(_position, 2), value);
        _position += 2;
    }

    public void WriteUInt32(uint value)
    {
        BinaryPrimitives.WriteUInt32BigEndian(_buffer.Slice(_position, 4), value);
        _position += 4;
    }

    public void WriteBytes(ReadOnlySpan<byte> value)
    {
        value.CopyTo(_buffer.Slice(_position, value.Length));
        _position += value.Length;
    }

    /// <summary>Reserve bytes now, fill them later (for lengths/checksums).</summary>
    public Span<byte> Reserve(int count)
    {
        var slice = _buffer.Slice(_position, count);
        _position += count;
        return slice;
    }
}