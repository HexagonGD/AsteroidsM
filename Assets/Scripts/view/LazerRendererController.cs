using System.Collections.Generic;
using UnityEngine;

public class LazerRendererController
{
    private readonly LineRenderer _lineRendererPrefab;
    private readonly LazerRenderer.Config _config;
    private List<LazerRenderer> _renderers = new();

    public LazerRendererController(LineRenderer lineRendererPrefab, LazerRenderer.Config config)
    {
        _lineRendererPrefab = lineRendererPrefab;
        _config = config;
    }

    public void DrawLazer(Vector3 startPosition, Vector3 direction)
    {
        var lineRenderer = GameObject.Instantiate(_lineRendererPrefab);
        LazerRenderer renderer = new LazerRenderer(lineRenderer, _config);
        renderer.EndedDrawEvent += OnEndDraw;
        renderer.Drawlazer(startPosition, direction);
        _renderers.Add(renderer);
    }

    public void Update(float deltaTime)
    {
        for(var i = _renderers.Count - 1; i >= 0; i--)
        {
            _renderers[i].Update(deltaTime);
        }
    }

    private void OnEndDraw(LazerRenderer renderer)
    {
        renderer.EndedDrawEvent -= OnEndDraw;
        _renderers.Remove(renderer);
    }
}