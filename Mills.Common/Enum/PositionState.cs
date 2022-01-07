using System;

namespace Mills.Common.Enum
{
    /// <summary>
    /// Enum der möglichen Werte, die eine Position auf dem Spielbrett annehmen kann
    /// </summary>
    [Flags]
    public enum PositionState
    {
        Player1 = 1,
        Player2 = 2,
        AvailableForMove = 4,
        // Kombinierbar mit Player1 und Player2 um einen Spielstein als antfernbar zu markieren
        RemoveTarget = 8
    }
}
