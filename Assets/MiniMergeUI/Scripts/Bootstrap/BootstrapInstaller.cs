using MiniMergeUI.Services;
using MiniMergeUI.Services.Factories;
using MiniMergeUI.View;
using UnityEngine;
using Zenject;

namespace MiniMergeUI.Bootstrap
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _gameCanvasPrefab;
        [SerializeField] private GameObject _dragControllerPrefab;
        [SerializeField] private GameObject _chipPrefab;


        public override void InstallBindings()
        {
            Container.Bind<BoardState>()
                .AsSingle();

            Container.Bind<ChipPlacer>()
                .AsSingle();

            Container.Bind<ChipRegistry>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<GameCanvas>()
                .FromComponentInNewPrefab(_gameCanvasPrefab)
            .AsSingle();

            Container.BindInterfacesAndSelfTo<DragHandler>()
                .FromComponentInNewPrefab(_dragControllerPrefab)
                .AsSingle();


            Container.BindInterfacesAndSelfTo<ChipFactory>()
                .AsSingle()
                .WithArguments(_chipPrefab)
                .NonLazy();

            Container.BindInterfacesAndSelfTo<SpawnConductor>()
                .AsSingle()
                .NonLazy();

            Container.Bind<CellsConductor>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<ChipsConductor>()
                 .AsSingle()
                 .NonLazy();
        }
    }
}