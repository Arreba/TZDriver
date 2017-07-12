namespace TZDriver.Models.Tools.Events
{
    public class ValueChangedEventArgs : System.EventArgs
    {
        public string Command { get; private set; }
        public double State { get; private set; }

        public ValueChangedEventArgs(string command, double state)
        {
            Command = command;
            State = state;
        }

    }
}
