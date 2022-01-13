namespace Mills.Common.Enum
{
    public enum RequestMethod
    {
        None,

        // System
        Error,
        Login,
        LoggedIn,
        Logout,
        Register,
        Registered,
        SendActiveUsers,
        Challenge,
        SendChallenges,
        ChallengeAccepted,
        ChallengeCancelled,
        GameStarted,
        SendMessage,

        //Gameplay
        Place,
        Placed,
        Move,
        Moved,
        Remove,
        Removed,
        Lose,
        Win,
        Forfeit
    }
}
