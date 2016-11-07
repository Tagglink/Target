using UnityEngine;
using System.Collections;

public class mainMenuManager : MonoBehaviour {

    public Camera Camera;

    public Canvas mainCanvas;
    public Canvas playCanvas;
    public Canvas settCanvas;
    private Canvas currentCanvas;

    private Vector3 cameraPosition;
    private Vector3 canvasPosition;

    private Quaternion cameraRotation;
    private Quaternion canvasRotation;

    public float speed = 1.0F;
    private float startTime;
    private float journeyLength;

    private bool isLerping;

    void Start()
    {
        currentCanvas = mainCanvas;
    }

    public void ChangeCanvas(Canvas newCanvas)
    {
        CanvasGroup currentCanvasGroup = currentCanvas.GetComponent<CanvasGroup>();
        CanvasGroup newCanvasGroup = newCanvas.GetComponent<CanvasGroup>();

        newCanvasGroup.alpha = 1;
        newCanvasGroup.interactable = true;

        StartCoroutine(HideMenu(newCanvas, currentCanvasGroup));

        AnimateToCanvas(newCanvas);
    }

    public void AnimateToCanvas(Canvas canvas)
    {
        canvasPosition = canvas.transform.FindChild("TargetPosition").transform.position;
        cameraPosition = Camera.transform.position;

        canvasRotation = canvas.transform.FindChild("TargetPosition").transform.rotation;
        cameraRotation = Camera.transform.rotation;

        isLerping = true;
        startTime = Time.time;
    }

    IEnumerator HideMenu(Canvas newCanvas, CanvasGroup currentCanvasGroup)
    {
        yield return new WaitForSeconds(0.5f);

        currentCanvas = newCanvas;

        currentCanvasGroup.alpha = 0;
        currentCanvasGroup.interactable = false;
    }

    void Update()
    {
        if (isLerping)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / 10;

            Camera.transform.position = Vector3.Lerp(cameraPosition, canvasPosition, fracJourney);

            Camera.transform.rotation = Quaternion.Slerp(cameraRotation, canvasRotation, fracJourney);

            if (fracJourney >= 1)
            {
                isLerping = false;
            }
        }
    }
}
