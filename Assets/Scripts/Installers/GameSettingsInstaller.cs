using System;
using EscapeMaze.Game.Player;
using Zenject;

namespace EscapeMaze.Game {

    public class GameSettingsInstaller : ScriptableObjectInstaller {

        public int totalBots;
        public ShootingBotSettings shootingBotSettings;

        public PlayerSettings playerSettings;

        public override void InstallBindings() { }

        public class Common {
            public int gameTime;
            public int keys;
        }

        [Serializable]
        public class Player {
            public int health;
            public int maxSpeed;
        }

    }
}
