using TicTacToe;
using UnityEngine;
using Zenject;

namespace ZenjectInstallers
{
    public class InputControllerInstaller : MonoInstaller
    {

        [SerializeField] private GameObject _inputControllerGameObject;

        public override void InstallBindings()
        {
            Container.Bind<IGameInputController>()
                .FromNewComponentOn(_inputControllerGameObject);

            Container.Bind<HumanPlayer>().AsSingle();
        }
    } 
}