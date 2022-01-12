namespace Mills.Common.Enum
{
    public enum RequestMethod
    {
        // System
        Ok,
        Error,
        Login,
        Logout,
        Register,
        GetActiveUsers,
        Challenge,
        SendMessage,
        ReceiveMessage,

        //Gameplay
        Place,
        Move,
        Remove,
        Lose,
        Win
    }
}
