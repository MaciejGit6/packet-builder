using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketBuilder.Core.Protocols;

/// <summary>
/// A 32bit Ipv4 address
/// </summary>


public readonly struct Ipv4Address
{
    private readonly byte _a, _b, _c, _d;

    public Ipv4Address(byte a, byte b, byte c, byte d) => (_a, _b, _c, _d) = (a, b, c, d);

    public void WriteTo(Span<byte> destination)
    {
        destination[0] = _a; destination[1] = _b; destination[2] = _c; destination[3] = _d;
    }

    public static Ipv4Address Parse(string text)
    {
        var p = text.Split('.');
        if (p.Length != 4)
            throw new FormatException($"'{text}' is not an Ipv4 address.");
        return new Ipv4Address(byte.Parse(p[0]), byte.Parse(p[1]), byte.Parse(p[2]), byte.Parse(p[3]));
    }
    public override string ToString() => $"{_a}.{_b}.{_c}.{_d}";
    
}
