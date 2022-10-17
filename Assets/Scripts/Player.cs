using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] private LineRenderer line;
    [SerializeField] private float speed = 3;


    public Action OnCoinCollected;
    public Action OnPlayerDead;

    private SpriteRenderer sr;
    private Queue<Vector2> positionsQueue;
    private Queue<int> indexQueue;

    private void Start() {
        positionsQueue = new Queue<Vector2>();
        indexQueue = new Queue<int>();
        sr = GetComponent<SpriteRenderer>();

        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position);
        index = 1;

        StartCoroutine(MoveControl());
    }

    public void AddPosition(Vector2 position) {
        positionsQueue.Enqueue(position);
    }

    private int index;

    private IEnumerator MoveControl() {
        while (true) {
            yield return new WaitUntil(() => positionsQueue.Count != 0);
            index = line.positionCount;
            line.positionCount++;
            line.SetPosition(line.positionCount - 1, transform.position);
            var t = transform.DOMove(positionsQueue.Dequeue(), speed).SetEase(Ease.Linear).SetSpeedBased()
                .OnUpdate(delegate {
                    var lastIndex = line.positionCount - 1;
                    line.SetPosition(index, transform.position);
                });
            yield return t.WaitForCompletion();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Coin")) {
            //collect coin
            OnCoinCollected.Invoke();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Spike")) {
            //game over
            transform.DOKill();
            StopAllCoroutines();
            MakeAgonyAnimation();
        }
    }

    private void MakeAgonyAnimation() {
        sr.DOFade(0, 0.1f).SetLoops(5).OnComplete(delegate {
            OnPlayerDead?.Invoke();
            Destroy(gameObject);
        });
    }
}