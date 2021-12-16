<Query Kind="Program">
  <NuGetReference>Shouldly</NuGetReference>
  <Namespace>Xunit</Namespace>
  <Namespace>Shouldly</Namespace>
</Query>

#load ".\AdventOfCode"

// 1101 0010 1111 1110 0010 1000
// VVVT TTAA AAAB BBBB CCCC C
void Main()
{
    var input = AdventOfCode.ReadAll(16, false);


    var packet = new BitsPacketFactory(input.ToBitsString()).GetNext();
    packet.SumVersions().Dump("Part One");
    packet.GetValue().Dump("Part Two");

}



public static class Helpers
{
    public static byte[] ToBytes(this string hexString)
    {
        return hexString.Select(x => Convert.ToByte(x.ToString(), 16)).ToArray();
    }

    public static string ToBitsString(this string input)
    {
        var sb = new StringBuilder();
        foreach (var b in input.ToBytes())
        {
            sb.Append(Convert.ToString(b, 2).PadLeft(4, '0'));
        }
        return sb.ToString();
    }
}

public class BitsPacketFactory
{
    private StringReader _sr;

    public BitsPacketFactory(string bits)
    {
        _sr = new StringReader(bits);
    }

    public BitsPacket GetNext(bool trimPadding = true)
    {
        var version = GetNextBits(3);
        var typeId = GetNextBits(3);

        switch (typeId)
        {
            case 4:
                return GetLiteral(version, typeId, trimPadding);
            default:
                return GetOperator(version, typeId);

        }
    }

    private LiteralValue GetLiteral(int version, int typeId, bool trimPadding)
    {
        int value = 0;
        var read = 6;

        while (true)
        {
            var temp = GetNextBits(5);
            read += 5;
            value = value << 4;
            value |= temp & 0b01111;

            if ((temp & 0b10000) != 0b10000)
                break;
        }

        // Lazy effort at trim padding
        while (trimPadding && read % 4 != 0)
        {
            _sr.Read();
            read++;
        }

        return new LiteralValue
        {
            Version = version,
            TypeId = typeId,
            Value = value,
            Read = read,
        };
    }

    private Operator GetOperator(int version, int typeId)
    {
        var lengthType = GetNextBits(1);
        var read = 6 + 1;

        List<BitsPacket> subPackets = new();
        if (lengthType == 1)
        {
            var numPackets = GetNextBits(11);
            read += 11;
            for (var i = 0; i < numPackets; i++)
            {
                subPackets.Add(GetNext(false));
            }
        }
        if (lengthType == 0)
        {
            var bitsToRead = GetNextBits(15);
            read += 15;
            while (true)
            {
                subPackets.Add(GetNext(false));
                if (subPackets.Sum(x => x.SumRead()) == bitsToRead)
                    break;
            }
        }
        return new Operator
        {
            Version = version,
            TypeId = typeId,
            LengthType = lengthType,
            Read = read,
            SubPackets = subPackets
        };
    }

    public int GetNextBits(int n)
    {
        var sb = new StringBuilder();
        for (var i = 0; i < n; i++)
        {
            sb.Append(_sr.Read() - '0');
        }
        return Convert.ToInt32(sb.ToString(), 2);
    }
}

public abstract class BitsPacket
{
    public int Version { get; set; }
    public int TypeId { get; set; }
    public int Read { get; set; }

    public bool IsLiteral => TypeId == 4; // Redundant but meh
    public List<BitsPacket> SubPackets { get; set; } = new();

    public int SumVersions()
    {
        return Version + SubPackets.Select(x => x.SumVersions()).Sum();
    }

    public int SumRead()
    {
        return Read + SubPackets.Select(x => x.SumRead()).Sum();
    }

    public abstract long GetValue();
}

public class LiteralValue : BitsPacket
{
    public int Value { get; set; }

    public override long GetValue()
    {
        return Value;
    }
}

public class Operator : BitsPacket
{
    public int LengthType { get; set; }

    public override long GetValue()
    {
        switch (TypeId)
        {
            case 0:
                return SubPackets.Sum(x => x.GetValue());
            case 1:
                return SubPackets.Select(x => x.GetValue()).Aggregate(1L, (x, y) => x * y);
            case 2:
                return SubPackets.Min(x => x.GetValue());
            case 3:
                return SubPackets.Max(x => x.GetValue());
            case 5:
                return SubPackets.First().GetValue() > SubPackets.Last().GetValue() ? 1 : 0;
            case 6:
                return SubPackets.First().GetValue() > SubPackets.Last().GetValue() ? 0 : 1;
            case 7:
                return SubPackets.First().GetValue() == SubPackets.Last().GetValue() ? 1 : 0;
        }
        throw new NotImplementedException();
    }
}

#region private::Tests

[Fact]
void ToBytes_WithSimpleInput_Works()
{
    var result = "A1B".ToBytes();
    result.Length.ShouldBe(3);
    result[0].ShouldBe((byte)0xA);
    result[1].ShouldBe((byte)0x1);
    result[2].ShouldBe((byte)0xB);
}

[Fact]
void BitsFactory_WithSimpleInput_Works()
{
    var packet = new BitsPacketFactory("D2FE28".ToBitsString()).GetNext();
}

[Fact]
void BitsFactory_WithOperator_Works()
{
    var packet = new BitsPacketFactory("38006F45291200".ToBitsString()).GetNext();
}

[Fact]
void BitsFactory_WithAnotherOperator_Works()
{
    var packet = new BitsPacketFactory("EE00D40C823060".ToBitsString()).GetNext();
}

[Fact]
void BitsFactory_few_more_8A004A801A8002F478_Works()
{
    var packet = new BitsPacketFactory("8A004A801A8002F478".ToBitsString()).GetNext();
    packet.SumVersions().ShouldBe(16);
}

[Fact]
void BitsFactory_few_more_620080001611562C8802118E34_Works()
{
    var packet = new BitsPacketFactory("620080001611562C8802118E34".ToBitsString()).GetNext();
    packet.Dump("620080001611562C8802118E34");
    packet.SumVersions().ShouldBe(12);
}


[Fact]
void BitsFactory_few_more_C0015000016115A2E0802F182340_Works()
{
    var packet = new BitsPacketFactory("C0015000016115A2E0802F182340".ToBitsString()).GetNext();
    packet.SumVersions().ShouldBe(23);
}

[Fact]
void BitsFactory_few_more_A0016C880162017C3686B18A3D4780_Works()
{
    var packet = new BitsPacketFactory("A0016C880162017C3686B18A3D4780".ToBitsString()).GetNext();
    packet.SumVersions().ShouldBe(31);
}

[Theory]
[InlineData("C200B40A82", 3)]
[InlineData("04005AC33890", 54)]
[InlineData("880086C3E88112", 7)]
[InlineData("CE00C43D881120", 9)]
[InlineData("D8005AC2A8F0", 1)]
[InlineData("F600BC2D8F", 0)]
[InlineData("9C005AC2F8F0", 0)]
[InlineData("9C0141080250320F1802104A08", 1)]
void BitsPacket_Meh_GetValue(string input, int expected)
{
    var packet = new BitsPacketFactory(input.ToBitsString()).GetNext();
    packet.GetValue().ShouldBe(expected);
}


#endregion

