using UnityEngine;

namespace Model
{
    public class GameStateView : MonoBehaviour
    {
        [field: SerializeField] private BoxCollider _gameLoseTrigger;
        
        public BoxCollider GameLoseTrigger => _gameLoseTrigger;
    }
}