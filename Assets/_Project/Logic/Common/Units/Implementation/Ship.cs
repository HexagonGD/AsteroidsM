using Asteroids.Logic.Common.DieHandler.Implementation;
using Asteroids.Logic.Common.Movement.Core;
using Asteroids.Logic.Common.Movement.DeadZoneHandler.Implementation;
using Asteroids.Logic.Common.Movement.Implementation;
using Asteroids.Logic.Common.Services.Sounds;
using Asteroids.Logic.Common.Units.Core;
using Asteroids.Logic.Movement.BorderHandler.Implementation;

namespace Asteroids.Logic.Common.Units.Implementation
{
    public class Ship : Unit
    {
        private SoundsService _soundsService;

        public Ship(AccelerationMovement movement, ScreenBorderTeleport borderHandler,
                    IngoreDeadZone deadZoneHandler, WithoutDieEffect dieHandler, SoundsService soundsService,
                    TransformData data = default) :
               base(movement, borderHandler, deadZoneHandler, dieHandler, data)
        {
            _soundsService = soundsService;
        }

        protected override void OnDie()
        {
            _soundsService.PlaySFX(SFXType.DestroyedShip);
        }
    }
}