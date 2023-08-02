using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGB.World
{
    /// <summary>
    /// https://community.monogame.net/t/extended-tileset-collision-data/16072/2
    /// </summary>
    public static class TiledMapExtensions
    {
        public static List<ICollisionActor> GetCollisionActors(this TiledMap map)
        {
            var tiledefs = new List<(TiledMapTilesetTile, int)>();
            var results = new List<ICollisionActor>();
            var globalCount = 1;

            foreach (var tileset in map.Tilesets)
            {
                foreach (var tile in tileset.Tiles)
                {
                    tiledefs.Add((tile, globalCount));
                }

                globalCount += tileset.TileCount;
            }

            foreach (var layer in map.TileLayers)
            {
                foreach(var tile in layer.Tiles)
                {
                    var tiledef = tiledefs
                        .Where(x => x.Item1.LocalTileIdentifier + x.Item2 == tile.GlobalIdentifier)
                        .ToList();

                    foreach (var def in tiledef)
                    {
                        foreach (var obj in def.Item1.Objects)
                        {
                            switch(obj)
                            {
                                case TiledMapRectangleObject rectangle:
                                    var rectangleCollider = new MapCollider(
                                        new RectangleF(
                                            tile.X * layer.TileWidth + rectangle.Position.X,
                                            tile.Y * layer.TileHeight + rectangle.Position.Y,
                                            rectangle.Size.Width,
                                            rectangle.Size.Height));
                                    results.Add(rectangleCollider);
                                    break;

                                case TiledMapEllipseObject ellipse:
                                    var circleCollider = new MapCollider(
                                        new CircleF(new Point2(
                                            tile.X * layer.TileWidth + ellipse.Center.X,
                                            tile.Y * layer.TileWidth + ellipse.Center.Y),
                                            ellipse.Radius.X));
                                    results.Add(circleCollider);
                                    break;
                                //case TiledMapPolygonObject polygon:
                                //    results.Add(new CollisionPolygon(polygon.Points));
                                //    break;
                                //case TiledMapPolylineObject polyline:
                                //    results.Add(new CollisionLine(polyline.Points[0], polyline.Points[1]));
                                //    break;
                                default:
                                    results.Add(new MapCollider(
                                        new RectangleF(obj.Position, obj.Size)));
                                    break;
                            }
                        }
                    }
                }
            }

            return results;
        }
    }
}
