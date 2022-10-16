using System.Collections;

namespace States
{
    public class PlaceUnitsTurn: State
    {
        public override IEnumerator Start()
        {
            // TODO see your cards & the map
            return base.Start();
        }

        public override IEnumerator Action()
        {
            // TODO place cards on territories
            return base.Action();
        }

        public override IEnumerator End()
        {
            // TODO press next phase button (BattleTurn)
            return base.End();
        }
    }
}