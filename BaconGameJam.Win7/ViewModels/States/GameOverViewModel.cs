using System.Collections.ObjectModel;
using BaconGameJam.Common.Models.Doodads;
using BaconGameJam.Common.Models.Levels;
using BaconGameJam.Win7.Views.Input;
using Microsoft.Xna.Framework;

namespace BaconGameJam.Win7.ViewModels.States
{
    public class GameOverViewModel : ViewModelBase
    {
        private readonly LevelFactory levelFactory;
        private readonly Level level;
        private readonly IKeyboardInputManager keyInput;
        private readonly IConductorViewModel conductor;
        bool StartNewGame;

        public GameOverViewModel(
            LevelFactory levelFactory, 
            Level level, 
            IKeyboardInputManager keyInput,
            IConductorViewModel conductor)
        {
            this.levelFactory = levelFactory;
            this.level = level;
            this.keyInput = keyInput;
            this.conductor = conductor;
            this.StartNewGame = false;
        }

        public void NavigateTo()
        {
            this.StartNewGame = false;
            this.keyInput.KeyDown += this.OnKeyDown;
        }

        private void OnKeyDown(object send, KeyboardEventArgs e)
        {
            if ((e.IsStart) && (!this.StartNewGame))
            {
                this.StartNewGame = true;
                this.keyInput.KeyDown -= this.OnKeyDown;
                //this.conductor.Pop();
                this.conductor.Push(typeof(PlayingViewModel));
            }
        }

        public void Update(GameTime gameTime)
        {
            this.level.Update(gameTime);
        }
    }
}