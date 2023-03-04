using UnityEngine;

public class UISwitcher : MonoBehaviour
{
    [SerializeField] private SerializableStringDictionary<GameObject> _UIDict;

    private GameObject _previousUI;

    public void ChangeUI(string UIName)
    {
        if (!_previousUI)
            CloseAllUI();
        else
            _previousUI.SetActive(false);

        _UIDict[UIName].SetActive(true);
        _previousUI = _UIDict[UIName];
    }

    private void CloseAllUI()
    {
        foreach(var keyValuePair in _UIDict)
        {
            keyValuePair.Value.SetActive(false);
        }
    }
}
