namespace SREmulator.SRPlayers;

public sealed class SRPlayerLevelStats : ISRPlayerStats<SRPlayerLevelStats>
{
    /// <summary>
    /// 0 - 6
    /// <list type="table">
    ///     <listheader>
    ///         <term>
    ///             <see cref="EquilibriumLevel"/>
    ///         </term>
    ///         <description>
    ///             <see cref="TrailblazeLevel"/>
    ///         </description>
    ///     </listheader>
    ///     <item>
    ///         <term>0</term>
    ///         <description>[1, 20]</description>
    ///     </item>
    ///     <item>
    ///         <term>1</term>
    ///         <description>[21, 30]</description>
    ///     </item>
    ///     <item>
    ///         <term>2</term>
    ///         <description>[31, 40]</description>
    ///     </item>
    ///     <item>
    ///         <term>3</term>
    ///         <description>[41, 50]</description>
    ///     </item>
    ///     <item>
    ///         <term>4</term>
    ///         <description>[51, 60]</description>
    ///     </item>
    ///     <item>
    ///         <term>5</term>
    ///         <description>[61, 65]</description>
    ///     </item>
    ///     <item>
    ///         <term>6</term>
    ///         <description>[66, 70]</description>
    ///     </item>
    /// </list>
    /// </summary>
    public int EquilibriumLevel;
    /// <summary>
    /// 1 - 70
    /// </summary>
    public int TrailblazeLevel;

    public SRPlayerLevelStats Clone()
    {
        return (SRPlayerLevelStats)MemberwiseClone();
    }
}
