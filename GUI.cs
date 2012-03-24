using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using XNAForms.Forms;

namespace XNAForms
{
    /// <summary>
    /// Manages the graphical user interface.
    /// </summary>
    public static class GUI
    {
        private static Cursor cursor;
        internal static Texture2D[] cursorTexs = new Texture2D[Enum.GetValues(typeof(CursorType)).Length];
        internal static List<Form> forms = new List<Form>();
        internal static List<int> formOrder = new List<int>();
        internal static Game game;

        /// <summary>
        /// Adds a form to the GUI.
        /// </summary>
        /// <param name="form">Form to add.</param>
        public static void Add(Form form)
        {
            form.index = forms.Count;
            formOrder.Add(forms.Count);
            forms.Add(form);
        }
        /// <summary>
        /// Sets the texture of a certain cursor.
        /// </summary>
        /// <param name="type">Cursor type.</param>
        /// <param name="tex">The texture.</param>
        public static void BindCursor(CursorType type, Texture2D tex)
        {
            cursorTexs[(int)type] = tex;
        }
        /// <summary>
        /// Sets the font of the GUI.
        /// </summary>
        /// <param name="font">The spritefont to use as the font.</param>
        public static void BindFont(SpriteFont font)
        {
            GUIHelper.font = font;
        }
        /// <summary>
        /// Draws the GUI.
        /// </summary>
        /// <param name="sb">Spritebatch that should be used to draw.</param>
        public static void Draw(SpriteBatch sb)
        {
            GUIHelper.sb = sb;
            foreach (int i in formOrder)
            {
                forms[i].Draw();
            }
            cursor.Draw();
            cursor = new Cursor(CursorType.NORMAL);
        }
        internal static void FlipCursor(bool horizontal)
        {
            cursor.flip = horizontal ? (byte)1 : (byte)2;
        }
        /// <summary>
        /// Initializes the GUI.
        /// </summary>
        /// <param name="game">The game to be used.</param>
        public static void Initialize(Game game)
        {
            GUI.game = game;
            Form.tbTex = GUIHelper.GenGradTex(new[] { new GradientPoint(new Color(34, 34, 34), 0), new GradientPoint(new Color(17, 17, 17), 28) }, Orientation.VERTICAL);
            GUIHelper.wTex = new Texture2D(game.GraphicsDevice, 1, 1);
            GUIHelper.wTex.SetData<Color>(new[] { Color.White });
        }
        internal static void SetCursor(CursorType type)
        {
            cursor.type = type;
        }
        /// <summary>
        /// Updates the GUI.
        /// </summary>
        public static void Update()
        {
            Input.Update();
            foreach (Form f in forms)
            {
                f.Update();
            }
        }
    }
}
