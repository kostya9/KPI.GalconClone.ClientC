using strange.extensions.dispatcher.eventdispatcher.api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchEventSystemView : BaseView
{

    private Vector3 previousTouchPos;
    private bool holding = false;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            dispatcher.Dispatch(Events.AppBack);
        }
#if UNITY_EDITOR
        UpdateEditorTouches();
#else
        UpdateDeviceTouches();
#endif
    }

    private void UpdateEditorTouches()
    {
        var mousePosition = Input.mousePosition;
        var previousFrameHolding = holding;
        holding = Input.GetMouseButton(0);
        if (holding && !previousFrameHolding)
        {
            GenerateTapEvent(mousePosition);
            previousTouchPos = mousePosition;
        }
        else if (holding && previousFrameHolding)
        {
            GenerateSwipeEvent(mousePosition);
            previousTouchPos = mousePosition;
        }
        else if (!holding && previousFrameHolding)
        {
            GenerateSwipeEndEvent();
        }
    }

    private void UpdateDeviceTouches()
    {
        var previousFrameHolding = holding;
        if (Input.touchCount > 0)
        {
            holding = true;
            var touch = Input.GetTouch(0);
            var touchPosition = touch.position;
            if (holding && !previousFrameHolding)
            {
                GenerateTapEvent(touchPosition);
                previousTouchPos = touchPosition;
            }
            else if (holding && previousFrameHolding)
            {
                GenerateSwipeEvent(touchPosition);
                previousTouchPos = touchPosition;
            }
        }
        else
        {
            holding = false;
            if (!holding && previousFrameHolding)
            {
                GenerateSwipeEndEvent();
            }
        }
    }

    private void GenerateTapEvent(Vector2 pos)
    {
        dispatcher.Dispatch(Events.Tap, pos);
    }

    private void GenerateSwipeEvent(Vector2 pos)
    {
        var swipe = new SwipeModel(previousTouchPos, pos);
        dispatcher.Dispatch(Events.Swipe, swipe);

    }

    private void GenerateSwipeEndEvent()
    {
        dispatcher.Dispatch(Events.SwipeEnd);
    }

}
