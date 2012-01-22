/* Copyright (C) 2012 by SkyForce (twice@eml.ru)
 * This work is licensed under the 
 * Creative Commons Attribution-NonCommercial-NoDerivs 3.0 Unported License. 
 * To view a copy of this license, visit 
 * http://creativecommons.org/licenses/by-nc-nd/3.0/ or send a letter to Creative Commons, 
 * 444 Castro Street, Suite 900, Mountain View, California, 94041, USA.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tan4ik
{
    public class LightEmmiter
    {
        public Vector3 position;
        public Vector3 color;
        public float corrector;
        public float radius;

        internal void UpdateEffect(EffectParameter effectParameter)
        {
            effectParameter.StructureMembers["position"].SetValue(position);
            effectParameter.StructureMembers["color"].SetValue(color * corrector);
            effectParameter.StructureMembers["invRadius"].SetValue(1f / radius);
        }
    }
}
