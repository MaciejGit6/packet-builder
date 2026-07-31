using PacketBuilder.Core.Checksums;
using Xunit;

namespace PacketBuilder.Core.Tests;

public class InternetChecksumTests
{
    // Real IPv4 header, checksum field zeroed; known result is 0xB861.
    private static readonly byte[] Ipv4Header =
    {
        0x45, 0x00, 0x00, 0x73, 0x00, 0x00, 0x40, 0x00,
        0x40, 0x11, 0x00, 0x00, 0xC0, 0xA8, 0x00, 0x01,
        0xC0, 0xA8, 0x00, 0xC7,
    };

    [Fact]
    public void Fold_MatchesKnownChecksum()
    {
        var c = new InternetChecksum();
        c.Add(Ipv4Header);
        Assert.Equal((ushort)0xB861, c.Fold());
    }
}