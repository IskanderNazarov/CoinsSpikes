using UnityEngine;

public class Level : MonoBehaviour {
    [SerializeField] private GameObject[] coins;


    public int TotalCoinsCount => coins.Length;
}