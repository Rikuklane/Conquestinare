using System.Collections;

namespace States
{
    public class MarketTurn: State
    {
        public override IEnumerator Start()
        {
            // TODO open market place
            return base.Start();
        }

        public override IEnumerator Action()
        {
            // TODO be able to buy stuff
            return base.Action();
        }

        public override IEnumerator End()
        {
            // TODO press next phase button (PlaceUnitsTurn)
            return base.End();
        }
    }
}