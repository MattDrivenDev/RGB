using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Shapes;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using System.Collections.Generic;
using System.Linq;

namespace RGB.World
{
    public class WorldMap
    {
        public const string LayerName_Spawn = "Spawn";
        public const string LayerName_Walls = "Walls";
        public const string LayerName_Base = "Base";

        public WorldMap(Game game, string tmx, OrthographicCamera camera)
        {
            Game = game;
            TiledMap = Game.Content.Load<TiledMap>(tmx);
            BaseLayer = TiledMap.GetLayer<TiledMapTileLayer>(LayerName_Base);
            WallLayer = TiledMap.GetLayer<TiledMapTileLayer>(LayerName_Walls);
            SpawnLayer = TiledMap.GetLayer<TiledMapObjectLayer>(LayerName_Spawn);
            Renderer = new TiledMapRenderer(Game.GraphicsDevice, TiledMap);
            CollisionActors = TiledMap.GetCollisionActors();
            Camera = camera;
        }

        public Game Game { get; init; }
        public TiledMapRenderer Renderer { get; init; }
        public TiledMap TiledMap { get; init; }
        public TiledMapTileLayer BaseLayer { get; init; }
        public TiledMapTileLayer WallLayer { get; init; }
        public TiledMapObjectLayer SpawnLayer { get; init; }
        public OrthographicCamera Camera { get; init; }
        public List<ICollisionActor> CollisionActors { get; init; }

        public Vector2 GetSpawnPoint(string character)
        {
            var spawnPoint = SpawnLayer.Objects.FirstOrDefault(x => x.Name == character);
            return spawnPoint.Position;
        }

        //public IShapeF[] GetCollisionObjects(TiledMapTile tile)
        //{
        //    var tileId = tile.GlobalIdentifier;
        //    var tileset = TiledMap.GetTilesetByTileGlobalIdentifier(tileId);
        //    var t = TiledMap.GetTilesetFirstGlobalIdentifier(tileset);

        //    var tilesetTile = tileset.Tiles.FirstOrDefault(x => x.LocalTileIdentifier + t == tileId);
        //    var objects = tilesetTile.Objects;
        //    var shapes = objects.Select(GetShape).ToArray();
        //    return shapes;
        //}

        public TiledMapTile GetTilesAt(string layer, Vector2 position)
        {
            var tiledMapLayer = TiledMap.GetLayer<TiledMapTileLayer>(layer);
            return GetTilesAt(tiledMapLayer, position);
        }

        public TiledMapTile GetTilesAt(TiledMapTileLayer layer, Vector2 location)
        {
            var mapX = location.X / TiledMap.TileWidth;
            var mapY = location.Y / TiledMap.TileHeight;
            var tile = layer.GetTile((ushort)mapX, (ushort)mapY);
            return tile;
        }

        public virtual void Update(GameTime gameTime)
        {
            Renderer.Update(gameTime);
        }

        public virtual void Draw(GameTime gameTime)
        {
            Renderer.Draw(Camera.GetViewMatrix());
        }
    }
}
