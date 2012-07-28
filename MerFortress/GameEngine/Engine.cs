using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MerFortress.GameEngine
{
    public class Engine : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Model world;
        private Vector3 Position = Vector3.One;
        private float Zoom = 2500;
        private float RotationY = 0.0f;
        private float RotationX = 0.0f;
        private Matrix gameWorldRotation;

        public Engine()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }
        protected override void UnloadContent()
        {
        }
        private void UpdateControls()
        {
            KeyboardState kState = Keyboard.GetState();
            MouseState mState = Mouse.GetState();

            if (kState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            #region WASD Keys -- Movement
            if (kState.IsKeyDown(Keys.W))
            {
                Position.Y += 0.1f;
            }
            if (kState.IsKeyDown(Keys.A))
            {
                Position.X += -0.1f;
            }
            if (kState.IsKeyDown(Keys.S))
            {
                Position.Y += -0.1f;
            }
            if (kState.IsKeyDown(Keys.D))
            {
                Position.X += 0.1f;
            }
            #endregion
            #region Mouse Handling -- Aiming/Shooting/Clicking
            RotationX += mState.X;
            RotationY += mState.Y;

            if (mState.XButton1 == ButtonState.Pressed)
            {
                //Shoooooooooot
                //Menu clicks?
            }
            if (mState.XButton2 == ButtonState.Pressed)
            {
                //Do the zooman'
                //Aim down sight
                //Help for menu options
            }
            #endregion
    
            gameWorldRotation =
                Matrix.CreateRotationX(MathHelper.ToRadians(RotationX)) *
                Matrix.CreateRotationY(MathHelper.ToRadians(RotationY));
        }
        protected override void Update(GameTime gameTime)
        {
            UpdateControls();
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            drawWorld("debugMap");

            base.Draw(gameTime);
        }

        private void drawWorld(string worldName)
        {
            if (world != null)
            {
                return;
            }
            world = Content.Load<Model>(worldName + ".FBX");

            drawModel(world);
        }
        private void drawModel(Model m)
        {
            Matrix[] transforms = new Matrix[m.Bones.Count];
            float aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
            m.CopyAbsoluteBoneTransformsTo(transforms);
            Matrix projection =
                Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f),
                    aspectRatio, 1.0f, 10000.0f);
                Matrix view = Matrix.CreateLookAt(new Vector3(0.0f, 50.0f, Zoom),
                Vector3.Zero, Vector3.Up);

            foreach (ModelMesh mesh in m.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.View = view;
                    effect.Projection = projection;
                    effect.World = gameWorldRotation *
                        transforms[mesh.ParentBone.Index] *
                        Matrix.CreateTranslation(Position);
                }
                mesh.Draw();
            }
        }
    }
}
