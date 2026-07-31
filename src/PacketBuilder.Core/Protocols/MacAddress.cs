using System.Globalization;

namespace PacketBuilder.Core.Protocols;

/// <summary>A 48-bit Ethernet MAC address.</summary>
public readonly struct MacAddress
{
    private readonly byte _b0, _b1, _b2, _b3, _b4, _b5;

    public MacAddress(byte b0, byte b1, byte b2, byte b3, byte b4, byte b5)
        => (_b0, _b1, _b2, _b3, _b4, _b5) = (b0, b1, b2, b3, b4, b5);

    public static readonly MacAddress Broadcast = new(0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF);

    public void WriteTo(Span<byte> destination)
    {
        destination[0] = _b0; destination[1] = _b1; destination[2] = _b2;
        destination[3] = _b3; destination[4] = _b4; destination[5] = _b5;
    }

    public static MacAddress Parse(string text)
    {
        var parts = text.Split(':', '-');
        if (parts.Length != 6)
            throw new FormatException($"'{text}' is not a MAC address (expected 6 octets).");

        Span<byte> b = stackalloc byte[6];
        for (int i = 0; i < 6; i++)
            b[i] = byte.Parse(parts[i], NumberStyles.HexNumber, CultureInfo.InvariantCulture);

        return new MacAddress(b[0], b[1], b[2], b[3], b[4], b[5]);
    }

    public override string ToString()
        => $"{_b0:x2}:{_b1:x2}:{_b2:x2}:{_b3:x2}:{_b4:x2}:{_b5:x2}";
}