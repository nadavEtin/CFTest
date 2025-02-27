namespace Assets.Infrastructure.Events
{
    public class TapsUpdateEventParams : BaseEventParams
    {
        public int MovesLeft { get; }

        public TapsUpdateEventParams(int tapsLeft)
        {
            MovesLeft = tapsLeft;
        }
    }
}