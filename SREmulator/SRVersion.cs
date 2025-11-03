namespace SREmulator;

// +------------------------+------------------------+---+---+---------------------------------------+---+
// | 1  1  1  1  1  1  1  1 | 1  1  1  1  1  1  1  1 | 1 | 1 | 0  0  0  0  0  0  0  0  0  0  0  0  0 | 1 |
// +------------------------+------------------------+---+---+---------------------------------------+---+
//           Major                    Minor        Phase2 Phase1             Reserved              Specified
[Flags]
public enum SRVersion : uint
{
    Unspecified = 0x00_00_00_00,
    Specified = 0x00_00_00_01,

    MajorMask = 0xFF_00_00_00,
    MinorMask = 0x00_FF_00_00,
    SpecifiedVersionMask = MajorMask | MinorMask | Specified,

    Major1 = 0x01_00_00_00 | Specified,
    Major2 = 0x02_00_00_00 | Specified,
    Major3 = 0x03_00_00_00 | Specified,

    Minor0 = 0x00_00_00_00 | Specified,
    Minor1 = 0x00_01_00_00 | Specified,
    Minor2 = 0x00_02_00_00 | Specified,
    Minor3 = 0x00_03_00_00 | Specified,
    Minor4 = 0x00_04_00_00 | Specified,
    Minor5 = 0x00_05_00_00 | Specified,
    Minor6 = 0x00_06_00_00 | Specified,
    Minor7 = 0x00_07_00_00 | Specified,
    Minor8 = 0x00_08_00_00 | Specified,

    Phase1 = 0x00_00_01_00 | Specified,
    Phase2 = 0x00_00_02_00 | Specified,

    Ver1p0 = Major1 | Minor0,
    Ver1p1 = Major1 | Minor1,
    Ver1p2 = Major1 | Minor2,
    Ver1p3 = Major1 | Minor3,
    Ver1p4 = Major1 | Minor4,
    Ver1p5 = Major1 | Minor5,
    Ver1p6 = Major1 | Minor6,

    Ver2p0 = Major2 | Minor0,
    Ver2p1 = Major2 | Minor1,
    Ver2p2 = Major2 | Minor2,
    Ver2p3 = Major2 | Minor3,
    Ver2p4 = Major2 | Minor4,
    Ver2p5 = Major2 | Minor5,
    Ver2p6 = Major2 | Minor6,
    Ver2p7 = Major2 | Minor7,

    Ver3p0 = Major3 | Minor0,
    Ver3p1 = Major3 | Minor1,
    Ver3p2 = Major3 | Minor2,
    Ver3p3 = Major3 | Minor3,
    Ver3p4 = Major3 | Minor4,
    Ver3p5 = Major3 | Minor5,
    Ver3p6 = Major3 | Minor6,
    Ver3p7 = Major3 | Minor7,
}

public static class SRVersions
{
    private static SRVersion InternalCreate(byte major, byte minor)
    {
        var majorEnum = (SRVersion)((uint)major << 8 * 3);
        var minorEnum = (SRVersion)((uint)minor << 8 * 2);
        return majorEnum | minorEnum | SRVersion.Specified;
    }

    private static int GetMaxAvailableMinor(int major)
    {
        return major switch
        {
            1 => 6,
            2 => 7,
            3 => 8,

            _ => 0
        };
    }

    public static SRVersion Create(int major, int minor)
    {
        return InternalCreate((byte)major, (byte)minor);
    }
    public static SRVersion CreateAvailable(int major, int minor)
    {
        major = Math.Clamp(major, 1, 3);
        minor = Math.Clamp(minor, 0, GetMaxAvailableMinor(major));
        return InternalCreate((byte)major, (byte)minor);
    }
}
