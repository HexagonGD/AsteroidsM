using UnityEngine;
using UnityEngine.UI;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] AsteroidsSystem.Config _asteroidSystemConfig;
    [SerializeField] ShipSystem.Config _shipSystemConfig;
    [SerializeField] UFOSystem.Config _ufoSystemConfig;

    [Header("UI")]
    [SerializeField] private ScoreUI _gameScoreUI;
    [SerializeField] private ScoreUI _finalScoreUI;
    [SerializeField] private ShipInfoUI _shipInfoUI;
    [SerializeField] private Button _startGameButton;

    private AsteroidsSystem _asteroidSystem;
    private ShipSystem _shipSystem;
    private UFOSystem _ufoSystem;

    private int _score = 0;
    private bool _isGameRun = true;

    private void Awake()
    {
        var playZone = new PlayZone(_camera);
        var spawnPosition = new SpawnOutsideGameZone();

        _asteroidSystem = new AsteroidsSystem(_asteroidSystemConfig, playZone, spawnPosition);
        _shipSystem = new ShipSystem(_shipSystemConfig, playZone);
        _ufoSystem = new UFOSystem(_ufoSystemConfig, _shipSystem.ShipUnit, playZone, spawnPosition);

        _shipSystem.ShipDeadEvent += OnShipDead;
        _startGameButton.onClick.AddListener(RunGame);

        RunGame();
    }

    private void Update()
    {
        if (_isGameRun)
        {
            _asteroidSystem.Update(Time.deltaTime);
            _shipSystem.Update(Time.deltaTime);
            _ufoSystem.Update(Time.deltaTime);

            _shipInfoUI.UpdateInfo(_shipSystem);
        }
    }

    private void RunGame()
    {
        _gameScoreUI.UpdateScore(0);
        _shipInfoUI.gameObject.SetActive(true);
        _finalScoreUI.gameObject.SetActive(false);

        _asteroidSystem.Clear();
        _ufoSystem.Clear();
        _shipSystem.Clear();

        _shipSystem.ShipDeadEvent += OnShipDead;
        _asteroidSystem.SmallAsteroidDeadEvent += UpdateScore;
        _asteroidSystem.BigAsteroidDeadEvent += UpdateScore;
        _ufoSystem.UFODeadEvent += UpdateScore;
        
        _isGameRun = true;
    }

    private void UpdateScore()
    {
        _gameScoreUI.UpdateScore(++_score);
    }

    private void OnShipDead()
    {
        _shipInfoUI.gameObject.SetActive(false);
        _finalScoreUI.gameObject.SetActive(true);
        _finalScoreUI.UpdateScore(_score);

        _shipSystem.ShipDeadEvent -= OnShipDead;
        _asteroidSystem.SmallAsteroidDeadEvent -= UpdateScore;
        _asteroidSystem.BigAsteroidDeadEvent -= UpdateScore;
        _ufoSystem.UFODeadEvent -= UpdateScore;

        _score = 0;

        _isGameRun = false;
    }
}