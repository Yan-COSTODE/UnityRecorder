using UnityEngine;

public class UseRecorder : MonoBehaviour
{
    #region Fields & Properties
    #region Fields
    [SerializeField] private KeyCode input = KeyCode.LeftShift;
    #endregion

    #region Properties
    #endregion
    #endregion
    
    #region Methods
    private void Update()
    {
        if (Input.GetKeyDown(input))
            UnityRecorder.Instance.LaunchPlayBack();
    }
    #endregion
}
