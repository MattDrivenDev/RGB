using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace RGB.World
{
    public class MapCollider : ICollisionActor
    {
        public MapCollider(IShapeF bounds)
        {
            Bounds = bounds;
        }

        public IShapeF Bounds { get; init; }

        public void OnCollision(CollisionEventArgs collisionInfo)
        {

        }
    }
}
