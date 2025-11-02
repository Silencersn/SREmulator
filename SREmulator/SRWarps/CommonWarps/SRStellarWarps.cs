namespace SREmulator.SRWarps.CommonWarps;

public static class SRStellarWarps
{
    public static readonly SRStellarWarp Ver1p0 = new(SRVersion.Ver1p0);
    public static readonly SRStellarWarp Ver1p1 = new(SRVersion.Ver1p1);
    public static readonly SRStellarWarp Ver1p2 = new(SRVersion.Ver1p2);
    public static readonly SRStellarWarp Ver1p3 = new(SRVersion.Ver1p3);
    public static readonly SRStellarWarp Ver1p4 = new(SRVersion.Ver1p4);
    public static readonly SRStellarWarp Ver1p5 = new(SRVersion.Ver1p5);
    public static readonly SRStellarWarp Ver1p6 = new(SRVersion.Ver1p6);
    public static readonly SRStellarWarp Ver2p0 = new(SRVersion.Ver2p0);
    public static readonly SRStellarWarp Ver2p1 = new(SRVersion.Ver2p1);
    public static readonly SRStellarWarp Ver2p2 = new(SRVersion.Ver2p2);
    public static readonly SRStellarWarp Ver2p3 = new(SRVersion.Ver2p3);
    public static readonly SRStellarWarp Ver2p4 = new(SRVersion.Ver2p4);
    public static readonly SRStellarWarp Ver2p5 = new(SRVersion.Ver2p5);
    public static readonly SRStellarWarp Ver2p6 = new(SRVersion.Ver2p6);
    public static readonly SRStellarWarp Ver2p7 = new(SRVersion.Ver2p7);

    public static SRStellarWarp GetStellarWarpByVersion(SRVersion version)
    {
        return new SRStellarWarp(version);
    }
}
