using UI;

namespace Infrastructure.Services.ObserverScene
{
    public interface ISceneObserver : IService
    {
        void ChangeState(EndGameAction endGameAction);
    }
}
