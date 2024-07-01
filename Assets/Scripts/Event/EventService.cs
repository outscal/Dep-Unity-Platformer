/**  This script demonstrates implementation of the Observer Pattern.
*  If you're interested in learning about Observer Pattern, 
*  you can find a dedicated course on Outscal's website.
*  Link: https://outscal.com/courses
**/
using Platformer.InputSystem;
using UnityEngine;

namespace Platformer.Events
{
    public class EventService
    {
        #region Input events
        public EventController<float> OnHorizontalAxisInputReceived { get; private set; }
        public EventController<PlayerInputTriggers> OnPlayerTriggerInputReceived { get; private set; }
        #endregion

        #region UI event
        public EventController<int> OnLevelSelected { get; private set; }
        #endregion

        #region Player Events
        public EventController<Vector3> OnPlayerMoved { get; private set; }
        #endregion

        public EventService()
        {
            OnLevelSelected = new EventController<int>();
            OnHorizontalAxisInputReceived = new EventController<float>();
            OnPlayerTriggerInputReceived = new EventController<PlayerInputTriggers>();
            OnPlayerMoved = new EventController<Vector3>();
        }
    }
}