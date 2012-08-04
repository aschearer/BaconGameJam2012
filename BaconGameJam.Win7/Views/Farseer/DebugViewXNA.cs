using System;
using System.Collections.Generic;
using System.Linq;
using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BaconGameJam.Win7.Views.Farseer
{
    /// <summary>
    /// A debug view that works in XNA.
    /// A debug view shows you what happens inside the physics engine. You can view
    /// bodies, joints, fixtures and more.
    /// </summary>
    public class DebugViewXNA : DebugView, IDisposable
    {
        //Drawing
        private PrimitiveBatch _primitiveBatch;
        private SpriteBatch _batch;
        private GraphicsDevice _device;
        private Vector2[] _tempVertices = new Vector2[Settings.MaxPolygonVertices];
        private List<StringData> _stringData;

        private Matrix _localProjection;
        private Matrix _localView;

        //Shapes
        public Color DefaultShapeColor = new Color(0.9f, 0.7f, 0.7f);
        public Color InactiveShapeColor = new Color(0.5f, 0.5f, 0.3f);
        public Color KinematicShapeColor = new Color(0.5f, 0.5f, 0.9f);
        public Color SleepingShapeColor = new Color(0.6f, 0.6f, 0.6f);
        public Color StaticShapeColor = new Color(0.5f, 0.9f, 0.5f);
        public Color TextColor = Color.White;

        //Contacts
        private int _pointCount;
        private const int MaxContactPoints = 2048;
        private ContactPoint[] _points = new ContactPoint[MaxContactPoints];

        //Debug panel
#if XBOX
        public Vector2 DebugPanelPosition = new Vector2(55, 100);
#else
        public Vector2 DebugPanelPosition = new Vector2(40, 100);
#endif
        private int _max;
        private int _avg;
        private int _min;

        //Performance graph
        public bool AdaptiveLimits = true;
        public int ValuesToGraph = 500;
        public int MinimumValue;
        public int MaximumValue = 1000;
        private List<float> _graphValues = new List<float>();

#if XBOX
        public Rectangle PerformancePanelBounds = new Rectangle(265, 100, 200, 100);
#else
        public Rectangle PerformancePanelBounds = new Rectangle(250, 100, 200, 100);
#endif
        private Vector2[] _background = new Vector2[4];
        public bool Enabled = true;

#if XBOX || WINDOWS_PHONE
        public const int CircleSegments = 16;
#else
        public const int CircleSegments = 32;
#endif

        public DebugViewXNA(World world)
            : base(world)
        {
            world.ContactManager.PreSolve += this.PreSolve;

            //Default flags
            this.AppendFlags(DebugViewFlags.Shape);
            this.AppendFlags(DebugViewFlags.Controllers);
            this.AppendFlags(DebugViewFlags.Joint);
        }

        public void BeginCustomDraw(ref Matrix projection, ref Matrix view)
        {
            this._primitiveBatch.Begin(ref projection, ref view);
        }

        public void EndCustomDraw()
        {
            this._primitiveBatch.End();
        }

        #region IDisposable Members

        public void Dispose()
        {
            this.World.ContactManager.PreSolve -= this.PreSolve;
        }

        #endregion

        private void PreSolve(Contact contact, ref Manifold oldManifold)
        {
            if ((this.Flags & DebugViewFlags.ContactPoints) == DebugViewFlags.ContactPoints)
            {
                Manifold manifold = contact.Manifold;

                if (manifold.PointCount == 0)
                {
                    return;
                }

                Fixture fixtureA = contact.FixtureA;

                FixedArray2<PointState> state1, state2;
                FarseerPhysics.Collision.Collision.GetPointStates(out state1, out state2, ref oldManifold, ref manifold);

                FixedArray2<Vector2> points;
                Vector2 normal;
                contact.GetWorldManifold(out normal, out points);

                for (int i = 0; i < manifold.PointCount && this._pointCount < MaxContactPoints; ++i)
                {
                    if (fixtureA == null)
                    {
                        this._points[i] = new ContactPoint();
                    }
                    ContactPoint cp = this._points[this._pointCount];
                    cp.Position = points[i];
                    cp.Normal = normal;
                    cp.State = state2[i];
                    this._points[this._pointCount] = cp;
                    ++this._pointCount;
                }
            }
        }

        /// <summary>
        /// Call this to draw shapes and other debug draw data.
        /// </summary>
        private void DrawDebugData()
        {
            if ((this.Flags & DebugViewFlags.Shape) == DebugViewFlags.Shape)
            {
                foreach (Body b in this.World.BodyList)
                {
                    Transform xf;
                    b.GetTransform(out xf);
                    foreach (Fixture f in b.FixtureList)
                    {
                        if (b.Enabled == false)
                        {
                            this.DrawShape(f, xf, this.InactiveShapeColor);
                        }
                        else if (b.BodyType == BodyType.Static)
                        {
                            this.DrawShape(f, xf, this.StaticShapeColor);
                        }
                        else if (b.BodyType == BodyType.Kinematic)
                        {
                            this.DrawShape(f, xf, this.KinematicShapeColor);
                        }
                        else if (b.Awake == false)
                        {
                            this.DrawShape(f, xf, this.SleepingShapeColor);
                        }
                        else
                        {
                            this.DrawShape(f, xf, this.DefaultShapeColor);
                        }
                    }
                }
            }
            if ((this.Flags & DebugViewFlags.ContactPoints) == DebugViewFlags.ContactPoints)
            {
                const float axisScale = 0.3f;

                for (int i = 0; i < this._pointCount; ++i)
                {
                    ContactPoint point = this._points[i];

                    if (point.State == PointState.Add)
                    {
                        // Add
                        this.DrawPoint(point.Position, 0.1f, new Color(0.3f, 0.95f, 0.3f));
                    }
                    else if (point.State == PointState.Persist)
                    {
                        // Persist
                        this.DrawPoint(point.Position, 0.1f, new Color(0.3f, 0.3f, 0.95f));
                    }

                    if ((this.Flags & DebugViewFlags.ContactNormals) == DebugViewFlags.ContactNormals)
                    {
                        Vector2 p1 = point.Position;
                        Vector2 p2 = p1 + axisScale * point.Normal;
                        this.DrawSegment(p1, p2, new Color(0.4f, 0.9f, 0.4f));
                    }
                }
                this._pointCount = 0;
            }
            if ((this.Flags & DebugViewFlags.PolygonPoints) == DebugViewFlags.PolygonPoints)
            {
                foreach (Body body in this.World.BodyList)
                {
                    foreach (Fixture f in body.FixtureList)
                    {
                        PolygonShape polygon = f.Shape as PolygonShape;
                        if (polygon != null)
                        {
                            Transform xf;
                            body.GetTransform(out xf);

                            for (int i = 0; i < polygon.Vertices.Count; i++)
                            {
                                Vector2 tmp = MathUtils.Multiply(ref xf, polygon.Vertices[i]);
                                this.DrawPoint(tmp, 0.1f, Color.Red);
                            }
                        }
                    }
                }
            }
            if ((this.Flags & DebugViewFlags.Joint) == DebugViewFlags.Joint)
            {
                foreach (Joint j in this.World.JointList)
                {
                    this.DrawJoint(j);
                }
            }
            if ((this.Flags & DebugViewFlags.Pair) == DebugViewFlags.Pair)
            {
                Color color = new Color(0.3f, 0.9f, 0.9f);
                for (int i = 0; i < this.World.ContactManager.ContactList.Count; i++)
                {
                    Contact c = this.World.ContactManager.ContactList[i];
                    Fixture fixtureA = c.FixtureA;
                    Fixture fixtureB = c.FixtureB;

                    AABB aabbA;
                    fixtureA.GetAABB(out aabbA, 0);
                    AABB aabbB;
                    fixtureB.GetAABB(out aabbB, 0);

                    Vector2 cA = aabbA.Center;
                    Vector2 cB = aabbB.Center;

                    this.DrawSegment(cA, cB, color);
                }
            }
            if ((this.Flags & DebugViewFlags.AABB) == DebugViewFlags.AABB)
            {
                Color color = new Color(0.9f, 0.3f, 0.9f);
                IBroadPhase bp = this.World.ContactManager.BroadPhase;

                foreach (Body b in this.World.BodyList)
                {
                    if (b.Enabled == false)
                    {
                        continue;
                    }

                    foreach (Fixture f in b.FixtureList)
                    {
                        for (int t = 0; t < f.ProxyCount; ++t)
                        {
                            FixtureProxy proxy = f.Proxies[t];
                            AABB aabb;
                            bp.GetFatAABB(proxy.ProxyId, out aabb);

                            this.DrawAABB(ref aabb, color);
                        }
                    }
                }
            }
            if ((this.Flags & DebugViewFlags.CenterOfMass) == DebugViewFlags.CenterOfMass)
            {
                foreach (Body b in this.World.BodyList)
                {
                    Transform xf;
                    b.GetTransform(out xf);
                    xf.Position = b.WorldCenter;
                    this.DrawTransform(ref xf);
                }
            }
            if ((this.Flags & DebugViewFlags.Controllers) == DebugViewFlags.Controllers)
            {
                for (int i = 0; i < this.World.ControllerList.Count; i++)
                {
                    Controller controller = this.World.ControllerList[i];

                    BuoyancyController buoyancy = controller as BuoyancyController;
                    if (buoyancy != null)
                    {
                        AABB container = buoyancy.Container;
                        this.DrawAABB(ref container, Color.LightBlue);
                    }
                }
            }
            if ((this.Flags & DebugViewFlags.DebugPanel) == DebugViewFlags.DebugPanel)
            {
                this.DrawDebugPanel();
            }
        }

        private void DrawPerformanceGraph()
        {
            this._graphValues.Add(this.World.UpdateTime);

            if (this._graphValues.Count > this.ValuesToGraph + 1)
                this._graphValues.RemoveAt(0);

            float x = this.PerformancePanelBounds.X;
            float deltaX = this.PerformancePanelBounds.Width / (float)this.ValuesToGraph;
            float yScale = this.PerformancePanelBounds.Bottom - (float)this.PerformancePanelBounds.Top;

            // we must have at least 2 values to start rendering
            if (this._graphValues.Count > 2)
            {
                this._max = (int)this._graphValues.Max();
                this._avg = (int)this._graphValues.Average();
                this._min = (int)this._graphValues.Min();

                if (this.AdaptiveLimits)
                {
                    this.MaximumValue = this._max;
                    this.MinimumValue = 0;
                }

                // start at last value (newest value added)
                // continue until no values are left
                for (int i = this._graphValues.Count - 1; i > 0; i--)
                {
                    float y1 = this.PerformancePanelBounds.Bottom -
                               ((this._graphValues[i] / (this.MaximumValue - this.MinimumValue)) * yScale);
                    float y2 = this.PerformancePanelBounds.Bottom -
                               ((this._graphValues[i - 1] / (this.MaximumValue - this.MinimumValue)) * yScale);

                    Vector2 x1 =
                        new Vector2(MathHelper.Clamp(x, this.PerformancePanelBounds.Left, this.PerformancePanelBounds.Right),
                                    MathHelper.Clamp(y1, this.PerformancePanelBounds.Top, this.PerformancePanelBounds.Bottom));

                    Vector2 x2 =
                        new Vector2(
                            MathHelper.Clamp(x + deltaX, this.PerformancePanelBounds.Left, this.PerformancePanelBounds.Right),
                            MathHelper.Clamp(y2, this.PerformancePanelBounds.Top, this.PerformancePanelBounds.Bottom));

                    this.DrawSegment(x1, x2, Color.LightGreen);

                    x += deltaX;
                }
            }

            this.DrawString(this.PerformancePanelBounds.Right + 10, this.PerformancePanelBounds.Top, "Max: " + this._max);
            this.DrawString(this.PerformancePanelBounds.Right + 10, this.PerformancePanelBounds.Center.Y - 7, "Avg: " + this._avg);
            this.DrawString(this.PerformancePanelBounds.Right + 10, this.PerformancePanelBounds.Bottom - 15, "Min: " + this._min);

            //Draw background.
            this._background[0] = new Vector2(this.PerformancePanelBounds.X, this.PerformancePanelBounds.Y);
            this._background[1] = new Vector2(this.PerformancePanelBounds.X,
                                         this.PerformancePanelBounds.Y + this.PerformancePanelBounds.Height);
            this._background[2] = new Vector2(this.PerformancePanelBounds.X + this.PerformancePanelBounds.Width,
                                         this.PerformancePanelBounds.Y + this.PerformancePanelBounds.Height);
            this._background[3] = new Vector2(this.PerformancePanelBounds.X + this.PerformancePanelBounds.Width,
                                         this.PerformancePanelBounds.Y);

            this.DrawSolidPolygon(this._background, 4, Color.DarkGray, true);
        }

        private void DrawDebugPanel()
        {
            int fixtures = 0;
            for (int i = 0; i < this.World.BodyList.Count; i++)
            {
                fixtures += this.World.BodyList[i].FixtureList.Count;
            }

            int x = (int)this.DebugPanelPosition.X;
            int y = (int)this.DebugPanelPosition.Y;

            this.DrawString(x, y, "Objects:" +
                             "\n- Bodies: " + this.World.BodyList.Count +
                             "\n- Fixtures: " + fixtures +
                             "\n- Contacts: " + this.World.ContactList.Count +
                             "\n- Joints: " + this.World.JointList.Count +
                             "\n- Controllers: " + this.World.ControllerList.Count +
                             "\n- Proxies: " + this.World.ProxyCount);

            this.DrawString(x + 110, y, "Update time:" +
                                   "\n- Body: " + this.World.SolveUpdateTime +
                                   "\n- Contact: " + this.World.ContactsUpdateTime +
                                   "\n- CCD: " + this.World.ContinuousPhysicsTime +
                                   "\n- Joint: " + this.World.Island.JointUpdateTime +
                                   "\n- Controller: " + this.World.ControllersUpdateTime +
                                   "\n- Total: " + this.World.UpdateTime);
        }

        public void DrawAABB(ref AABB aabb, Color color)
        {
            Vector2[] verts = new Vector2[4];
            verts[0] = new Vector2(aabb.LowerBound.X, aabb.LowerBound.Y);
            verts[1] = new Vector2(aabb.UpperBound.X, aabb.LowerBound.Y);
            verts[2] = new Vector2(aabb.UpperBound.X, aabb.UpperBound.Y);
            verts[3] = new Vector2(aabb.LowerBound.X, aabb.UpperBound.Y);

            this.DrawPolygon(verts, 4, color);
        }

        private void DrawJoint(Joint joint)
        {
            if (!joint.Enabled)
                return;

            Body b1 = joint.BodyA;
            Body b2 = joint.BodyB;
            Transform xf1, xf2;
            b1.GetTransform(out xf1);

            Vector2 x2 = Vector2.Zero;

            // WIP David
            if (!joint.IsFixedType())
            {
                b2.GetTransform(out xf2);
                x2 = xf2.Position;
            }

            Vector2 p1 = joint.WorldAnchorA;
            Vector2 p2 = joint.WorldAnchorB;
            Vector2 x1 = xf1.Position;

            Color color = new Color(0.5f, 0.8f, 0.8f);

            switch (joint.JointType)
            {
                case JointType.Distance:
                    this.DrawSegment(p1, p2, color);
                    break;
                case JointType.Pulley:
                    PulleyJoint pulley = (PulleyJoint)joint;
                    Vector2 s1 = pulley.GroundAnchorA;
                    Vector2 s2 = pulley.GroundAnchorB;
                    this.DrawSegment(s1, p1, color);
                    this.DrawSegment(s2, p2, color);
                    this.DrawSegment(s1, s2, color);
                    break;
                case JointType.FixedMouse:
                    this.DrawPoint(p1, 0.5f, new Color(0.0f, 1.0f, 0.0f));
                    this.DrawSegment(p1, p2, new Color(0.8f, 0.8f, 0.8f));
                    break;
                case JointType.Revolute:
                    //DrawSegment(x2, p1, color);
                    this.DrawSegment(p2, p1, color);
                    this.DrawSolidCircle(p2, 0.1f, Vector2.Zero, Color.Red);
                    this.DrawSolidCircle(p1, 0.1f, Vector2.Zero, Color.Blue);
                    break;
                case JointType.FixedAngle:
                    //Should not draw anything.
                    break;
                case JointType.FixedRevolute:
                    this.DrawSegment(x1, p1, color);
                    this.DrawSolidCircle(p1, 0.1f, Vector2.Zero, Color.Pink);
                    break;
                case JointType.FixedLine:
                    this.DrawSegment(x1, p1, color);
                    this.DrawSegment(p1, p2, color);
                    break;
                case JointType.FixedDistance:
                    this.DrawSegment(x1, p1, color);
                    this.DrawSegment(p1, p2, color);
                    break;
                case JointType.FixedPrismatic:
                    this.DrawSegment(x1, p1, color);
                    this.DrawSegment(p1, p2, color);
                    break;
                case JointType.Gear:
                    this.DrawSegment(x1, x2, color);
                    break;
                //case JointType.Weld:
                //    break;
                default:
                    this.DrawSegment(x1, p1, color);
                    this.DrawSegment(p1, p2, color);
                    this.DrawSegment(x2, p2, color);
                    break;
            }
        }

        public void DrawShape(Fixture fixture, Transform xf, Color color)
        {
            switch (fixture.ShapeType)
            {
                case ShapeType.Circle:
                    {
                        CircleShape circle = (CircleShape)fixture.Shape;

                        Vector2 center = MathUtils.Multiply(ref xf, circle.Position);
                        float radius = circle.Radius;
                        Vector2 axis = xf.R.Col1;

                        this.DrawSolidCircle(center, radius, axis, color);
                    }
                    break;

                case ShapeType.Polygon:
                    {
                        PolygonShape poly = (PolygonShape)fixture.Shape;
                        int vertexCount = poly.Vertices.Count;
                        System.Diagnostics.Debug.Assert(vertexCount <= Settings.MaxPolygonVertices);

                        for (int i = 0; i < vertexCount; ++i)
                        {
                            this._tempVertices[i] = MathUtils.Multiply(ref xf, poly.Vertices[i]);
                        }

                        this.DrawSolidPolygon(this._tempVertices, vertexCount, color);
                    }
                    break;


                case ShapeType.Edge:
                    {
                        EdgeShape edge = (EdgeShape)fixture.Shape;
                        Vector2 v1 = MathUtils.Multiply(ref xf, edge.Vertex1);
                        Vector2 v2 = MathUtils.Multiply(ref xf, edge.Vertex2);
                        this.DrawSegment(v1, v2, color);
                    }
                    break;

                case ShapeType.Loop:
                    {
                        LoopShape loop = (LoopShape)fixture.Shape;
                        int count = loop.Vertices.Count;

                        Vector2 v1 = MathUtils.Multiply(ref xf, loop.Vertices[count - 1]);
                        this.DrawCircle(v1, 0.05f, color);
                        for (int i = 0; i < count; ++i)
                        {
                            Vector2 v2 = MathUtils.Multiply(ref xf, loop.Vertices[i]);
                            this.DrawSegment(v1, v2, color);
                            v1 = v2;
                        }
                    }
                    break;
            }
        }

        public override void DrawPolygon(Vector2[] vertices, int count, float red, float green, float blue)
        {
            this.DrawPolygon(vertices, count, new Color(red, green, blue));
        }

        public void DrawPolygon(Vector2[] vertices, int count, Color color)
        {
            if (!this._primitiveBatch.IsReady())
            {
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");
            }
            for (int i = 0; i < count - 1; i++)
            {
                this._primitiveBatch.AddVertex(vertices[i], color, PrimitiveType.LineList);
                this._primitiveBatch.AddVertex(vertices[i + 1], color, PrimitiveType.LineList);
            }

            this._primitiveBatch.AddVertex(vertices[count - 1], color, PrimitiveType.LineList);
            this._primitiveBatch.AddVertex(vertices[0], color, PrimitiveType.LineList);
        }

        public override void DrawSolidPolygon(Vector2[] vertices, int count, float red, float green, float blue)
        {
            this.DrawSolidPolygon(vertices, count, new Color(red, green, blue), true);
        }

        public void DrawSolidPolygon(Vector2[] vertices, int count, Color color)
        {
            this.DrawSolidPolygon(vertices, count, color, true);
        }

        public void DrawSolidPolygon(Vector2[] vertices, int count, Color color, bool outline)
        {
            if (!this._primitiveBatch.IsReady())
            {
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");
            }
            if (count == 2)
            {
                this.DrawPolygon(vertices, count, color);
                return;
            }

            Color colorFill = color * (outline ? 0.5f : 1.0f);

            for (int i = 1; i < count - 1; i++)
            {
                this._primitiveBatch.AddVertex(vertices[0], colorFill, PrimitiveType.TriangleList);
                this._primitiveBatch.AddVertex(vertices[i], colorFill, PrimitiveType.TriangleList);
                this._primitiveBatch.AddVertex(vertices[i + 1], colorFill, PrimitiveType.TriangleList);
            }

            if (outline)
            {
                this.DrawPolygon(vertices, count, color);
            }
        }

        public override void DrawCircle(Vector2 center, float radius, float red, float green, float blue)
        {
            this.DrawCircle(center, radius, new Color(red, green, blue));
        }

        public void DrawCircle(Vector2 center, float radius, Color color)
        {
            if (!this._primitiveBatch.IsReady())
            {
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");
            }
            const double increment = Math.PI * 2.0 / CircleSegments;
            double theta = 0.0;

            for (int i = 0; i < CircleSegments; i++)
            {
                Vector2 v1 = center + radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
                Vector2 v2 = center +
                             radius *
                             new Vector2((float)Math.Cos(theta + increment), (float)Math.Sin(theta + increment));

                this._primitiveBatch.AddVertex(v1, color, PrimitiveType.LineList);
                this._primitiveBatch.AddVertex(v2, color, PrimitiveType.LineList);

                theta += increment;
            }
        }

        public override void DrawSolidCircle(Vector2 center, float radius, Vector2 axis, float red, float green,
                                             float blue)
        {
            this.DrawSolidCircle(center, radius, axis, new Color(red, green, blue));
        }

        public void DrawSolidCircle(Vector2 center, float radius, Vector2 axis, Color color)
        {
            if (!this._primitiveBatch.IsReady())
            {
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");
            }
            const double increment = Math.PI * 2.0 / CircleSegments;
            double theta = 0.0;

            Color colorFill = color * 0.5f;

            Vector2 v0 = center + radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
            theta += increment;

            for (int i = 1; i < CircleSegments - 1; i++)
            {
                Vector2 v1 = center + radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
                Vector2 v2 = center +
                             radius *
                             new Vector2((float)Math.Cos(theta + increment), (float)Math.Sin(theta + increment));

                this._primitiveBatch.AddVertex(v0, colorFill, PrimitiveType.TriangleList);
                this._primitiveBatch.AddVertex(v1, colorFill, PrimitiveType.TriangleList);
                this._primitiveBatch.AddVertex(v2, colorFill, PrimitiveType.TriangleList);

                theta += increment;
            }
            this.DrawCircle(center, radius, color);

            this.DrawSegment(center, center + axis * radius, color);
        }

        public override void DrawSegment(Vector2 start, Vector2 end, float red, float green, float blue)
        {
            this.DrawSegment(start, end, new Color(red, green, blue));
        }

        public void DrawSegment(Vector2 start, Vector2 end, Color color)
        {
            if (!this._primitiveBatch.IsReady())
            {
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");
            }
            this._primitiveBatch.AddVertex(start, color, PrimitiveType.LineList);
            this._primitiveBatch.AddVertex(end, color, PrimitiveType.LineList);
        }

        public override void DrawTransform(ref Transform transform)
        {
            const float axisScale = 0.4f;
            Vector2 p1 = transform.Position;

            Vector2 p2 = p1 + axisScale * transform.R.Col1;
            this.DrawSegment(p1, p2, Color.Red);

            p2 = p1 + axisScale * transform.R.Col2;
            this.DrawSegment(p1, p2, Color.Green);
        }

        public void DrawPoint(Vector2 p, float size, Color color)
        {
            Vector2[] verts = new Vector2[4];
            float hs = size / 2.0f;
            verts[0] = p + new Vector2(-hs, -hs);
            verts[1] = p + new Vector2(hs, -hs);
            verts[2] = p + new Vector2(hs, hs);
            verts[3] = p + new Vector2(-hs, hs);

            this.DrawSolidPolygon(verts, 4, color, true);
        }

        public void DrawString(int x, int y, string s, params object[] args)
        {
            this._stringData.Add(new StringData(x, y, s, args, this.TextColor));
        }

        public void DrawArrow(Vector2 start, Vector2 end, float length, float width, bool drawStartIndicator,
                              Color color)
        {
            // Draw connection segment between start- and end-point
            this.DrawSegment(start, end, color);

            // Precalculate halfwidth
            float halfWidth = width / 2;

            // Create directional reference
            Vector2 rotation = (start - end);
            rotation.Normalize();

            // Calculate angle of directional vector
            float angle = (float)Math.Atan2(rotation.X, -rotation.Y);
            // Create matrix for rotation
            Matrix rotMatrix = Matrix.CreateRotationZ(angle);
            // Create translation matrix for end-point
            Matrix endMatrix = Matrix.CreateTranslation(end.X, end.Y, 0);

            // Setup arrow end shape
            Vector2[] verts = new Vector2[3];
            verts[0] = new Vector2(0, 0);
            verts[1] = new Vector2(-halfWidth, -length);
            verts[2] = new Vector2(halfWidth, -length);

            // Rotate end shape
            Vector2.Transform(verts, ref rotMatrix, verts);
            // Translate end shape
            Vector2.Transform(verts, ref endMatrix, verts);

            // Draw arrow end shape
            this.DrawSolidPolygon(verts, 3, color, false);

            if (drawStartIndicator)
            {
                // Create translation matrix for start
                Matrix startMatrix = Matrix.CreateTranslation(start.X, start.Y, 0);
                // Setup arrow start shape
                Vector2[] baseVerts = new Vector2[4];
                baseVerts[0] = new Vector2(-halfWidth, length / 4);
                baseVerts[1] = new Vector2(halfWidth, length / 4);
                baseVerts[2] = new Vector2(halfWidth, 0);
                baseVerts[3] = new Vector2(-halfWidth, 0);

                // Rotate start shape
                Vector2.Transform(baseVerts, ref rotMatrix, baseVerts);
                // Translate start shape
                Vector2.Transform(baseVerts, ref startMatrix, baseVerts);
                // Draw start shape
                this.DrawSolidPolygon(baseVerts, 4, color, false);
            }
        }

        public void RenderDebugData(ref Matrix projection, ref Matrix view)
        {
            if (!this.Enabled)
            {
                return;
            }

            //Nothing is enabled - don't draw the debug view.
            if (this.Flags == 0)
                return;

            this._device.RasterizerState = RasterizerState.CullNone;
            this._device.DepthStencilState = DepthStencilState.Default;

            this._primitiveBatch.Begin(ref projection, ref view);
            this.DrawDebugData();
            this._primitiveBatch.End();

            if ((this.Flags & DebugViewFlags.PerformanceGraph) == DebugViewFlags.PerformanceGraph)
            {
                this._primitiveBatch.Begin(ref this._localProjection, ref this._localView);
                this.DrawPerformanceGraph();
                this._primitiveBatch.End();
            }

            // begin the sprite batch effect
            this._batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            // end the sprite batch effect
            this._batch.End();

            this._stringData.Clear();
        }

        public void RenderDebugData(ref Matrix projection)
        {
            if (!this.Enabled)
            {
                return;
            }
            Matrix view = Matrix.Identity;
            this.RenderDebugData(ref projection, ref view);
        }

        public void LoadContent(GraphicsDevice device, ContentManager content)
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            this._device = device;
            this._batch = new SpriteBatch(this._device);
            this._primitiveBatch = new PrimitiveBatch(this._device, 1000);
            this._stringData = new List<StringData>();

            this._localProjection = Matrix.CreateOrthographicOffCenter(0f, this._device.Viewport.Width, this._device.Viewport.Height,
                                                                  0f, 0f, 1f);
            this._localView = Matrix.Identity;
        }

        #region Nested type: ContactPoint

        private struct ContactPoint
        {
            public Vector2 Normal;
            public Vector2 Position;
            public PointState State;
        }

        #endregion

        #region Nested type: StringData

        private struct StringData
        {
            public object[] Args;
            public Color Color;
            public string S;
            public int X, Y;

            public StringData(int x, int y, string s, object[] args, Color color)
            {
                this.X = x;
                this.Y = y;
                this.S = s;
                this.Args = args;
                this.Color = color;
            }
        }

        #endregion
    }
}