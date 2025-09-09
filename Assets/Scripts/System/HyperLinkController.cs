using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;



public class HyperLinkController : MonoBehaviour, IPointerClickHandler {

    public void OnPointerClick( PointerEventData eventData ) {

        Vector2 pos             = Input.mousePosition;
        TextMeshProUGUI text    = GetComponent<TextMeshProUGUI>();
        Canvas canvas           = text.canvas;
        Camera camera           = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

        int index   = TMP_TextUtilities.FindIntersectingLink( text, pos, camera );

        if ( index != -1 ) {

            TMP_LinkInfo linkUrlInfo = text.textInfo.linkInfo[ index ];

            string url = linkUrlInfo.GetLinkID();

            Application.OpenURL( url );
        }
    }
}