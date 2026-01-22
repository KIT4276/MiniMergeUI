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
        [SerializeField] private ChipVisualLibrary _visualSO;


        public override void InstallBindings()
        {
            Container.Bind<ChipVisualLibrary>()
                .FromInstance(_visualSO)
                .AsSingle();

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

            Container.Bind<MergeServices>()
                .AsSingle();

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