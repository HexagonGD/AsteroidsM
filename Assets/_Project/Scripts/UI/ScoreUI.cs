using TMPro;
using UnityEngine;

namespace Asteroids.UI
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreTMP;

        public void UpdateScore(int score)
        {
            _scoreTMP.text = $"Score: {score}";
        }
    }
}