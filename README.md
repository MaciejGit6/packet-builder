# packet-builder

A little C# tool that builds raw network packets byte by byte, from the Ethernet header up.

it generates test traffic for **rawsight**, a packet sniffer I wrote in C. rawsight reads whole Ethernet frames off an interface and dissects them (ARP / ICMP / IP / TCP / UDP / CoAP / DTLS), so I needed something that could *produce* those frames on demand — crafted, malformed, whatever I want to throw at it. That's this.

The nice part is the two programs never touch each other's code. They only ever meet as bytes on the wire.

## What it does right now

You describe a packet as nested layers and it serializes them into a real frame:

```csharp
var eth = new EthernetLayer
{
    Destination = MacAddress.Broadcast,
    Source = MacAddress.Parse("de:ad:be:ef:00:01"),
    EtherType = EtherType.Ipv4,
    Payload = new Ipv4Layer
    {
        Source = Ipv4Address.Parse("192.168.0.1"),
        Destination = Ipv4Address.Parse("192.168.0.199"),
        Protocol = IpProtocol.Udp,
        Payload = new UdpLayer
        {
            SourcePort = 40000,
            DestinationPort = 5683, // CoAP
            Payload = new RawPayload(new byte[] { 0x40, 0x01, 0x00, 0x00 }),
        },
    },
};
```

Each layer writes its own header and the header fields that depend on the rest of the packet (total length, checksums) get patched in afterwards with a little `Reserve()` trick. The output is a hex string you can paste straight into Wireshark's "Import from Hex Dump" and watch it decode.

## Running it

Needs .NET 9.

```
dotnet build
dotnet test
dotnet run --project src/PacketBuilder.Cli
```

Right now the CLI just prints one hardcoded frame as hex, e.g.:

```
FFFFFFFFFFFFDEADBEEF00010806
```

(that one's a broadcast ARP request — the first thing I got working)

## Project layout

```
src/
  PacketBuilder.Core   <- the actual packet-building library
  PacketBuilder.Cli    <- prints a frame for now
tests/
  PacketBuilder.Core.Tests
```

Everything interesting is in `Core/Protocols` (one file per layer) and `Core/Checksums`.

## Done so far

- [x] big-endian `PacketWriter` over a `Span<byte>`
- [x] RFC 1071 internet checksum (+ a unit test with a known value)
- [x] Ethernet II, ARP, IPv4 (with header checksum), UDP layers
- [x] MAC / IPv4 address types

## TODO

- [ ] proper UDP checksum (needs the IPv4 pseudo-header)
- [ ] CoAP and DTLS layers
- [ ] actually inject frames onto an interface (veth pair) instead of just printing hex
- [ ] a scenario file so I don't have to hardcode packets in `Program.cs`
- [ ] a fuzz mode to throw malformed frames at rawsight and see what breaks

## Notes to self

- Ethernet II framing: rawsight's BPF filter reads the ethertype at offset 12, so the framing has to be exactly right or it filters my packets out.
- UDP checksum can legally be 0 on IPv4, which is why I got away with skipping it at first.
- Injecting real frames will need root / `CAP_NET_RAW` — same as rawsight.

Built for MiNI PW, Programming 3.
