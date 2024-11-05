namespace CranchyLib.Networking
{
    internal static class Extensions
    {
        internal static int ToMilliseconds(this double seconds)
        {
            double milliseconds = seconds * 1000.0;

            if (milliseconds > int.MaxValue)
            {
                return int.MaxValue;
            }
            else if (milliseconds < 0.01)
            {
                return 10;
            }

            return (int)milliseconds;
        }
    }
}
