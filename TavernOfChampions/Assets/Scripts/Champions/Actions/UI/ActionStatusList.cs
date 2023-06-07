using UnityEngine;

namespace TavernOfChampions.Champion.Actions.UI
{
    public class ActionStatusList : MonoBehaviour
    {
        [SerializeField] private Transform _content;
        [SerializeField] private ActionStatus _actionStatus;

        public ActionStatus CreateActionStatus(Sprite sprite, int count)
        {
            var newActionStatus = Instantiate(_actionStatus, _content);
            newActionStatus.SetSprite(sprite);
            newActionStatus.SetCount(count);

            return newActionStatus;
        }

        public void DestroyActionStatus(ActionStatus actionStatus)
        {
            Destroy(actionStatus.gameObject);
        }
    }
}