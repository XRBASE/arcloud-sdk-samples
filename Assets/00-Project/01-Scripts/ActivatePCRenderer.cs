using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePCRenderer : MonoBehaviour
{
    public void OpenRenderer() {
        PointcloudController.Instance.OpenViewer();
    }
}
