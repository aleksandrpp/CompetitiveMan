using UnityEngine;

namespace AK.CompetitiveController
{
    public class MoveController
    {
        private readonly MoveConfig _config;
        private readonly Capsule _capsule;

        private Vector3 _velocity, _position;
        private Vector2 _delta;
        private Quaternion _rotation;
        private bool _jump, _grounded;

        public MoveController(MoveConfig config, CapsuleCollider capsuleCollider, Vector3 position)
        {
            _config = config;
            _capsule = new Capsule(capsuleCollider, config);
            _position = position;
        }

        public bool Grounded => _grounded;
        public Vector3 Position => _position;
        public Vector3 Velocity => _velocity;

        public void Update(Vector2 delta, Quaternion rotation, bool jump)
        {
            _delta = delta;
            _rotation = rotation;
            _jump = jump;

            (_, _, _grounded) = _capsule.Trace(Vector3.down * .1f, _config.groundLayers);

            CalculateVelocity();

            _position += _velocity * Time.deltaTime;
            _capsule.ResolveCollisions(ref _position, ref _velocity, _config.groundLayers);
        }

        private void CalculateVelocity()
        {
            var forward = _rotation * Vector3.forward;
            var right = _rotation * Vector3.right;

            var direction = new Vector3(
                forward.x * _delta.y + right.x * _delta.x,
                0,
                forward.z * _delta.y + right.z * _delta.x);

            direction.Normalize();

            _velocity.y -= _config.Gravity * Time.deltaTime;

            if (!_grounded)
            {
                _velocity -= 1 / _config.AirFriction * Time.deltaTime * _velocity;
            }
            else
            {
                _velocity = _config.Speed * direction;

                if (_jump)
                    _velocity.y += _config.JumpPower;
            }
        }
    }
}