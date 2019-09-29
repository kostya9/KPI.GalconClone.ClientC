using strange.extensions.dispatcher.eventdispatcher.api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : BaseView
{

    [SerializeField] Transform ballTransform;
    [SerializeField] Rigidbody ballRigidbody;
    [SerializeField] float forceMultiplier;
    [SerializeField] Transform backPlane;
    [SerializeField] BallView ballView;
    [SerializeField] float finishPosition;
    [SerializeField] Vector3 cameraStartPosition;
    [SerializeField] Vector3 ballStartPosition;
    [SerializeField] Transform finishTransform;
    [SerializeField] TrailRenderer trail;

    [SerializeField] Button backButton;
    [SerializeField] GameObject swipeToStartText;

    private Camera mainCamera;
    private Vector2 swipeToBallRelativeViewportPos = Vector2.zero;
    private Vector3 velocity;
    private Vector3 previousTargetPosition;
    private Vector3 targetPosition;
    private float previousFrameTime;
    private float currentFrameTime;
    private bool isUnderControl = false;
    private bool gameStarted = false;
    private bool gameFinished = false;
    private float cameraVelocity = 5f;
    private List<LevelPartView> level;

    public void LoadView()
    {
        dispatcher.AddListener(Events.Swipe, OnSwipe);
        dispatcher.AddListener(Events.SwipeEnd, OnSwipeEnd);
        dispatcher.AddListener(Events.Tap, OnTap);
        dispatcher.AddListener(Events.AppBack, BackToMenu);
        mainCamera = Camera.main;
        ballView.onWrongObjectHit = EndGame;
        backButton.onClick.AddListener(BackToMenu);
        StartLevel();
    }

    public void RemoveView()
    {
        dispatcher.RemoveListener(Events.Swipe, OnSwipe);
        dispatcher.RemoveListener(Events.SwipeEnd, OnSwipeEnd);
        dispatcher.RemoveListener(Events.Tap, OnTap);
        dispatcher.RemoveListener(Events.AppBack, BackToMenu);
    }

    private void StartLevel()
    {
        ballTransform.position = ballStartPosition;
        ballRigidbody.velocity = Vector3.zero;
        mainCamera.transform.position = cameraStartPosition;
        gameStarted = false;
        isUnderControl = false;
        gameFinished = false;
        dispatcher.Dispatch(Events.GenerateLevel, this);
        finishTransform.position = Vector3.forward * finishPosition + Vector3.up * 0.01f;
        trail.Clear();
    }

    private void OnTap(IEvent eventData)
    {
        var position = (Vector2)eventData.data;
        swipeToBallRelativeViewportPos = mainCamera.ScreenToViewportPoint(position) - mainCamera.WorldToViewportPoint(ballTransform.position);
        currentFrameTime = Time.time;
        targetPosition = ballTransform.position;
        gameStarted = true;
    }

    public void OnSwipe(IEvent eventData)
    {
        if (gameFinished)
        {
            return;
        }
        var swipe = eventData.data as SwipeModel;
        Vector2 newPos = mainCamera.ScreenToViewportPoint(swipe.position);
        Vector2 ballViewportPos = mainCamera.WorldToViewportPoint(ballTransform.position);
        Plane plane = new Plane(Vector3.up, ballTransform.position);
        float distance = 0f;

        var targetViewportPosition = newPos - swipeToBallRelativeViewportPos;
        var ray = mainCamera.ViewportPointToRay(targetViewportPosition);
        if (plane.Raycast(ray, out distance))
        {
            previousFrameTime = currentFrameTime;
            currentFrameTime = Time.time;
            previousTargetPosition = targetPosition;
            targetPosition = ray.GetPoint(distance);
            isUnderControl = true;
        }
        else
        {
            isUnderControl = false;
        }

    }

    private void OnSwipeEnd()
    {
        isUnderControl = false;
    }



    private void FixedUpdate()
    {
        if (isUnderControl && !gameFinished)
        {
            // smoothing target pos (fixed time step 0.01)
            // for smooth movement
            var lerpedTargetPosition = Vector3.Lerp(previousTargetPosition, targetPosition, (Time.fixedTime - previousFrameTime) / (currentFrameTime - previousFrameTime) - 1f);
            var deltaPos = lerpedTargetPosition - ballRigidbody.position;
            ballRigidbody.velocity = deltaPos * forceMultiplier + cameraVelocity * Vector3.forward;
        }
    }

    private void LateUpdate()
    {
        swipeToStartText.SetActive(!gameFinished && !gameStarted);
        if (gameStarted && !gameFinished)
        {
            CheckGameFinished();
            mainCamera.transform.position += Vector3.forward * Time.deltaTime * cameraVelocity;
            Ray ray = mainCamera.ViewportPointToRay(Vector2.right / 2f);
            Plane plane = new Plane(Vector3.up, ballTransform.position);
            float distance = 0f;
            if (plane.Raycast(ray, out distance))
            {
                backPlane.transform.position = ray.GetPoint(distance);
            }
        }
    }

    private void EndGame()
    {
        Debug.Log("endGame");
        if (gameFinished)
        {
            return;
        }
        gameFinished = true;
        StartCoroutine(RestartLevel());
    }

    private IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(2f);
        DeleteLevel();
        StartLevel();
    }

    private void DeleteLevel()
    {
        foreach(var part in level)
        {
            part.gameObject.SetActive(false);
            Destroy(part.gameObject);
        }
    }

    private void CheckGameFinished()
    {
        if (ballTransform.position.z > finishPosition)
        {
            EndGame();
            gameFinished = true;
        }
    }

    public void SetLevel(List<LevelPartView> parts)
    {
        level = parts;
    }

    private void BackToMenu()
    {
        dispatcher.Dispatch(Events.UIMainScreenLoad);
        dispatcher.Dispatch(Events.GameRemove);
    }
}
