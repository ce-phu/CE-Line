using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public bool isNext = false;
    public bool isFilled = false;
    public bool isBase = false;
    public bool isDisable = false;
    public bool isActive = false;
    public bool instructionShown = false;

    public Image image;

    public int row;
    public int col;

    [SerializeField] private Animator anim;

    private Color normalColor = new Color(0.8627451f, 0.8666667f, 0.8784314f);

    [SerializeField] private RectTransform baseImage;
    [SerializeField] private RectTransform filledRightAngle;
    [SerializeField] private RectTransform filledStraightAngle;
    [SerializeField] private RectTransform filledSingle;

    public enum NextDirection
    {
        NOTINITIALIZE,
        TOP,
        BOTTOM,
        LEFT,
        RIGHT,
        NONE,
    }

    public enum PreviousDirection
    {
        NOTINITIALIZE,
        TOP,
        BOTTOM,
        LEFT,
        RIGHT,
    }

    private void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        if (isDisable) return;

        if (Input.GetMouseButtonDown(0))
            GameManager.isPointerDown = true;

        if (Input.GetMouseButtonUp(0))
            GameManager.isPointerDown = false;
    }

    public void In()
    {
        anim.Play("Cell_In");
    }

    public void Out()
    {
        anim.Play("Cell_Out");
    }

    public void FinishOutAnimation()
    {
        Destroy(gameObject);
    }

    private void SetSelected()
    {
        if (isDisable) return;
        if (isBase) return;

        isFilled = true;
        anim.Play("Cell_Fill");
    }

    public void SetNormal()
    {
        if (isDisable) return;
        if (isBase) return;

        isNext = false;

        if (isFilled)
        {
            anim.Play("Cell_UnFill");
        }
        else
        {
            SetTexture(PreviousDirection.NOTINITIALIZE, NextDirection.NOTINITIALIZE);
        }

        isFilled = false;

        image.color = !instructionShown ? normalColor : new Color(normalColor.r, normalColor.g, normalColor.b, .5f);
    }

    public void SetUnFilledAnimationCompleted()
    {
        SetTexture(PreviousDirection.NOTINITIALIZE, NextDirection.NOTINITIALIZE);
    }

    public void SetInstructionShown()
    {
        if (isDisable) return;
        if (isBase) return;

        isNext = false;
        isFilled = false;
        instructionShown = true;

        image.color = new Color(normalColor.r, normalColor.g, normalColor.b, .5f);
    }

    public void SetBase()
    {
        if (isDisable) return;
        isBase = true;

        baseImage.gameObject.SetActive(true);
        filledRightAngle.gameObject.SetActive(false);
        filledStraightAngle.gameObject.SetActive(false);
        filledSingle.gameObject.SetActive(false);
    }

    public void SetDisable()
    {
        isDisable = true;
        isBase = false;
        image.color = new Color(1, 1, 1, 0);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isDisable) return;
        // Debug.Log("Clicked: " + row + " " + col);
        TriggerAction();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isDisable) return;
        if (GameManager.isPointerDown)
        {
            // Debug.Log("Dragged over: " + row + " " + col);
            TriggerAction();
        }
    }

    private void TriggerAction()
    {
        if (isDisable) return;
        if (isBase)
        {
            GameManager.ResetAllCell();
            GameManager.CheckAdjacentCell(this);
        }

        if (isNext || isFilled)
        {
            SetSelected();
            GameManager.SetActiveCell(this);
            GameManager.CheckAdjacentCell(this);
        }
    }

    public void SetAdjacent()
    {
        // if (image.color != Color.yellow)
        // {
        //     image.color = Color.cyan;
        // }
        // else
        // {
        //     image.color = Color.green;
        // }
        //
        isNext = true;
    }

    public void SetUnAdjacent()
    {
        // if (image.color != Color.red && image.color != Color.yellow)
        // {
        //     image.color = Color.white;
        // }

        isNext = false;
    }

    public void SetTexture(PreviousDirection prevDirection, NextDirection nextDirection = NextDirection.NONE)
    {
        baseImage.gameObject.SetActive(false);
        filledRightAngle.gameObject.SetActive(false);
        filledStraightAngle.gameObject.SetActive(false);
        filledSingle.gameObject.SetActive(false);

        if (prevDirection == PreviousDirection.NOTINITIALIZE && nextDirection == NextDirection.NOTINITIALIZE)
        {
            baseImage.gameObject.SetActive(false);
            filledRightAngle.gameObject.SetActive(false);
            filledStraightAngle.gameObject.SetActive(false);
            filledSingle.gameObject.SetActive(false);
        }

        if (nextDirection == NextDirection.NONE)
        {
            filledSingle.gameObject.SetActive(true);
            switch (prevDirection)
            {
                case PreviousDirection.TOP:
                    filledSingle.localRotation = Quaternion.Euler(0, 0, 90);
                    break;
                case PreviousDirection.BOTTOM:
                    filledSingle.localRotation = Quaternion.Euler(0, 0, 270);
                    break;
                case PreviousDirection.LEFT:
                    filledSingle.localRotation = Quaternion.Euler(0, 0, 180);
                    break;
                case PreviousDirection.RIGHT:
                    filledSingle.localRotation = Quaternion.Euler(0, 0, 0);
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (prevDirection)
            {
                case PreviousDirection.TOP:
                    switch (nextDirection)
                    {
                        case NextDirection.BOTTOM:
                            filledStraightAngle.gameObject.SetActive(true);
                            filledStraightAngle.localRotation = Quaternion.Euler(0, 0, 90);
                            break;
                        case NextDirection.LEFT:
                            filledRightAngle.gameObject.SetActive(true);
                            filledRightAngle.localRotation = Quaternion.Euler(0, 0, 90);
                            break;
                        case NextDirection.RIGHT:
                            filledRightAngle.gameObject.SetActive(true);
                            filledRightAngle.localRotation = Quaternion.Euler(0, 0, 0);
                            break;
                        default:
                            break;
                    }

                    break;
                case PreviousDirection.BOTTOM:
                    switch (nextDirection)
                    {
                        case NextDirection.TOP:
                            filledStraightAngle.gameObject.SetActive(true);
                            filledStraightAngle.localRotation = Quaternion.Euler(0, 0, 90);
                            break;
                        case NextDirection.LEFT:
                            filledRightAngle.gameObject.SetActive(true);
                            filledRightAngle.localRotation = Quaternion.Euler(0, 0, 180);
                            break;
                        case NextDirection.RIGHT:
                            filledRightAngle.gameObject.SetActive(true);
                            filledRightAngle.localRotation = Quaternion.Euler(0, 0, 270);
                            break;
                        default:
                            break;
                    }

                    break;
                case PreviousDirection.LEFT:
                    switch (nextDirection)
                    {
                        case NextDirection.TOP:
                            filledRightAngle.gameObject.SetActive(true);
                            filledRightAngle.localRotation = Quaternion.Euler(0, 0, 90);
                            break;
                        case NextDirection.BOTTOM:
                            filledRightAngle.gameObject.SetActive(true);
                            filledRightAngle.localRotation = Quaternion.Euler(0, 0, 180);
                            break;
                        case NextDirection.RIGHT:
                            filledStraightAngle.gameObject.SetActive(true);
                            filledStraightAngle.localRotation = Quaternion.Euler(0, 0, 0);
                            break;
                        default:
                            break;
                    }

                    break;
                case PreviousDirection.RIGHT:
                    switch (nextDirection)
                    {
                        case NextDirection.TOP:
                            filledRightAngle.gameObject.SetActive(true);
                            filledRightAngle.localRotation = Quaternion.Euler(0, 0, 0);
                            break;
                        case NextDirection.BOTTOM:
                            filledRightAngle.gameObject.SetActive(true);
                            filledRightAngle.localRotation = Quaternion.Euler(0, 0, 270);
                            break;
                        case NextDirection.LEFT:
                            filledStraightAngle.gameObject.SetActive(true);
                            filledStraightAngle.localRotation = Quaternion.Euler(0, 0, 0);
                            break;
                        default:
                            break;
                    }

                    break;
                default:
                    break;
            }
        }
    }
}