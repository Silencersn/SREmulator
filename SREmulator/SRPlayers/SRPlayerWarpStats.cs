namespace SREmulator.SRPlayers;

public sealed class SRPlayerWarpStats : ISRPlayerStats<SRPlayerWarpStats>
{
    public bool Guarantee5;
    public bool Guarantee4;
    public int Counter5;
    public int Counter4;
    public int Counter5Character;
    public int Counter5LightCone;
    public int Counter4Character;
    public int Counter4LightCone;
    public ulong? ExtraInfo;

    public void GetStar5(bool isUp, bool isCharacter)
    {
        Guarantee5 = !isUp;
        Counter5 = 0;
        Counter4++;
        Counter4Character++;
        Counter4LightCone++;
        if (isCharacter)
        {
            Counter5Character = 0;
            Counter5LightCone++;
        }
        else
        {
            Counter5Character++;
            Counter5LightCone = 0;
        }
    }
    public void GetStar4(bool isUp, bool isCharacter)
    {
        Guarantee4 = !isUp;
        Counter5++;
        Counter4 = 0;
        Counter5Character++;
        Counter5LightCone++;
        if (isCharacter)
        {
            Counter4Character = 0;
            Counter4LightCone++;
        }
        else
        {
            Counter4Character++;
            Counter4LightCone = 0;
        }
    }
    public void GetStar3()
    {
        Counter5++;
        Counter4++;
        Counter5Character++;
        Counter5LightCone++;
        Counter4Character++;
        Counter4LightCone++;
    }

    public SRPlayerWarpStats Clone()
    {
        return (SRPlayerWarpStats)MemberwiseClone();
    }
}
