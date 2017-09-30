using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D3Slider : MonoBehaviour {

    public float value;

    private void OnMouseDrag()
    {
        RaycastHit hit;
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 150);
        transform.localPosition = Aralik(Quaternion.Euler(-transform.eulerAngles) * (hit.point- transform.parent.position));
        value = transform.localPosition.x * 0.5f;
    }

    Vector3 Aralik(Vector3 posizyon)
    {
        if (posizyon.x > 2)
        {
            posizyon.x = 2;
        }
        else if (posizyon.x < 0)
        {
            posizyon.x = 0;
        }
        posizyon.y = 0;
        posizyon.z = 0;
        return new Vector3(posizyon.x, posizyon.y, posizyon.z);
    }

}
