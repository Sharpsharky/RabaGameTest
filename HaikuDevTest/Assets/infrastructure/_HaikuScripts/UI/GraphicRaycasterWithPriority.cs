using UnityEngine;
using UnityEngine.UI;

public class GraphicRaycasterWithPriority : GraphicRaycaster {

    [SerializeField]
    int _raycasterPriority;

    public override int sortOrderPriority {
        get {
            return _raycasterPriority;
        }
    }
}
