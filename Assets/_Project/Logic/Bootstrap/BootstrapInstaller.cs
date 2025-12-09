using UnityEngine;
using Zenject;

public class BootstrapInstaller : MonoInstaller
{
    [SerializeField] private EntryPoint _entryPoint;

    public override void InstallBindings()
    {
        Container.BindInstance<EntryPoint>(_entryPoint).AsSingle();
        Container.QueueForInject(_entryPoint);
    }
}