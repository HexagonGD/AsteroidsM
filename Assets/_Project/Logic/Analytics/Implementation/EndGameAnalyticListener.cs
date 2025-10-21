using Asteroids.Logic.Analytics.Core;
using Asteroids.Logic.Analytics.Implementation.UnitDiedListeners;
using Asteroids.Logic.Analytics.Implementation.WeaponListeners;
using Asteroids.Logic.FSMachine;
using System;
using System.Collections.Generic;

namespace Asteroids.Logic.Analytics.Implementation
{
    public class EndGameAnalyticListener : IAnalyticListener
    {
        public event Action<string, IEnumerable<AnalyticParameter>> OnEventListened;

        private readonly BulletWeaponFireAnalyticListener _bulletFireListener;
        private readonly LazerWeaponFireAnalyticListener _lazerFireListener;
        private readonly UFODiedAnalyticListener _ufoDiedListener;
        private readonly AsteroidDiedAnalyticListener _asteroidDiedListener;
        private readonly FSM _fsm;

        private float _bulletFiredCount = 0;
        private float _lazerFiredCount = 0;
        private float _ufoDiedCount = 0;
        private float _asteroidDiedCount = 0;

        public EndGameAnalyticListener(BulletWeaponFireAnalyticListener bulletFireListener, LazerWeaponFireAnalyticListener lazerFireListener,
                                       UFODiedAnalyticListener ufoDiedListener, AsteroidDiedAnalyticListener asteroidDiedListener, FSM fsm)
        {
            _bulletFireListener = bulletFireListener;
            _lazerFireListener = lazerFireListener;
            _ufoDiedListener = ufoDiedListener;
            _asteroidDiedListener = asteroidDiedListener;
            _fsm = fsm;

            _bulletFireListener.OnEventListened += BulletFiredHandler;
            _lazerFireListener.OnEventListened += LazerFiredLHandler;
            _ufoDiedListener.OnEventListened += UFODiedHandler;
            _asteroidDiedListener.OnEventListened += AsteroidDiedHandler;
            _fsm.OnStateChanged += StateChangedHandler;
        }

        private void BulletFiredHandler(string eventName, IEnumerable<AnalyticParameter> args)
        {
            _bulletFiredCount++;
        }

        private void LazerFiredLHandler(string eventName, IEnumerable<AnalyticParameter> args)
        {
            _lazerFiredCount++;
        }

        private void UFODiedHandler(string eventName, IEnumerable<AnalyticParameter> args)
        {
            _ufoDiedCount++;
        }

        private void AsteroidDiedHandler(string eventName, IEnumerable<AnalyticParameter> args)
        {
            _asteroidDiedCount++;
        }

        private void StateChangedHandler(StateEnum state)
        {
            if (state == StateEnum.Score)
            {
                OnEventListened?.Invoke(EventNames.EndGameEvent,
                    new List<AnalyticParameter>() {
                    new AnalyticParameter("bulletFiredCount", _bulletFiredCount),
                    new AnalyticParameter("lazerFiredCount", _lazerFiredCount),
                    new AnalyticParameter("ufoDiedCount", _ufoDiedCount),
                    new AnalyticParameter("asteroidDiedCount", _asteroidDiedCount)});

                _bulletFiredCount = 0;
                _lazerFiredCount = 0;
                _ufoDiedCount = 0;
                _asteroidDiedCount = 0;
            }
        }
    }
}