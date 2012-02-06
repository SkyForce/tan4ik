/* Copyright (C) 2012 by SkyForce (twice@eml.ru)
 * This work is licensed under the 
 * Creative Commons Attribution-NonCommercial-NoDerivs 3.0 Unported License. 
 * To view a copy of this license, visit 
 * http://creativecommons.org/licenses/by-nc-nd/3.0/ or send a letter to Creative Commons, 
 * 444 Castro Street, Suite 900, Mountain View, California, 94041, USA.
*/

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Tan4ik
{
    class Lights
    {

        float lightRadius;
        float lightZ;
        float lightC;
        EffectParameter lightParameter;
        LightEmmiter[] lights = new LightEmmiter[2];
        Texture2D back, normal;
        Effect deferred;

        public Lights(Texture2D back, Texture2D textureNormal, Effect def)
        {
            this.back = back;
            normal = textureNormal;
            deferred = def;

            lightRadius = 320f;
            lightZ = 50f;
            lightC = 1f;

            lights[0] = new LightEmmiter();
            lights[0].color = new Vector3(0f, 1f, 0f);
            lights[0].position = new Vector3(100, 400, lightZ);
            lights[0].radius = lightRadius;
            lights[0].corrector = lightC;

            lights[1] = new LightEmmiter();
            lights[1].color = new Vector3(1f, 0f, 0f);
            lights[1].position = new Vector3(500, 400, lightZ);
            lights[1].radius = lightRadius;
            lights[1].corrector = lightC;

            deferred.CurrentTechnique = deferred.Techniques["Deferred"];

            deferred.Parameters["screenWidth"].SetValue(800);
            deferred.Parameters["screenHeight"].SetValue(600);
            deferred.Parameters["ambientColor"].SetValue(new Vector3(1, 1, 1) * 0.1f);
            deferred.Parameters["numberOfLights"].SetValue(2);

            deferred.Parameters["normaltexture"].SetValue(textureNormal);

            lightParameter = deferred.Parameters["lights"];
        }

        public void Update(Vector2 pos1, Vector2 pos2)
        {
            for (int i = 0; i < lights.Length; i++)
            {
                LightEmmiter l = lights[i];
                l.UpdateEffect(lightParameter.Elements[i]);
            }
            lights[0].position = new Vector3(pos1.X, pos1.Y, lightZ);
            lights[1].position = new Vector3(pos2.X, pos2.Y, lightZ);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (EffectPass pass in deferred.CurrentTechnique.Passes)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);

                pass.Apply();
                spriteBatch.Draw(back, new Rectangle(0, 0, 800, 600), Color.White);
                spriteBatch.End();
            }
        }
    }
}
